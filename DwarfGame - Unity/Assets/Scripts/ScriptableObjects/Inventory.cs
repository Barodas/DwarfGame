using UnityEngine;
using UnityEngine.Events;

namespace DwarfGame
{
    [CreateAssetMenu]
    public class Inventory : ScriptableObject, ISerializationCallbackReceiver
    {
        public IntEvent InventorySlotUpdated;
        public IntEvent InventorySelectedChanged;

        public SerializedInstanceItem[] SerializedItemList;
        
        public InstanceItem[] ItemList;

        public int SelectedSlot { get; private set; }
        public InstanceItem SelectedSlotItem => ItemList[SelectedSlot];

        private void Awake()
        {
            InventorySlotUpdated = new IntEvent();
            InventorySelectedChanged = new IntEvent();
        }
        
        public void UseSelectedItem(ItemParams args)
        {
            if (ItemList[SelectedSlot] != null)
            {
                if (ItemList[SelectedSlot].UseItem(args))
                {
                    ItemList[SelectedSlot] = null;
                }
            }
            else
            {
                if (args.ClickType == ClickType.Left)
                {
                    Item.LeftClickUseEmpty(args);
                }
            }
            InventorySlotUpdated.Invoke(SelectedSlot);
        }
        
        /// <summary>
        /// Adds the InventoryItem to the Inventory
        /// </summary>
        /// <param name="instanceItem"></param>
        /// <returns>Returns false if the inventory could not fit the entire inventoryItem</returns>
        public bool AddItemToInventory(InstanceItem instanceItem)
        {
            // Add items to existing slots of same item
            for (int i = 0; i < ItemList.Length; i++)
            {
                if (ItemList[i]?.Item != null && ItemList[i].Item == instanceItem.Item)
                {
                    if (ItemList[i].Combine(instanceItem))
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
                if (ItemList[i] == null || ItemList[i].Item == null)
                {
                    ItemList[i] = instanceItem;
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
            SerializedItemList = new SerializedInstanceItem[5];
            ItemList = new InstanceItem[SerializedItemList.Length];
            
            for (int i = 0; i < SerializedItemList.Length; i++)
            {
                InventorySlotUpdated.Invoke(i);
            }
        }

        public void OnBeforeSerialize()
        {
            SerializedItemList = new SerializedInstanceItem[5];
            
            for (int i = 0; i < ItemList.Length; i++)
            {
                if (ItemList[i] != null)
                {
                    SerializedItemList[i] = new SerializedInstanceItem(ItemList[i]);
                }
            }
        }

        public void OnAfterDeserialize()
        {
            ItemList = new InstanceItem[SerializedItemList.Length];

            for (int i = 0; i < SerializedItemList.Length; i++)
            {
                if (SerializedItemList[i].Item != null)
                {
                    ItemList[i] = new InstanceItem(SerializedItemList[i]);
                }
            }
        }
    }

    [System.Serializable]
    public class IntEvent : UnityEvent<int>
    {
        
    }
}