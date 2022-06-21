using Godot;
using System;

namespace SupaLidlGame.BoundingBoxes
{
    public class EnemyDamagebox : Damagebox
    {
        private float _resetIgnoreListTimeLeft = 0;

        // Called when the node enters the scene tree for the first time.
        public override void _on_Damagebox_area_entered(Area2D area)
        {
            if (_resetIgnoreListTimeLeft <= 0)
            {
                ResetIgnoreList();

                if (_ignoreList.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Enemy hit!");
                    base._on_Damagebox_area_entered(area);

                    // reset ignorelist after 500ms
                    _resetIgnoreListTimeLeft = 0.5f;
                }
            }
        }

        public override void _Process(float delta)
        {
            _resetIgnoreListTimeLeft -= delta;

            base._Process(delta);
        }
    }
}
