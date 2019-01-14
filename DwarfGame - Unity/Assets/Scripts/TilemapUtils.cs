using UnityEngine;
using UnityEngine.Tilemaps;

namespace DwarfGame
{
    public static class TilemapUtils
    {
        public static void DestroyTile(this Tilemap tileMap, Vector3Int position)
        {
            TileBasic tile = tileMap.GetTile<TileBasic>(position);
            if (tile != null)
            {
                WorldItem worldItem = WorldItem.CreateWorldItem(tile.Item, tileMap.CellToWorld(position));
                tileMap.SetTile(position, null);
            }
        }

        public static bool PlaceTile(this Tilemap tilemap, Vector3Int position, Item item)
        {
            if (!tilemap.HasTile(position) && item != null)
            {
                TileBasic tile = new TileBasic(item);
                tilemap.SetTile(position, tile);
                return true;
            }

            return false;
        }
    }
}