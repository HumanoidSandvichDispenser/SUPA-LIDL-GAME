using Godot;

namespace SupaLidlGame.Items
{
    public abstract class Item : Node2D
    {
        [Export]
        public string ItemName { get; set; }
        
        [Export]
        public string Description  { get; set; }

        public bool IsReady { get; private set; } = false;

        public override void _Ready()
        {
            IsReady = true;
        }

        public abstract void Equip();

        public abstract void Unequip();
    }
}
