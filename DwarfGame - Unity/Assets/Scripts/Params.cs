using System.Collections.Generic;
using UnityEngine;

namespace DwarfGame
{
    public enum TargetType
    {
        None,
        Tile,
        BackgroundTile,
        Entity
    }

    public enum ResolutionType
    {
        None,
        PlaceTile
    }

    public enum ClickType
    {
        Left,
        Right
    }
    
    public class ItemParams
    {
        // Target Params
        public TargetType TargetType;
        public ClickType ClickType;
        public Vector2 TargetPosition;
        public Vector2 AdjacentPosition;
        public Vector2 OriginPosition;
        public HitDirection HitDirection;
        public TileClass TileClass;
        
        // Item Params
        public int Damage;
        public int CurrentDurability;
        public int StackSize;
        public Dictionary<string, int> IntStore;
        
        // Resolution Params
        public ResolutionType ResolutionType;

        public ItemParams()
        {
        }

        public ItemParams(InstanceItem item)
        {
            CurrentDurability = item.CurrentDurability;
            StackSize = item.StackSize;
            IntStore = item.IntStore;
        }
    }
}