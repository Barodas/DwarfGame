using UnityEngine;
using UnityEngine.Events;

namespace DwarfGame
{
    [CreateAssetMenu]
    public class Inventory : ScriptableObject
    {
        public IntEvent InventorySlotUpdated;
        public IntEvent InventorySelectedChanged;
        public Item[] ItemList;

        public int SelectedSlot { get; private set; }
        
        private void Awake()
        {
            InventorySlotUpdated = new IntEvent();
            InventorySelectedChanged = new IntEvent();
        }
        
        public bool AddItemToInventory(Item item)
        {
            for (int i = 0; i < ItemList.Length; i++)
            {
                if (ItemList[i] == null)
                {
                    ItemList[i] = item;
                    InventorySlotUpdated.Invoke(i);
                    return true;
                }
            }

            return false;
        }

        public void RemoveItemFromInventory(int slot)
        {
            if (ItemList[slot] != null)
            {
                ItemList[slot] = null;
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