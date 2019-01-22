using System.Collections;
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
    
    public class TargetParams
    {
        public TargetType TargetType;
        public TileClass TileClass;
        public ClickType ClickType;
        public Vector2 TargetPosition;
        public Vector2 AdjacentPosition;
        public Vector2 OriginPosition;
        public HitDirection HitDirection;
        public int Damage;
        public Dictionary<string, int> IntStore;
        public int StackSize;
    }

    public class ResolutionParams
    {
        public TargetType TargetType;
        public ResolutionType ResolutionType;
        public Vector2 Position;
        public HitDirection HitDirection;
        public int Damage;
        public Dictionary<string, int> IntStore;
        public int StackSize;
    }
}