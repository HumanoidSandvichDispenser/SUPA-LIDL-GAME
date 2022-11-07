using Godot;

namespace SupaLidlGame.Entities.Projectiles
{
    public interface IProjectile
    {
        public Vector2 Velocity { get; set; }

        public Vector2 Direction { get; set; }

        public float InitialVelocityMagnitude { get; set; }

        public float Gravity { get; set; }

        public float Damage { get; set; }

        public float Knockback { get; set; }

        public float Lifetime { get; set; }

        public KinematicBody2D Inflictor { get; set; }
    }
}
