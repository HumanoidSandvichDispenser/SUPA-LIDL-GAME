namespace SupaLidlGame.BoundingBoxes
{
    public class PlayerHitbox : Hitbox
    {
        public override void _Ready()
        {
            _stats = GetNode<Utils.PlayerStats>("../PlayerStats");
        }
    }
}
