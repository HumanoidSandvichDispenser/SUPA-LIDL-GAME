using Godot;
using System;

namespace SupaLidlGame.Entities
{
    public class Bat : Enemy
    {
        public override void _Ready()
        {
            _thinker = new AI.BounceThinker();
            base._Ready();
        }

        public override void Die()
        {
            IsDead = true;
            QueueFree();
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
        }
    }
}
