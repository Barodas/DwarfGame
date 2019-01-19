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

        public void UseSelectedItem()
        {
            if (ItemList[SelectedSlot].UseItem())
            {
                ItemList[SelectedSlot] = null;
                InventorySlotUpdated.Invoke(SelectedSlot);
            }
        }
        
        /// <summary>
        /// Adds the InventoryItem to the Inventory
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <returns>Returns false if the inventory could not fit the entire inventoryItem</returns>
        public bool AddItemToInventory(InventoryItem inventoryItem)
        {
            // Add items to existing slots of same item
            for (int i = 0; i < ItemList.Length; i++)
            {
                if (ItemList[i]?.Item != null && ItemList[i].Item == inventoryItem.Item)
                {
                    if (ItemList[i].Combine(inventoryItem))
                    {
                        InventorySlotUpdated.Invoke(i);
                        return true;
                    }
                    
                    InventorySlotUpdated.Invoke(i);
                }
            }
            
            // Add to next available slot
            for (int i = 0; i < ItemList.Length; i++)
            {
                if (ItemList[i].Item == null)
                {
                    ItemList[i] = inventoryItem;
                    InventorySlotUpdated.Invoke(i);
                    return true;
                }
            }

            return false;
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