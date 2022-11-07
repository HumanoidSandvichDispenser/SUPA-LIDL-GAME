using Godot;

namespace SupaLidlGame.Utils
{
    public class WeaponStats : Resource
    {
        [Export]
        public float Damage { get; set; } = 0;

        /// <summary>
        /// Time it takes to reuse/refire a weapon.
        /// </summary>
        [Export]
        public float UseTime { get; set; } = 0;

        /// <summary>
        /// Duration the damagebox will be active. Only applies to melee weapons.
        /// </summary>
        [Export]
        public float DamageTime { get; set; } = 0.1f;

        /// <summary>
        /// Range of the weapon (size of the damagebox).
        /// </summary>
        [Export]
        public float Range { get; set; } = 0;

        /// <summary>
        /// Magnitude of the knockback force of the weapon.
        /// </summary>
        [Export]
        public float Knockback { get; set; } = 0;

        /// <summary>
        /// Initial velocity of any projectile spawned by the weapon.
        /// </summary>
        [Export]
        public float Velocity { get; set; } = 0;

        [Export]
        public WeaponType Type { get; set; } = WeaponType.None;
    }
}
