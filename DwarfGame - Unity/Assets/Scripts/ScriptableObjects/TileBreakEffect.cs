using UnityEngine;
using UnityEngine.Tilemaps;

namespace DwarfGame
{
    public class TileBreakEffect : TileBase
    {
        private Sprite _sprite;
        private Quaternion _direction;
        
        public TileBreakEffect Initialise(Sprite sprite, Quaternion direction)
        {
            _sprite = sprite;
            _direction = direction;
            return this;
        }
    
        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = _sprite;
            Matrix4x4 matrix = tileData.transform;
            matrix.SetTRS(Vector3.zero, _direction, Vector3.one);
            tileData.transform = matrix;
            tileData.color = Color.white;
            tileData.colliderType = Tile.ColliderType.None;
        }
    }
}