using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfGame
{
    [CreateAssetMenu]
    public class ItemTile : Item
    {
        public int WorldTileDamage = 1;

        public override void Use(Vector2 position)
        {
            TilemapManager.Instance.TerrainTilemap.PlaceTile(
                TilemapManager.Instance.TerrainTilemap.WorldToCell(position),
                this);
        }
    }
}