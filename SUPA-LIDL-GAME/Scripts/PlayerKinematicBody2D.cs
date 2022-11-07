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

        private Vector2 _directionFacing = Vector2.Right;

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
            PlayerStats = GetNode<Utils.PlayerStats>("PlayerStats");

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
            if ((InputState & PlayerInputState.Attacking) != 0)
            {
                // this will freeze the player as it plays the animation
                if (_inventory.SelectedItem is Items.Weapons.Weapon weapon)
                {
                    Vector2 mousePosition = GetLocalMousePosition();
                    Utils.WeaponStats stats = weapon.TryAttack(mousePosition);

                    // we can attack
                    if (!(stats is null))
                    {
                        // face towards the direction we are attacking
                        if (mousePosition.x < 0)
                        {
                            Direction = Vector2.Left;
                        }
                        else
                        {
                            Direction = Vector2.Right;
                        }

                        FaceDirection(Direction);

                        switch (stats.Type)
                        {
                            case Utils.WeaponType.None:
                                // TODO: Replace with proper exception
                                throw new Exception();
                            case Utils.WeaponType.Melee:
                                if (Velocity.y > 0)
                                {
                                    GD.Print(Velocity);
                                    Velocity = Vector2.Zero;
                                }
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
            else if (_isOnFloor)
            {
                if (Direction.x == 0 || Velocity.x == 0)
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
            _animationTree.Set("parameters/conditions/is_not_attacking", !IsAttacking);

            FaceDirection(Direction);
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
                instance.Inflictor = this;
                _inventory.AddItem(instance);
                _inventory.SelectedItem = instance;
            }
        }

        public override void _Input(InputEvent @event)
        {
            // this is for testing
            if (@event is InputEventMouseMotion motion)
            {
                Target = GetLocalMousePosition();
                float blend = Target.Normalized().y;
                if (System.Math.Abs(blend) < 0.5f)
                {
                    blend = 0;
                }
                _animationTree.Set("parameters/PlayerMelee/blend_position",
                        blend);
            }
        }

        public void Push(float magnitude)
        {
            ApplyImpulse(_directionFacing * magnitude);
        }

        private void FaceDirection(Vector2 direction)
        {
            if (direction.x != 0 && !IsMovementFrozen)
            {
                if (direction.x < 0)
                {
                    // use Scale instead of FlipX so children will also be
                    // flipped (e.g. sword sprite)
                    _characterSprite.Scale = new Vector2(-1, 1);
                    //_characterSprite.FlipH = true;
                    _inventory.Scale = new Vector2(-1, 1);
                   _directionFacing = Vector2.Left;
                }
                else if (direction.x > 0)
                {
                    _characterSprite.Scale = new Vector2(1, 1);
                    //_characterSprite.FlipH = false;
                    _inventory.Scale = new Vector2(1, 1);
                   _directionFacing = Vector2.Right;
                }
            }
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
            GD.Print("Received damage");
            //if (attacker is Entities.Enemy)
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
