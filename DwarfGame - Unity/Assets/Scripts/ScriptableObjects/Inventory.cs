using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DwarfGame
{
    [CreateAssetMenu]
    public class Inventory : ScriptableObject
    {
        public int InventorySize = 5;
        public List<Item> ItemList;

        public bool AddItemToInventory(Item item)
        {
            if (ItemList.Count < InventorySize)
            {
                ItemList.Add(item);
                return true;
            }

            return false;
        }

        public void ClearInventory()
        {
            ItemList.Clear();
        }
    }
}