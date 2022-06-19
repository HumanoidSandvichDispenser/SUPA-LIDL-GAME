using Godot;

namespace SupaLidlGame.Entities.AI
{
    public class BounceThinker : Thinker
    {
        private float _resetDirectionTime;
        private float _resetDirectionTimeLeft = 0;

        public BounceThinker(float resetDirectionTime = 0.5f)
        {
            _resetDirectionTime = resetDirectionTime;
        }

        public override void Think(float delta, Enemy enemy)
        {
            KinematicCollision2D coll = enemy.GetLastSlideCollision();

            if (!(coll is null))
            {
                Vector2 bounce = enemy.PreviousVelocity.Bounce(coll.Normal);
                enemy.ApplyImpulse(bounce.Normalized() * enemy.MaxSpeed, true);

                // bounce for 1 second before resetting direction back to player
                if (_resetDirectionTimeLeft <= 0)
                {
                    _resetDirectionTimeLeft = _resetDirectionTime;
                }
                else // if we perform multiple bounces, just add some extra time
                {
                    _resetDirectionTimeLeft += _resetDirectionTime / 4;
                }
                System.Diagnostics.Debug.WriteLine("Bounce!");
                enemy.Direction = Vector2.Zero;
            }
            else
            {
                // there is no collision
                
                if (_resetDirectionTimeLeft > 0)
                {
                    _resetDirectionTimeLeft -= delta;
                }
                else
                {
                    // move direction to player
                    base.Think(delta, enemy);
                }
            }
        }
    }
}
