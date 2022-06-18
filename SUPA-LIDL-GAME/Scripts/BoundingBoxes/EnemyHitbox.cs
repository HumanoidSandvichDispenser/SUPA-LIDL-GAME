namespace SupaLidlGame.BoundingBoxes
{
    public class EnemyHitbox : Hitbox
    {
        public Entities.Enemy EnemyParent => GetParent<Entities.Enemy>();

        public override void _Ready()
        {
            _stats = GetParent().GetNode<Utils.Stats>("Stats");
        }
    }
}
