using Godot;
using System;

namespace SupaLidlGame
{
    public class HealthBar : Node
    {
        private float _health;

        private Label _label;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _label = GetNode<Label>("Label");
            GlobalState.Player.PlayerStats.Connect("HealthChanged", this,
                    "_on_PlayerStats_HealthChanged");
        }

        public void _on_PlayerStats_HealthChanged(float oldHealth, float newHealth)
        {
            _label.Text = Math.Ceiling(newHealth).ToString();
        }
    }
}
