using Godot;
using System;
using System.Collections.Generic;
using SupaLidlGame.Extensions;

namespace SupaLidlGame.Utils.Spawners
{
    public class ForestSpawner : Spawner
    {
        private static Random _random = new Random();

        public override void _Ready()
        {
            Actors.Add(GD.Load<PackedScene>("res://Objects/Actors/Bat.tscn"));

            base._Ready();
        }

        public override void SpawnRandomActor()
        {
            if (Actors.Count < 1)
                return;

            // get extents/ranges of the spawn area
            /*
            RectangleShape2D rect = _shape.Shape as RectangleShape2D;
            float x1, x2, y1, y2;
            x1 = GlobalPosition.x - rect.Extents.x;
            x2 = GlobalPosition.x + rect.Extents.x;
            y1 = GlobalPosition.y - rect.Extents.y;
            y2 = GlobalPosition.y + rect.Extents.y;

            // generate random position
            float x, y;
            x = (float)((x2 - x1) * _random.NextDouble() + x1);
            y = (float)((y2 - y1) * _random.NextDouble() + y1);
            */

            var actor = Actors.PickRandomElement();
            var instance = actor.Instance<HumanoidKinematicBody2D>();
            AddChild(instance);
            instance.GlobalPosition = GlobalPosition;
        }
    }
}
