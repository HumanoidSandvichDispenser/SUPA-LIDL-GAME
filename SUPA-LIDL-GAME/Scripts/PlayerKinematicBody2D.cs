using Godot;
using System;
using SupaLidlGame.State;

namespace SupaLidlGame
{
    public sealed class PlayerKinematicBody2D : HumanoidKinematicBody2D
    {
        private Sprite _characterSprite = null;

        private AnimationPlayer _animationPlayer = null;

        private AnimationTree _animationTree = null;

        private AnimationNodeStateMachinePlayback _animationState = null;

        private Utils.Inventory _inventory = null;

        public Utils.PlayerStats PlayerStats { get; private set; }

        public PlayerInputState InputState { get; set; } = PlayerInputState.None;

        public bool IsLanding { get; set; } = false;

        public bool IsAttacking { get; set; } = false;

        public bool IsOnAttackCooldown { get; set; } = false;

        private Vector2 _directionFacing = Vector2.Zero;

        public override void _Ready()
        {
            if (GlobalState.Player != null)
                throw new Exception("A Player entity already exists in the game!");
            GlobalState.Player = this;

            _characterSprite = GetNode("CharacterSprite") as Sprite;
            _animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
            _animationTree = GetNode("AnimationTree") as AnimationTree;
            _animationState = _animationTree.Get("parameters/playback") as
                    AnimationNodeStateMachinePlayback;
            _inventory = GetNode<Utils.Inventory>("Inventory");
            PlayerStats = GetNode("PlayerStats") as Utils.PlayerStats;

            _characterSprite.FlipH = false;
            _animationTree.Active = true;

            base._Ready();
        }

        public override void _Process(float delta)
        {
            GrabInput();
            base._Process(delta);
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_isOnFloor)
            {
                if ((InputState & PlayerInputState.Attacking) != 0)
                {
                    // this will freeze the player as it plays the animation
                    if (_inventory.SelectedItem is Items.Weapons.Weapon weapon)
                    {
                        Utils.WeaponStats stats;

                        // attempt to fire the weapon, then let us handle using the weapon
                        if (!((stats = weapon.TryAttack()) is null))
                        {
                            // we can attack
                            switch (stats.Type)
                            {
                                case Utils.WeaponType.None:
                                    // TODO: Replace with proper exception
                                    throw new Exception();
                                case Utils.WeaponType.Melee:
                                    System.Diagnostics.Debug.WriteLine("Attacking");
                                    _animationState.Travel("PlayerMelee");
                                    break;
                                case Utils.WeaponType.Ranged:
                                    break;
                                case Utils.WeaponType.Magic:
                                    break;
                            }
                        }
                    }
                    
                }
                else if (Direction.x == 0 || Velocity.x == 0)
                {
                    _animationState.Travel("PlayerIdle");
                }
                else
                {
                    _animationState.Travel("PlayerWalk");
                }
            }
            else
            {
                if (Velocity.y < 0)
                {
                    _animationState.Travel("PlayerJumping");
                }
                else if (Velocity.y > 0)
                {
                    _animationState.Travel("PlayerFalling");
                }
            }


            IsMovementFrozen = IsAttacking || IsLanding;

            // process movement
            base._PhysicsProcess(delta);

            // detect velocity before landing
            bool isJustLanding = _isOnFloor && PreviousVelocity.y > 0;
            bool isHardLand = isJustLanding && PreviousVelocity.y > 480;

            // hardland
            if (isHardLand && PlayerStats != null)
            {
                PlayerStats.Health -= (PreviousVelocity.y - 440) / 4;
            }

            _animationTree.Set("parameters/conditions/is_hard_land", isHardLand);
            _animationTree.Set("parameters/conditions/is_soft_land", isJustLanding && !isHardLand);
            _animationTree.Set("parameters/conditions/is_y_velocity_positive", Velocity.y > 0);

            if (Direction.x != 0 && !IsMovementFrozen)
            {
                if (Direction.x < 0)
                {
                    // use Scale instead of FlipX so children will also be
                    // flipped (e.g. sword sprite)
                    _characterSprite.Scale = new Vector2(-1, 1);
                    //_characterSprite.FlipH = true;
                    _inventory.Scale = new Vector2(-1, 1);
                   _directionFacing = Vector2.Left;
                }
                else if (Direction.x > 0)
                {
                    _characterSprite.Scale = new Vector2(1, 1);
                    //_characterSprite.FlipH = false;
                    _inventory.Scale = new Vector2(1, 1);
                   _directionFacing = Vector2.Right;
                }
            }
        }

        private void GrabInput()
        {
            // get direction based on input
            _direction = Vector2.Zero;
            InputState = PlayerInputState.None;

            if (Input.IsActionPressed("move_left"))
            {
                _direction.x -= 1;
            }

            if (Input.IsActionPressed("move_right"))
            {
                _direction.x += 1;
            }

            if (Input.IsActionJustPressed("jump"))
            {
                System.Diagnostics.Debug.WriteLine("Pressed jump");
                _direction.y += 1;
            }

            if (Input.IsActionPressed("attack"))
            {
                InputState |= PlayerInputState.Attacking;
            }

            if (Input.IsActionJustPressed("attack2"))
            {
                //var sword = ResourceLoader.Load<PackedScene>("res://Scripts/Items/Weapons/BaseSword.tscn");
                var sword = GD.Load<PackedScene>("res://Scripts/Items/Weapons/BaseSword.tscn");
                var instance = sword.Instance<Items.Weapons.BaseSword>();
                _inventory.AddItem(instance);
                _inventory.SelectedItem = instance;
            }
        }

        public void Push(float magnitude)
        {
            Vector2 direction = new Vector2(_directionFacing.x, 0);
            ApplyImpulse(direction * magnitude);
        }

        public void _on_PlayerStats_HealthChanged(float oldHealth, float newHealth)
        {
            if (!IsDead)
            {
                ShowDamageText(oldHealth - newHealth);
            }

            if (newHealth <= 0)
            {
                Die();
                System.Diagnostics.Debug.WriteLine("You died!");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"You lost {oldHealth - newHealth} health!");
            }
        }

        public void _on_Hitbox_ReceivedDamage(float damage,
                KinematicBody2D attacker,
                float knockback,
                Vector2 knockbackOrigin = default,
                Vector2 knockbackVector = default)
        {
            if (attacker is Entities.Enemy)
            {
                PlayerStats.Health -= damage;

                Vector2 direction;
                if (knockbackOrigin == default)
                {
                    if (knockbackVector == default)
                    {
                        // direction to player
                        direction = (GlobalPosition -
                                attacker.GlobalPosition);
                    }
                    else
                    {
                        direction = knockbackVector;
                    }
                }
                else
                {
                    direction = knockbackOrigin - GlobalPosition;
                }

                // launch the player up a bit
                direction = direction.Normalized() + Vector2.Up / 2;

                // yes, we're normalizing twice
                ApplyImpulse(direction * knockback, true);
            }
        }
    }
}
