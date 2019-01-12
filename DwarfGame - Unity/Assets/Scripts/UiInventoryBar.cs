using UnityEngine;

namespace DwarfGame
{
    public class UiInventoryBar : MonoBehaviour
    {
        // TODO: Populate child objects based on number of slots in inventory, update UI to reflect changes in inventory size
        public Inventory PlayerInventory;
        public UiInventorySlot[] UiSlots;

        private void Start()
        {
            PlayerInventory.InventorySlotUpdated.AddListener(UpdateUiSlot);
        }
        
        private void UpdateUiSlot(int slot)
        {
            // TODO: Consider handling empty inventory slots better than using nulls
            UiSlots[slot].UpdateSprite(PlayerInventory.ItemList[slot] != null ? PlayerInventory.ItemList[slot].ItemSprite : null);
        }
    }
}