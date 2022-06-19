namespace SupaLidlGame.Entities.AI
{
    public class Thinker
    {
        /// <summary>
        /// Thinks for the AI. Determines the direction and state of the AI.
        /// </summary>
        public virtual void Think(float delta, Enemy enemy)
        {
            PlayerKinematicBody2D plr = GlobalState.Player;
            enemy.Direction = plr.GlobalPosition - enemy.GlobalPosition;
        }
    }
}
