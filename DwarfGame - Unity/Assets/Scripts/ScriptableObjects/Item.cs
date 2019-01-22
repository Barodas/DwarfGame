using System;
using UnityEngine;

namespace DwarfGame
{
    /// <summary>
    /// Blueprint of an item. The SO contains the default stats and functionality that are used by the InventoryItem.
    /// </summary>
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public Sprite ItemSprite;
        public int StackLimit = 1;
        public bool ModifiesLeftUse = false;
        public bool ModifiesRightUse = false;
        
        /// <summary>
        /// Generally treated as placing the item in some way
        /// </summary>
        /// <param name="args">Contains various input arguments based on target object</param>
        public virtual ResolutionParams RightClickUse(TargetParams args)
        {
            return new ResolutionParams{ResolutionType = ResolutionType.None, StackSize = args.StackSize, IntStore = args.IntStore};
        }
        
        /// <summary>
        /// Generally treated as swinging the item at the target position
        /// </summary>
        /// <param name="args">Contains various input arguments based on target object</param>
        public virtual ResolutionParams LeftClickUse(TargetParams args)
        {
            TilemapManager.Instance.DamageTile(TileLayer.Terrain,
                TilemapManager.Instance.TerrainTilemap.WorldToCell(
                    args.TargetPosition),
                args.Damage, 
                args.HitDirection);
            
            return new ResolutionParams{TargetType = args.TargetType, Position = args.TargetPosition, Damage = args.Damage, StackSize = args.StackSize, IntStore = args.IntStore};
        }
    }
}