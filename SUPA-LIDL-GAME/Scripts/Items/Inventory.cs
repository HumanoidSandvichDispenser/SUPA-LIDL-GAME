using Godot;
using System.Collections.Generic;

namespace SupaLidlGame.Items
{
    public class Inventory : Node
    {
        public List<Item> Items { get; private set; }

        public override void _Ready()
        {

        }

        public Item DropItem(Item item)
        {
            Items.Remove(item);

            // TODO: Item dropping functionality (spawning in the world)

            return item;
        }
    }
}
