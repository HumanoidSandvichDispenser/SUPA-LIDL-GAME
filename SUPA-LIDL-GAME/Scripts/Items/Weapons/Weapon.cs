using Godot;
using System.Collections.Generic;
using SupaLidlGame.Extensions;

namespace SupaLidlGame.Items.Weapons
{
    public abstract class Weapon : Item
    {
        protected float _useTimeLeft = 0;

        protected float _damageTimeLeft = 0;

        public bool CanStartAttack => _useTimeLeft <= 0;

        [Export]
        public Utils.WeaponStats WeaponStats { get; protected set; }

        public KinematicBody2D Inflictor { get; set; }

        public List<WeaponMods.WeaponMod> Mods { get; protected set; }

        public override void _Ready()
        {
            base._Ready();

            Mods = new List<WeaponMods.WeaponMod>();
            Equip();
        }

        public override void _Process(float delta)
        {
            if (_useTimeLeft <= 0)
            {
                StopAttack();
            }
            else
            {
                _useTimeLeft -= delta;
            }

            if (_damageTimeLeft <= 0)
            {
                StopDamage();
            }
            else
            {
                _damageTimeLeft -= delta;
            }
        }

        /// <summary>
        /// Attempts to start attack.
        /// </summary>
        /// <returns>
        /// <see cref="WeaponStats"/> if <see cref="CanStartAttack"/> is <see
        /// langword="true"/>
        /// </returns>
        public Utils.WeaponStats TryAttack(Vector2 direction)
        {
            if (CanStartAttack)
            {
                StartAttack(direction);
                _useTimeLeft = WeaponStats.UseTime;
                return WeaponStats;
            }

            return default;
        }

        public override void Equip()
        {
            // if the inflictor was not explicitly set, use the first parent
            // that is a HumanoidKinematicBody2D
            if (Inflictor is null)
            {
                Inflictor = this.GetFirstParentOfClass<HumanoidKinematicBody2D>();
            }
        }

        public override void Unequip()
        {
            Inflictor = null;
        }

        protected abstract void StartAttack(Vector2 direction);

        protected abstract void StopAttack();

        /// <summary>
        /// Stops weapon damage.
        /// </summary>
        /// <remarks>
        /// This method should only be called from <see cref="StartAttack()"/>
        /// </remarks>
        protected virtual void StopDamage()
        {

        }
    }
}
