using Godot;
using SupaLidlGame.Utils;

namespace SupaLidlGame.Items.WeaponMods
{
    public abstract class WeaponMod
    {
        public float Value { get; set; } = 0;

        public int SlotSpace { get; protected set; }

        public abstract string Description { get; }

        /// <summary>
        /// Modifies stats
        /// </summary>
        /// <param name="stats">
        /// A copy of the weapon stats to modify
        /// </param>
        /// <returns>
        /// Weapon stats
        /// </returns>
        public abstract WeaponStats ModifyStats(WeaponStats stats);

        public virtual void OnWeaponFire(Items.Weapons.Weapon weapon,
                KinematicBody2D attacker,
                Vector2 direction)
        {

        }
    }
}
