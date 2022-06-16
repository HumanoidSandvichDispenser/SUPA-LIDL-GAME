namespace SupaLidlGame.BoundingBoxes
{
    public class EnemyHitbox : Hitbox
    {
        public Enemy EnemyParent => GetParent<Enemy>();

        public override void _Ready()
        {
            _stats = GetParent().GetNode<Utils.Stats>("Stats");
        }
    }
}
