using Godot;
using System;

namespace SupaLidlGame.Entities
{
    public class FeelsDankCube : Enemy
    {
        public override void _Ready()
        {
            base._Ready();
        }

        public override void Die()
        {
            QueueFree();
        }

        public override void Think()
        {
            PlayerKinematicBody2D player = GlobalState.Player;
            Vector2 moveTo = player.GlobalPosition - GlobalPosition;
            _direction = moveTo.Normalized();
        }
    }
}
