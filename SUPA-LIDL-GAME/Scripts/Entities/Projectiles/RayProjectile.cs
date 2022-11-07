using Godot;
using System.Collections.Generic;
using SupaLidlGame.BoundingBoxes;

namespace SupaLidlGame.Entities.Projectiles
{
    public class RayProjectile : Node2D, IProjectile
    {
        protected HashSet<Hitbox> _ignoreList = new HashSet<Hitbox>();

        public Vector2 Velocity { get; set; }

        [Export]
        public Vector2 Direction { get; set; }

        [Export]
        public float InitialVelocityMagnitude { get; set; }

        [Export]
        public float Gravity { get; set; }

        [Export]
        public float Damage { get; set; }

        [Export]
        public float Knockback { get; set; }

        [Export]
        public KinematicBody2D Inflictor { get; set; }

        [Export]
        public float Lifetime { get; set; }

        /// <summary>
        /// Does nothing at the moment
        /// </summary>
        [Export]
        public bool MoveToDirection { get; set; } = false;

        /// <summary>
        /// If <see langword="true"/>, the projectile will change its
        /// orientation to the direction of its velocity.
        /// </summary>
        [Export]
        public bool RotateToVelocity { get; set; } = false;

        protected RayCast2D _rayCast;

        protected Sprite _sprite;

        protected float _currentLifetime;

        public override void _Ready()
        {
            Velocity = Direction.Resized(InitialVelocityMagnitude);
            _rayCast = GetNode<RayCast2D>("RayCast2D");
            _sprite = GetNode<Sprite>("Sprite");
            _currentLifetime = Lifetime;
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_currentLifetime <= 0)
            {
                QueueFree();
            }

            _currentLifetime -= delta;
            var spaceState = GetWorld2d().DirectSpaceState;

            Velocity += Gravity * Vector2.Down * delta;

            // cast vector = displacement for one frame
            var deltaDisplacement = Velocity * delta;
            //uint collisionLayer = 1 + 4;

            /*
            var result = spaceState.IntersectRay(GlobalPosition,
                    GlobalPosition + deltaDisplacement,
                    collisionLayer: collisionLayer,
                    collideWithAreas: true);

            if (result.Count > 0)
            {
                var collider = result["collider"];
                if (collider is Hitbox hitbox)
                {
                    if (!_ignoreList.Contains(hitbox))
                    {
                        _ignoreList.Add(hitbox);
                        hitbox.InflictDamage(Damage,
                                Inflictor,
                                Knockback,
                                knockbackVector: deltaDisplacement.Normalized());
                    }
                }
            }
            */

            _rayCast.CastTo = deltaDisplacement;

            // inflict dmg on all entities that the projectile's raycast hits
            if (_rayCast.IsColliding())
            {
                if (_rayCast.GetCollider() is Hitbox hitbox)
                {
                    if (!_ignoreList.Contains(hitbox))
                    {
                        _ignoreList.Add(hitbox);
                        hitbox.InflictDamage(Damage,
                                null,
                                Knockback,
                                knockbackVector: deltaDisplacement.Normalized());
                    }
                }
            }

            Position += deltaDisplacement;

            _sprite.FlipH = deltaDisplacement.x < 0;

            if (RotateToVelocity)
            {
                if (_sprite.FlipH)
                {
                    Rotation = Godot.Mathf.Atan2(-deltaDisplacement.y,
                            -deltaDisplacement.x);
                }
                else
                {
                    Rotation = Godot.Mathf.Atan2(deltaDisplacement.y,
                            deltaDisplacement.x);
                }
            }
        }
    }
}
