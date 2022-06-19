using Godot;
using System;
using System.Diagnostics;

namespace SupaLidlGame
{
    public class HumanoidKinematicBody2D : KinematicBody2D
    {
        public const float GRAVITY_CONSTANT = 570;

        public const float TERMINAL_VELOCITY = 2048;

        public const float JUMP_VELOCITY = 280;

        protected bool _isOnFloor = false;

        protected Vector2 _direction = Vector2.Zero;

        protected Vector2 _velocity = Vector2.Zero;

        protected KinematicCollision2D _kinematicCollision;

        [Export]
        public float AccelerationMagnitude { get; set; } = 768;

        [Export]
        public float FrictionCoefficient { get; set; } = 512;

        [Export]
        public float MaxSpeed { get; set; } = 128;

        [Export]
        public bool IsFlyingBody { get; set; } = false;

        [Export]
        public float Mass { get; set; } = 1;

        public bool IsDead { get; set; } = false;

        public Vector2 PreviousVelocity { get; protected set; } = Vector2.Zero;

        public bool IsMovementFrozen { get; protected set; } = false; // LUL

        /// <summary>
        /// Calculated by direction times acceleration factor plus acceleration
        /// due to gravity. Friction is not considered in the final
        /// acceleration. forsenScoots
        /// </summary>
        public Vector2 Acceleration => _direction.Normalized() *
                AccelerationMagnitude;

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

        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _PhysicsProcess(float delta)
        {
            if (IsDead)
            {
                base._PhysicsProcess(delta);
                return;
            }

            if (IsMovementFrozen)
            {
                Direction = Vector2.Zero;
            }

            Vector2 snap = Vector2.Down * 8;

            PreviousVelocity = _velocity;

            if (IsFlyingBody)
            {
                _velocity = Fly(delta, _velocity);
                snap = Vector2.Zero;
            }
            else
            {
                _velocity = Walk(delta, _velocity, ref snap);
            }

            _velocity = MoveAndSlideWithSnap(_velocity, snap, Vector2.Up, stopOnSlope: true);
            _kinematicCollision = GetLastSlideCollision();

            _isOnFloor = IsOnFloor();
        }

        [Obsolete]
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

        protected Vector2 Walk(float delta, Vector2 velocity, ref Vector2 snap)
        {
            // accelerate in the x direction
            if (Direction.x != 0)
            {
                velocity.x += Acceleration.x * delta;

                // clamp
                velocity.x = Math.Max(Math.Min(velocity.x, MaxSpeed), -MaxSpeed);
            }
            else
            {
                Vector2 finalVelocity = new Vector2(0, velocity.y);
                velocity.x = velocity.MoveToward(finalVelocity, FrictionCoefficient * delta).x;
            }

            // accelerate in the y direction from jumping
            if (Direction.y > 0)
            {
                if (_isOnFloor)
                {
                    snap = Vector2.Zero;
                    velocity.y = Vector2.Up.y * JUMP_VELOCITY;
                }
            }

            // accelerate in the y direction due to gravity
            velocity.y += Gravity.y * delta;
            velocity.y = Math.Max(velocity.y, -TERMINAL_VELOCITY);
            return velocity;
        }

        protected Vector2 Fly(float delta, Vector2 velocity)
        {
            // accelerate in the x and y directions

            // if we are traveling faster than max velocity, there is probably
            // an external force acting upon us.
            if (velocity.LengthSquared() > Math.Pow(MaxSpeed, 2) + 1)
            {
                // we have this statement in both conditions because we only
                // want to add the velocity AFTER the comparison but BEFORE
                // decelerating.
                //velocity += Acceleration * delta;

                // decelerate to max speed
                velocity = velocity.MoveToward(velocity.Normalized() * MaxSpeed,
                        FrictionCoefficient * delta);

                // if velocity is slightly larger than MaxSpeed, then clamp the velocity
                if (velocity.LengthSquared() < Math.Pow(MaxSpeed, 2) + 1)
                {
                    velocity = velocity.Clamped(MaxSpeed);
                }
            }

            // otherwise, we move in the direction we want to move and clamp it
            // to max speed (so it doesn't accelerate faster than max speed)
            else
            {
                velocity += Acceleration * delta;
                velocity = velocity.Clamped(MaxSpeed);
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
        /// Kills the humanoid entitiy
        /// </summary>
        public virtual void Die()
        {
            IsDead = true;
        }
    }
}
