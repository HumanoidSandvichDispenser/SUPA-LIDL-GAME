using System;

namespace SupaLidlGame.State
{
    [Flags]
    public enum HumanoidState
    {
        Idle,
        Move = 0b0001,
        Jump = 0b0010,
        Dash = 0b0100,
        Attack = 0b1000,
        Hurt = 0b00010000,
    }
}
