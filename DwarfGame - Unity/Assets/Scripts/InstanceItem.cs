using System.Collections.Generic;
using UnityEngine;

namespace DwarfGame
{
    /// <summary>
    /// Instanced version of an item in the inventory.
    /// </summary>
    public class InstanceItem
    {
        public Item Item;
        public int StackSize;
        public int CurrentDurability;

        public Dictionary<string, int> IntStore = new Dictionary<string, int>();
        
        public Sprite ItemSprite => Item.ItemSprite;
        
        public InstanceItem(Item item, int stackSize = 1, int curDurability = 0)
        {
            Item = item;
            StackSize = stackSize;
            CurrentDurability = curDurability;
            
            Initialise();
        }

        public InstanceItem(SerializedInstanceItem serializedInstanceItem)
        {
            Item = serializedInstanceItem.Item;
            StackSize = serializedInstanceItem.StackSize;
            CurrentDurability = serializedInstanceItem.CurrentDurability;
            IntStore = serializedInstanceItem.IntStore;
        }
        
        public void Initialise()
        {
            TargetParams args = new TargetParams{IntStore = IntStore, CurrentDurability = CurrentDurability};
            ResolutionParams resolution = Item.Initialise(args);
            CurrentDurability = resolution.CurrentDurability;
            IntStore = resolution.IntStore;
        }
        
        public bool UseItem(TargetParams args)
        {
            if (Item == null)
            {
                LeftClickUseEmpty(args);
                return false;
            }
            
            args.IntStore = IntStore;
            args.StackSize = StackSize;
            args.CurrentDurability = CurrentDurability;
            
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
            
            IntStore = resolution.IntStore;
            StackSize = resolution.StackSize;
            CurrentDurability = args.CurrentDurability;
            
            return StackSize <= 0;
        }
        
        /// <summary>
        /// Combines 2 InventoryItem stacks. Returns true if inventoryItem.StackSize is 0.
        /// </summary>
        /// <param name="instanceItem"></param>
        /// <returns></returns>
        public bool Combine(InstanceItem instanceItem)
        {
            instanceItem.StackSize = Add(instanceItem.StackSize);
            if (instanceItem.StackSize <= 0)
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
            return IntStore != null && IntStore.ContainsKey(key) ? IntStore[key] : 0;
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