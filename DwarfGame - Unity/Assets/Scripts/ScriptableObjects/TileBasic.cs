using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DwarfGame
{
    public class TileBasic : TileBase
    {
        public ItemTile Item;

        public TileBasic Initialise(ItemTile item)
        {
            Item = item;
            return this;
        }
    
        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = Item.ItemSprite;
            tileData.color = Color.white;
            tileData.colliderType = Tile.ColliderType.Grid;
        }
    
#if UNITY_EDITOR
        [MenuItem("Assets/Create/CustomTiles/BasicTile")]
        public static void CreateBasicTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Basic Tile", "New Basic Tile", "Asset", "Save Basic Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TileBasic>(), path);
        }
#endif
    }
}