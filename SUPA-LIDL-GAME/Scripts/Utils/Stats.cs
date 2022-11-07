using Godot;

namespace SupaLidlGame.Utils
{
    public class Stats : Node
    {
        protected float _maxHealth = 100;

        protected float _health = 100;

        protected bool _godmode = false;

        [Export]
        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                _maxHealth = value;
            }
        }

        [Export]
        public float Health
        {
            get => _health;
            set
            {
                if (value != _health)
                {
                    if (Godmode)
                    {
                        _health = MaxHealth;
                    }
                    else
                    {
                        float oldHealth = _health;
                        _health = value;
                        EmitSignal("HealthChanged", oldHealth, _health);
                    }
                }
            }
        }

        [Export]
        public bool Godmode
        {
            get => _godmode;
            set
            {
                _godmode = value;
                if (_godmode)
                {
                    _health = MaxHealth;
                }
            }
        }

        [Signal]
        // DansGame godot naming conventions
        public delegate void HealthChanged(float oldHealth, float newHealth);
    }
}
