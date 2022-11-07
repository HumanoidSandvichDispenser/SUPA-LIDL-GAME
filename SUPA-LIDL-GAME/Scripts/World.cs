using Godot;
using System;

namespace SupaLidlGame
{
    public class World : Node
    {
        private Node _enemies;

        public int MaxEnemyCount { get; set; } = 15;

        private float _spawnDelta = 0;

        public override void _Ready()
        {
            _enemies = GetNode<Node>("Enemies");
        }

        public override void _Process(float delta)
        {
            if ((_spawnDelta += delta) > 1)
            {
                _spawnDelta = 0;
                foreach (var child in GetNode<Node>("Spawners").GetChildren())
                {
                    if (child is Utils.Spawners.Spawner spawner)
                    {
                        MaybeSpawnEnemy(spawner);
                    }
                    else
                    {
                        System.Diagnostics.Debug.Print("Non-spawner exists under Spawners node.");
                    }
                }
            }
        }

        private void MaybeSpawnEnemy(Utils.Spawners.Spawner spawner)
        {
            float distSq = GlobalState.Player.Position
                .DistanceSquaredTo(spawner.GlobalPosition);
            System.Diagnostics.Debug.WriteLine(distSq.ToString());

            // spawns from 256 to 512 units away
            if (distSq < 65536 || distSq > 262144)
            {
                return;
            }

            // chances of a mob spawning at this spawner
            int enemyCount = _enemies.GetChildCount();
            if (enemyCount < MaxEnemyCount)
            {
                // -1/4 ln((x + 1)/(m + 1))
                //double chance = -0.25 * Math.Log((double)(enemyCount + 1) / (MaxEnemyCount + 1));
                // -1/30 (x - m)
                double chance = (double)(MaxEnemyCount - enemyCount) / 60;
                if (new Random().NextDouble() < chance)
                {
                    //SpawnEnemy(spawner);
                    spawner.SpawnRandomActor();
                }
            }
        }

        private void SpawnEnemy(Utils.Spawners.Spawner spawner)
        {
            var bat = GD.Load<PackedScene>("res://Objects/Actors/Bat.tscn");
            var instance = bat.Instance<Entities.Bat>();
            _enemies.AddChild(instance);
            instance.GlobalPosition = spawner.GlobalPosition;
        }
    }
}
