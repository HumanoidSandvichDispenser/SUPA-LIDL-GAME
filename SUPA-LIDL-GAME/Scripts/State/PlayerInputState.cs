using System;

namespace SupaLidlGame.State
{
    [Flags]
    public enum PlayerInputState
    {
        None = 0,
        Lateral = 1,
        Vertical = 2,
        Attacking = 4
    }
}
