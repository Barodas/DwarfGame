using UnityEngine;
using UnityEngine.Tilemaps;

namespace DwarfGame
{
    public static class TilemapUtils
    {
        public static void DestroyTile(this Tilemap tilemap, Vector3Int position)
        {
            TileBasic tile = tilemap.GetTile<TileBasic>(position);
            if (tile != null)
            {
                WorldItem worldItem = WorldItem.CreateWorldItem(new InstanceItem(tile.Item), tilemap.CellToWorld(position));
                tilemap.SetTile(position, null);
            }
        }

        public static bool PlaceTile(this Tilemap tilemap, Vector3Int position, ItemTile item)
        {
            if (!tilemap.HasTile(position) && item != null)
            {
                TileBasic tile = ScriptableObject.CreateInstance<TileBasic>().Initialise(item);
                tilemap.SetTile(position, tile);
                return true;
            }

            return false;
        }
    }
}