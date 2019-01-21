using System.Diagnostics.SymbolStore;
using UnityEngine;

namespace DwarfGame
{
    [System.Serializable]
    public class InventoryItem
    {
        public Item Item;
        public int StackSize;

        public Sprite ItemSprite => Item.ItemSprite;

        public InventoryItem(Item item, int stackSize = 1)
        {
            Item = item;
            StackSize = stackSize;
        }

        public bool UseItem(Vector2 targetPosition)
        {
            Item.Use(targetPosition);
            return --StackSize <= 0;
        }
        
        /// <summary>
        /// Combines 2 InventoryItem stacks. Returns true if inventoryItem.StackSize is 0.
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <returns></returns>
        public bool Combine(InventoryItem inventoryItem)
        {
            inventoryItem.StackSize = Add(inventoryItem.StackSize);
            if (inventoryItem.StackSize <= 0)
            {
                return true;
            }

            return false;
        }
        
        private int Add(int amount)
        {
            if (StackSize + amount < Item.StackLimit)
            {
                StackSize += amount;
                return 0;
            }
            else
            {
                int remainder = (StackSize + amount) - Item.StackLimit;
                StackSize = Item.StackLimit;
                return remainder;
            }
        }
    }
}