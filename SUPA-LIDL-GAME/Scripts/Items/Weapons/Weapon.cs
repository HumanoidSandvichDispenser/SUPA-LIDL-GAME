using Godot;
using System.Diagnostics;

namespace SupaLidlGame.Items.Weapons
{
    public abstract class Weapon : Item
    {
        protected float _fireTimeLeft = 0;

        public bool CanStartAttack => _fireTimeLeft <= 0;

        public Utils.WeaponStats WeaponStats { get; protected set; }

        [Signal]
        public delegate void Attack(Weapon weapon, Utils.WeaponStats weaponStats);

        public override void _Ready()
        {
            WeaponStats = GetNode<Utils.WeaponStats>("WeaponStats");
        }

        public override void _Process(float delta)
        {
            if (_fireTimeLeft > 0)
            {
                _fireTimeLeft -= delta;
            }
        }

        /// <summary>
        /// Attempts to start attack.
        /// </summary>
        /// <returns>
        /// <see cref="WeaponStats"/> if <see cref="CanStartAttack"/> is <see
        /// langword="true"/>
        /// </returns>
        public Utils.WeaponStats TryAttack()
        {
            if (CanStartAttack)
            {
                return WeaponStats;
            }
            return null;
        }

        protected virtual void StartAttack()
        {

        }

        protected virtual void StopAttack()
        {

        }
    }
}
