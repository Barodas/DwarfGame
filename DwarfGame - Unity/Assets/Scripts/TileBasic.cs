using DwarfGame;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TileBasic : TileBase
{
    private int _damage = 100;
    public Item Item;

    // TODO: How should we handle destroying tiles? Should the tilemap be overridden to handle the creation of item version of tile before destroying?
    public void DestroyTile()
    {
        
    }
    
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = Item.ItemSprite;
        tileData.color = Color.white;
        tileData.colliderType = Tile.ColliderType.Grid;
    }
    
#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
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
