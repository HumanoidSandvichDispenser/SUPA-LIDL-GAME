namespace SupaLidlGame.Entities
{
    public class FeelsDankCube : Enemy
    {
        public override void _Ready()
        {
            _thinker = new AI.BounceThinker();
            base._Ready();
        }

        public override void Die()
        {
            IsDead = true;
            // TODO: Death animation
            QueueFree();
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
        }
    }
}
