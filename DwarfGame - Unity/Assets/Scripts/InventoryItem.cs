using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

namespace DwarfGame
{
    /// <summary>
    /// Instanced version of an item in the inventory.
    /// </summary>
    [Serializable]
    public class InventoryItem
    {
        public Item Item;
        public int StackSize;

        private Dictionary<string, int> _intStore = new Dictionary<string, int>();
        
        public Sprite ItemSprite => Item.ItemSprite;

        public InventoryItem(Item item, int stackSize = 1)
        {
            Item = item;
            StackSize = stackSize;
            
            Initialise();
        }

        public void Initialise()
        {
            TargetParams args = new TargetParams{IntStore = _intStore};
            ResolutionParams resolution = Item.Initialise(args);
            _intStore = resolution.IntStore;
        }
        
        public bool UseItem(TargetParams args)
        {
            if (Item == null)
            {
                LeftClickUseEmpty(args);
                return false;
            }
            
            args.IntStore = _intStore;
            args.StackSize = StackSize;
            
            ResolutionParams resolution;
            switch (args.ClickType)
            {
                default:
                    resolution = Item.LeftClickUse(args);
                    break;
                case ClickType.Right:
                    resolution = Item.RightClickUse(args);
                    break;
            }
            
            _intStore = resolution.IntStore;
            StackSize = resolution.StackSize;
            
            return StackSize <= 0;
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

        public int GetStoreValue(string key)
        {
            return _intStore != null && _intStore.ContainsKey(key) ? _intStore[key] : 0;
        }
        
        private void LeftClickUseEmpty(TargetParams args)
        {
            TilemapManager.Instance.DamageTile(TileLayer.Terrain,
                TilemapManager.Instance.TerrainTilemap.WorldToCell(
                    args.TargetPosition),
                args.Damage, 
                args.HitDirection);
        }
    }
}