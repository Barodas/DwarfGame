using UnityEngine;
using UnityEngine.Events;

namespace DwarfGame
{
    [CreateAssetMenu]
    public class Inventory : ScriptableObject
    {
        public IntEvent InventorySlotUpdated;
        public IntEvent InventorySelectedChanged;
        public InventoryItem[] ItemList;// TODO: We need better null checking around InventoryItems

        public int SelectedSlot { get; private set; }
        
        private void Awake()
        {
            InventorySlotUpdated = new IntEvent();
            InventorySelectedChanged = new IntEvent();
        }
        
        public bool AddItemToInventory(InventoryItem item)
        {
            // Add items to existing slots of same item
            for (int i = 0; i < ItemList.Length; i++)
            {
                if (ItemList[i].Item != null && ItemList[i].Item == item.Item)
                {
                    if (ItemList[i].StackSize + item.StackSize < item.Item.StackLimit)
                    {
                        ItemList[i].StackSize += item.StackSize;
                        item.StackSize = 0;
                        InventorySlotUpdated.Invoke(i);
                        return true;
                    }

                    int remainder = item.Item.StackLimit - ItemList[i].StackSize;
                    ItemList[i].StackSize = item.Item.StackLimit;
                    item.StackSize -= remainder;
                    InventorySlotUpdated.Invoke(i);
                }
            }
            
            // Add to next available slot
            for (int i = 0; i < ItemList.Length; i++)
            {
                if (ItemList[i].Item == null)
                {
                    ItemList[i] = item;
                    InventorySlotUpdated.Invoke(i);
                    return true;
                }
            }

            return false;
        }

        public void RemoveItemFromInventory(int slot, int amount = 1)
        {
            if (ItemList[slot].Item != null)
            {
                ItemList[slot].StackSize -= amount;
                if (ItemList[slot].StackSize <= 0)
                {
                    ItemList[slot].Item = null; // TODO: We need to be able to destroy the InventoryItem cleanly rather than destroying its elements
                }
                InventorySlotUpdated.Invoke(slot);
            }
        }

        public void ChangeSelectedSlot(int targetSlot)
        {
            
            SelectedSlot = (ItemList.Length + targetSlot) % ItemList.Length;
            InventorySelectedChanged.Invoke(SelectedSlot);
        }
        
        public void ClearInventory()
        {
            for (int i = 0; i < ItemList.Length; i++)
            {
                ItemList[i] = null;
                InventorySlotUpdated.Invoke(i);
            }
        }
    }

    [System.Serializable]
    public class IntEvent : UnityEvent<int>
    {
        
    }
}