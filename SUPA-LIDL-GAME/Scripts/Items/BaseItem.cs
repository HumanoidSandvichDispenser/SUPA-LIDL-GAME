using Godot;

namespace SupaLidlGame.Items
{
    public abstract class Item : Node2D
    {
        [Export]
        public string ItemName { get; set; }
        
        [Export]
        public string Description  { get; set; }

        public abstract void Equip();

        public abstract void Unequip();
    }
}
