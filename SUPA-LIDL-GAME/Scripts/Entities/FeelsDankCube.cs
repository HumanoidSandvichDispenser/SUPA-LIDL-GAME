using Godot;
using System;

namespace SupaLidlGame
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
    }
}
