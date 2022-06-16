using Godot;

namespace SupaLidlGame.Utils
{
    public struct DamageInfo
    {
        public float Damage { get; set; }
        public float KnockbackForce { get; set; }
        public KinematicBody2D Owner { get; set; }
    }
}
