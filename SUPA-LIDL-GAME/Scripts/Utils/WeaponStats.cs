using Godot;

namespace SupaLidlGame
{
    public class WeaponStats : Node
    {
        public float Damage { get; set; } = 0;

        /// <summary>
        /// Delay after using a weapon before being able to use it again.
        /// </summary>
        public float UseTime { get; set; } = 0;
    }
}
