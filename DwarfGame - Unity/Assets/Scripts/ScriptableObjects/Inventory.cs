using UnityEngine;
using UnityEngine.Events;

namespace DwarfGame
{
    [CreateAssetMenu]
    public class Inventory : ScriptableObject
    {
        public InventorySlotUpdateEvent InventorySlotUpdated;
        public Item[] ItemList;
        
        private void Awake()
        {
            InventorySlotUpdated = new InventorySlotUpdateEvent();
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
    public class InventorySlotUpdateEvent : UnityEvent<int>
    {
        
    }
}