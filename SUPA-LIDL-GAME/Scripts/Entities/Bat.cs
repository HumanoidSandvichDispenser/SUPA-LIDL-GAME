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
            _deathParticles.Emit();
            RemoveChild(_deathParticles);
            GetTree().Root.AddChild(_deathParticles);
            _deathParticles.GlobalPosition = GlobalPosition;
            IsDead = true;
            //GetNode<BoundingBoxes.Damagebox>("EnemyDamagebox").Enabled = false;
            QueueFree();
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
        }
    }
}
