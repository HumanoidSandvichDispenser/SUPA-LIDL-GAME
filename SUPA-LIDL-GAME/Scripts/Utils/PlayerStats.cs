using Godot;

namespace SupaLidlGame.Utils
{
    public class PlayerStats : Stats
    {
        [Export]
        public PlayerClass Class { get; set; } = PlayerClass.Melee;

        [Export]
        public int MaxJumps { get; set; } = 1;

        public int CurrentJumps { get; set; } = 0;
    }
}
