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
    
    public enum HitDirection
    {
        Top,
        Bottom,
        Left,
        Right
    }
    
    public class TilemapManager : MonoBehaviour
    {
        public static TilemapManager Instance { get; private set; }

        public Tilemap TerrainTilemap;
        public Tilemap TerrainBackgroundTilemap;
        public Tilemap EffectTilemap;

        public Sprite[] BreakEffectSprites;

        private Dictionary<HitDirection, Quaternion> _breakEffectSpriteRotation =
            new Dictionary<HitDirection, Quaternion>
            {
                {HitDirection.Top, Quaternion.Euler(0f, 0f, -90f)},
                {HitDirection.Bottom, Quaternion.Euler(0f, 0f, -270f)},
                {HitDirection.Left, Quaternion.Euler(0f, 0f, 0f)},
                {HitDirection.Right, Quaternion.Euler(0f, 0f, -180f)}
            };
        
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
        
        public void DamageTile(TileLayer layer, Vector3Int position, int amount, HitDirection hitDirection)
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
                        WorldItem worldItem = WorldItem.CreateWorldItem(new InstanceItem(tile.Item), tilemap.CellToWorld(position));
                        tilemap.SetTile(position, null);
                    }
                    // else make a worldtile and apply damage
                    else
                    {
                        _terrainWorldTiles.Add(position, new WorldTile{Damage = amount});
                        
                        // Create a break effect tile to represent damage
                        EffectTilemap.SetTile(position, CreateBreakEffectTile(hitDirection, amount, tile.Item.WorldTileDamage));
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
                        WorldItem worldItem = WorldItem.CreateWorldItem(new InstanceItem(tile.Item), tilemap.CellToWorld(position));
                        tilemap.SetTile(position, null);
                        EffectTilemap.SetTile(position, null);
                    }
                    else
                    {
                        // Update the break effect tile
                        EffectTilemap.SetTile(position, CreateBreakEffectTile(hitDirection, _terrainWorldTiles[position].Damage, tile.Item.WorldTileDamage));
                    }
                }
            }
        }

        private TileBreakEffect CreateBreakEffectTile(HitDirection hitDirection, float curDamage, float maxDamage)
        {
            float percentDamage = curDamage / maxDamage;
            int spriteValue = (int)Mathf.Lerp(0, BreakEffectSprites.Length, percentDamage);
            
            return ScriptableObject.CreateInstance<TileBreakEffect>().Initialise(BreakEffectSprites[spriteValue], _breakEffectSpriteRotation[hitDirection]);
        }
    }
}