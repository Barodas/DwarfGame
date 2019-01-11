using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBasic : Tile
{



#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/BasicTile")]
    public static void CreateBasicTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Basic Tile", "New Basic Tile", "Asset", "Save Basic Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TileBasic>(), path);
    }
#endif
}
