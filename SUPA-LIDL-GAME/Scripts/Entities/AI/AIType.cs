using Godot;

namespace SupaLidlGame.Entities.AI
{
    public enum AIType
    {
        /// <summary>
        /// Moves towards the player normally.
        /// </summary>
        Basic,
        /// <summary>
        /// AI can only jump towards the player, similar to slimes in Terraria. Incompatible with flying AI.
        /// </summary>
        JumpKing
    }
}
