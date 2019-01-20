using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DwarfGame
{
    public enum TileLayer
    {
        Terrain,
        TerrainBackground
    }
    
    public class TilemapManager : MonoBehaviour
    {
        public static TilemapManager Instance { get; private set; }

        public Tilemap TerrainTilemap;
        public Tilemap TerrainBackgroundTilemap;

        private Dictionary<Vector3Int, WorldTile> _terrainWorldTiles = new Dictionary<Vector3Int, WorldTile>();
        
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        private Tilemap GetTilemap(TileLayer layer)
        {
            switch (layer)
            {
                default:
                    return TerrainTilemap;
                case TileLayer.TerrainBackground:
                    return TerrainBackgroundTilemap;
            }
            
        }
        
        public void DamageTile(TileLayer layer, Vector3Int position, int amount)
        {
            Tilemap tilemap = GetTilemap(layer);
            TileBasic tile = tilemap.GetTile<TileBasic>(position);
            if (tile != null)
            {
                if (!_terrainWorldTiles.ContainsKey(position))
                {
                    // Check if the tile is destroyed instantly
                    if (amount > tile.Item.WorldTileDamage)
                    {
                        WorldItem worldItem = WorldItem.CreateWorldItem(new InventoryItem(tile.Item), tilemap.CellToWorld(position));
                        tilemap.SetTile(position, null);
                    }
                    // else make a worldtile and apply damage
                    else
                    {
                        _terrainWorldTiles.Add(position, new WorldTile{Damage = amount});
                    }
                }
                else
                {
                    // Get world tile from dictionary and apply damage
                    _terrainWorldTiles[position].Damage += amount;
                    // check for tile destruction
                    if (_terrainWorldTiles[position].Damage >= tile.Item.WorldTileDamage)
                    {
                        _terrainWorldTiles.Remove(position);
                        WorldItem worldItem = WorldItem.CreateWorldItem(new InventoryItem(tile.Item), tilemap.CellToWorld(position));
                        tilemap.SetTile(position, null);
                    }
                }
            }
        }
    }
}