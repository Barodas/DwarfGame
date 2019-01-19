using UnityEngine;
using UnityEngine.UI;

namespace DwarfGame
{
    public class UiInventoryBar : MonoBehaviour
    {
        // TODO: Populate child objects based on number of slots in inventory, update UI to reflect changes in inventory size
        public Inventory PlayerInventory;
        public UiInventorySlot[] UiSlots;
        public Image UiSelectedSlotIndicator;
        
        private void Start()
        {
            PlayerInventory.InventorySlotUpdated.AddListener(UpdateUiSlot);
            PlayerInventory.InventorySelectedChanged.AddListener(UpdateSelectedSlot);
            RefreshUiSlots();
        }
        
        private void UpdateUiSlot(int slot)
        {
            if (PlayerInventory.ItemList[slot]?.Item != null)
            {
                UiSlots[slot].UpdateSlot(PlayerInventory.ItemList[slot].ItemSprite, PlayerInventory.ItemList[slot].StackSize);
            }
            else
            {
                UiSlots[slot].ClearSlot();
            }
        }

        private void RefreshUiSlots()
        {
            for (int i = 0; i < PlayerInventory.ItemList.Length; i++)
            {
                UpdateUiSlot(i);
            }
        }

        private void UpdateSelectedSlot(int slot)
        {
            UiSelectedSlotIndicator.transform.SetParent(UiSlots[slot].transform, false);
        }
    }
}