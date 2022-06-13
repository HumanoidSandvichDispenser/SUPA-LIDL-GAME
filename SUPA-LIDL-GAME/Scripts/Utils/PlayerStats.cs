using Godot;

namespace SupaLidlGame.Utils
{
    public class PlayerStats : Stats
    {
        protected float _swordRange = 100;

        [Export]
        public float SwordRange
        {
            get => SwordRange;
            set
            {
                float oldValue = _swordRange;
                _swordRange = value;
                EmitSignal("SwordRangeChanged", oldValue, _swordRange);
            }
        }

        [Signal]
        public delegate void SwordRangeChanged(float oldSwordRange, float newSwordRange);
    }
}
