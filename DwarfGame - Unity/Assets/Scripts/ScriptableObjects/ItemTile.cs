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
    
    [CreateAssetMenu]
    public class ItemTile : Item
    {
        public TileClass Class = TileClass.None;
        public int WorldTileDamage = 1;

        public override void Use(Vector2 position)
        {
            TilemapManager.Instance.TerrainTilemap.PlaceTile(
                TilemapManager.Instance.TerrainTilemap.WorldToCell(position),
                this);
        }
    }
}