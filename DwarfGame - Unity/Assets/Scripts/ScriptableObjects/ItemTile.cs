using UnityEngine;

namespace DwarfGame
{
    public enum TileClass
    {
        None,
        Rock,
        Soil,
        Wood
    }
    
    /// <summary>
    /// An Item that can be placed in the world as tile.
    /// </summary>
    [CreateAssetMenu]
    public class ItemTile : Item
    {
        public TileClass Class = TileClass.None;
        public int WorldTileDamage = 1;

        public override ItemParams RightClickUse(ItemParams args)
        {
            Vector2 position = args.TargetType == TargetType.Tile ? args.AdjacentPosition : args.TargetPosition;
            
            if (TilemapManager.Instance.TerrainTilemap.WorldToCell(position) !=
                TilemapManager.Instance.TerrainTilemap.WorldToCell(args.OriginPosition))
            {
                TilemapManager.Instance.TerrainTilemap.PlaceTile(
                    TilemapManager.Instance.TerrainTilemap.WorldToCell(position),
                    this);
                --args.StackSize;
            }
            
            return args;
        }
    }
}