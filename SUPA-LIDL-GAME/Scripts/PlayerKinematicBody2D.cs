using Godot;
using System;
using SupaLidlGame.State;

namespace SupaLidlGame
{
    public class PlayerKinematicBody2D : HumanoidKinematicBody2D
    {
        private Sprite _characterSprite = null;
        
        private AnimationPlayer _animationPlayer = null;

        private AnimationTree _animationTree = null;

        private AnimationNodeStateMachinePlayback _animationState = null;

        private Utils.PlayerStats _playerStats;

        private BoundingBoxes.Damagebox _swordDamageBox;

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
            _playerStats = GetNode("PlayerStats") as Utils.PlayerStats;
            _swordDamageBox = GetNode<BoundingBoxes.Damagebox>(
                    "CharacterSprite/SwordSprite/Damagebox");

            // set the sword properties when we load player stats
            _swordDamageBox.Damage = _playerStats.SwordDamage;
            _swordDamageBox.Knockback = _playerStats.SwordKnockback;

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
                    _animationState.Travel("PlayerMelee");
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
            bool isJustLanding = _isOnFloor && _previousVelocity.y > 0;
            bool isHardLand = isJustLanding && _previousVelocity.y > 480;

            // hardland
            if (isHardLand && _playerStats != null)
            {
                _playerStats.Health -= (_previousVelocity.y - 440) / 4;
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
                   _directionFacing = Vector2.Left;
                }
                else if (Direction.x > 0)
                {
                    _characterSprite.Scale = new Vector2(1, 1);
                   _directionFacing = Vector2.Right;
                }
            }
        }

        protected void GrabInput()
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
        }

        public void Push(float magnitude)
        {
            Vector2 direction = new Vector2(_directionFacing.x, 0);
            ApplyImpulse(direction * magnitude);
        }

        public void _on_PlayerStats_HealthChanged(float oldHealth, float newHealth)
        {
            if (newHealth <= 0)
            {
                System.Diagnostics.Debug.WriteLine("You died!");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"You lost {oldHealth - newHealth} health!");
            }
        }

        public void _on_PlayerStats_SwordStatsChanged(Utils.PlayerStats stats)
        {
            _swordDamageBox.Damage = stats.SwordDamage;
            _swordDamageBox.Knockback = stats.SwordKnockback;
            
        }
    }
}
