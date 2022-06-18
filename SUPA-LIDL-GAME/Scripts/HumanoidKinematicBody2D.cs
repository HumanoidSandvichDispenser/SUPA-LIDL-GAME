using Godot;
using System;
using System.Diagnostics;
using SupaLidlGame.State;
using SupaLidlGame.Extensions;

namespace SupaLidlGame
{
    public class HumanoidKinematicBody2D : KinematicBody2D
    {
        public const float GRAVITY_CONSTANT = 570;

        public const float TERMINAL_VELOCITY = 2048;

        public const float JUMP_VELOCITY = 280;

        protected bool _isOnFloor = false;

        protected HumanoidState _humanoidState = HumanoidState.Idle;

        protected Vector2 _direction = Vector2.Zero;

        protected Vector2 _velocity = Vector2.Zero;

        protected Vector2 _previousVelocity = Vector2.Zero;

        public float AccelerationCoefficient { get; set; } = 768;

        public float FrictionCoefficient { get; set; } = 512;

        [Export]
        public float MaxSpeed { get; set; } = 128;

        [Export]
        public bool IsFlyingBody { get; set; } = false;

        [Export]
        public float Mass { get; set; } = 1;

        public bool IsDead { get; set; } = false;

        public bool IsMovementFrozen { get; protected set; } = false; // LUL

        /// <summary>
        /// Calculated by direction times acceleration factor plus acceleration
        /// due to gravity. Friction is not considered in the final
        /// acceleration. forsenScoots
        /// </summary>
        public Vector2 Acceleration => _direction * AccelerationCoefficient;

        public Vector2 Gravity => GRAVITY_CONSTANT * Vector2.Down;

        public Vector2 Direction
        {
            get => _direction;
            set => _direction = value;
        }

        public Vector2 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _humanoidState = HumanoidState.Move;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _PhysicsProcess(float delta)
        {
            if (IsDead)
            {
                base._PhysicsProcess(delta);
                return;
            }

            Vector2 snap = Vector2.Down * 8;

            _previousVelocity = _velocity;

            // apply movement from input
            if (!IsMovementFrozen)
            {
                _velocity = Accelerate(_velocity, delta);
                //_velocity.x = Direction.x * MaxSpeed;

                if (Direction.y > 0)
                {
                    if (_isOnFloor && !IsFlyingBody)
                    {
                        snap = Vector2.Zero;
                        _velocity.y = Vector2.Up.y * JUMP_VELOCITY;
                    }
                }
            }

            // apply friction
            if (IsFlyingBody)
            {
                Vector2 finalVelocity = new Vector2(_velocity.x, 0);
                _velocity.y = _velocity.MoveToward(finalVelocity, FrictionCoefficient * delta).y;
            }
            else // apply gravity normally
            {
                _velocity.y += Gravity.y * delta;
                _velocity.y = Math.Max(_velocity.y, -TERMINAL_VELOCITY);
            }

            _velocity = MoveAndSlideWithSnap(_velocity, snap, Vector2.Up, stopOnSlope: true);

            _isOnFloor = IsOnFloor();
        }

        protected Vector2 Accelerate(Vector2 velocity, float delta)
        {
            // accelerate and clamp to max speed
            velocity += (Acceleration * delta).Clamped(MaxSpeed);
            if (Direction.x != 0)
            {
                velocity.x += Acceleration.x * delta;
                velocity.x = Math.Max(Math.Min(velocity.x, MaxSpeed), -MaxSpeed);
            }
            else
            {
                Vector2 finalVelocity = new Vector2(0, velocity.y);
                velocity.x = velocity.MoveToward(finalVelocity, FrictionCoefficient * delta).x;
            }

            return velocity;
        }

        /// <summary>
        /// Applies an impulse. Velocity will be calculated based on impulse
        /// divided by the body's mass.
        /// </summary>
        /// <param name="impulse">Change in momentum expressed as a Vector2</param>
        /// <param name="resetVelocity">Reset the entitiy's velocity to zero before applying the impulse</param>
        public void ApplyImpulse(Vector2 impulse, bool resetVelocity = false)
        {
            if (resetVelocity)
            {
                _velocity = Vector2.Zero;
            }
            if (Mass > 0)
            {
                _velocity += impulse / Mass;
            }
        }

        /// <summary>
        /// Kills the enemy entitiy
        /// </summary>
        public virtual void Die()
        {
            IsDead = true;
        }
    }
}
