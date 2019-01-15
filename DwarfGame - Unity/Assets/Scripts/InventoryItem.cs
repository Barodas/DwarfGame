using UnityEngine;

namespace DwarfGame
{
    [System.Serializable]
    public class InventoryItem
    {
        public Item Item;
        public int StackSize = 1;

        public Sprite ItemSprite => Item.ItemSprite;

        public InventoryItem(Item item, int stackSize = 1)
        {
            Item = item;
            StackSize = stackSize;
        }
    }
}