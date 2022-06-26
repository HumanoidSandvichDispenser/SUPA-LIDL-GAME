using Godot;

namespace SupaLidlGame.Utils
{
    public class PlayerStats : Stats
    {
        protected float _meleeRange = 100;

        protected float _swordDamage = 20;

        protected float _swordKnockback = 150;

        [Export]
        public float MeleeRange
        {
            get => _meleeRange;
            set
            {
                _meleeRange = value;
                EmitSignal("SwordStatsChanged", this);
            }
        }

        [Export]
        public float SwordDamage
        {
            get => _swordDamage;
            set
            {
                _swordDamage = value;
                EmitSignal("SwordStatsChanged", this);
            }
        }

        [Export]
        public float SwordKnockback
        {
            get => _swordKnockback;
            set
            {
                _swordKnockback = value;
                EmitSignal("SwordStatsChanged", this);
            }
        }


        [Signal]
        public delegate void SwordStatsChanged(PlayerStats stats);
    }
}
