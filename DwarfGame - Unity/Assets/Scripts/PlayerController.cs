using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;

    public PlayerVariables PlayerVars;
    public GameObject BodySprite;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");
        if (input != 0)
        {
            _rb.AddForce(Vector3.right * input * (PlayerVars.MoveSpeedBase * PlayerVars.MovespeedMultiplier[PlayerVars.MoveSpeedCurModifier]));
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * (PlayerVars.JumpStrengthBase * PlayerVars.JumpStrengthMultiplier[PlayerVars.JumpStrengthCurModifier]), ForceMode2D.Impulse);
        }

        if(Input.GetMouseButtonDown(0))
        {
            TilemapManager.Instance.TerrainTilemap.SetTile(TilemapManager.Instance.TerrainTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)), null);

            //TileBase tile = TilemapManager.Instance.TerrainTilemap.GetTile(TilemapManager.Instance.TerrainTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            //if (tile != null)
            //{
            //    Debug.Log(tile.name);
            //}
        }

        if(input < 0)
        {
            BodySprite.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(input > 0)
        {
            BodySprite.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
