using Godot;

namespace SupaLidlGame.Entities.AI
{
    public class JumpKingThinker : Thinker
    {
        private float _resetDirectionTime = 0;

        public override void Think(float delta, Enemy enemy)
        {
            if (_resetDirectionTime <= 0)
            {
            }
            else
            {
                _resetDirectionTime -= delta;
            }

            KinematicCollision2D coll = enemy.GetLastSlideCollision();
            if (coll is null)
            {

            }
        }
    }
}
