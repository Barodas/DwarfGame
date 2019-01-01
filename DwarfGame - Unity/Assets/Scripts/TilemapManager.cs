using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public static TilemapManager Instance { get; private set; }

    public Tilemap TerrainTilemap;
    public Tilemap TerrainBackgroundTilemap;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        
    }
}
