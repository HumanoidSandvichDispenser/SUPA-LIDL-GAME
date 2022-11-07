using Godot;
using System.Diagnostics;
using System.Collections.Generic;
using SupaLidlGame.Items;

namespace SupaLidlGame.Utils
{
    public class Inventory : Node2D
    {
        public List<Item> Items { get; private set; } = new List<Item>();

        public const int MAX_CAPAITY = 32;

        public bool IsFull => Items.Count >= MAX_CAPAITY;
        
        private Item _selectedItem;

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (Items.Contains(value))
                {
                    if (!(_selectedItem is null))
                    {
                        RemoveChild(_selectedItem);
                    }
                    _selectedItem = value;

                    // only equip if ready (set inflictor and other stuff)
                    // if the item is not ready, it will equip once it is ready
                    if (_selectedItem.IsReady)
                    {
                        _selectedItem.Equip();
                    }

                    AddChild(value);
                }
                else
                {
                    Debug.Fail("Tried to equip an item not in the inventory.");
                }
            }
        }

        public Item AddItem(Item item)
        {
            if (IsFull)
                return null;

            Items.Add(item);
            return item;
        }

        public Item DropItem(Item item)
        {
            if (!Items.Contains(item))
                return null;

            if (item == _selectedItem)
            {
                _selectedItem = null;
            }

            GetTree().Root.AddChild(item);
            return item;
        }

        public override void _Ready()
        {
        }
    }
}
