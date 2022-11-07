using Godot;
using System;
using System.Collections.Generic;

namespace SupaLidlGame.Utils.Spawners
{
    public class Spawner : Position2D
    {
        //protected CollisionShape2D _shape;

        [Export]
        public List<PackedScene> Actors = new List<PackedScene>();

        public override void _Ready()
        {
            SetProcess(false);
            SetPhysicsProcess(false);
            //_shape = GetNode<CollisionShape2D>("Shape");
        }

        /// <summary>
        /// Returns
        /// </summary>
        public virtual bool IsInRange(Vector2 position)
        {
            //RectangleShape2D rect = _shape.Shape as RectangleShape2D;

            //float x1, x2, y1, y2;
            //x1 = GlobalPosition.x - rect.Extents.x;
            //x2 = GlobalPosition.x + rect.Extents.x;
            //y1 = GlobalPosition.y - rect.Extents.y;
            //y2 = GlobalPosition.y + rect.Extents.y;

            //Vector2 mag;

            //if (position.x < x1)
            //    mag.x = x1;
            //else if (position.x > x2)
            //    mag.x = x2;
            //else
            //    mag.x = position.x;

            //if (position.y < y1)
            //    mag.y = y1;
            //else if (position.y > y2)
            //    mag.y = y2;
            //else
            //    mag.y = position.y;

            float distSq = position.DistanceSquaredTo(GlobalPosition);
            System.Diagnostics.Debug.WriteLine(distSq.ToString());
            // spawns from 256 to 512 units away
            return distSq >= 65536 && distSq <= 262144;
        }

        public virtual void SpawnRandomActor()
        {
            if (Actors.Count < 1)
                return;

            System.Diagnostics.Debug.WriteLine("Please implement actor spawner");

            // Add logic that spawns a generic enemy
            // Spawn logic should be expected to spawn any appropriate enemy anywhere
        }

        protected virtual void SpawnActor()
        {

        }
    }
}
