using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public PlayerVariables PlayerVars;
    public GameObject BodySprite;

    private bool _isFalling;
    private bool _isJumping;

    private float _targetJumpHeight;

    private void Start()
    {
    }

    private void Update()
    {
        CheckGround();

        if(_isJumping)
        {
            // TODO: Check for collision with roof
            if(transform.position.y >= _targetJumpHeight)
            {
                _isJumping = false;
            }
            else
            {
                transform.Translate(Vector3.up * PlayerVars.JumpSpeedBase);
            }
        }
        else
        {
            if (_isFalling)
            {
                transform.Translate(Vector3.down * PlayerVars.FallSpeedBase);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _targetJumpHeight = transform.position.y + (PlayerVars.JumpHeightBase * PlayerVars.JumpHeightModifier[PlayerVars.JumpHeightCurModifier]) + 0.2f;
                    _isJumping = true;
                }
            }
        }
        

        float input = Input.GetAxisRaw("Horizontal");
        if (input != 0)
        {
            // TODO: Wall Detection
            transform.Translate(Vector3.right * input * (PlayerVars.MoveSpeedBase * PlayerVars.MovespeedMultiplier[PlayerVars.MoveSpeedCurModifier]));
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



    private void CheckGround()
    {
        Vector3 right = transform.position + new Vector3(PlayerVars.PlayerWidth, -0.55f, 0);
        Vector3 left = transform.position + new Vector3(-PlayerVars.PlayerWidth, -0.55f, 0);

        Tilemap terrain = TilemapManager.Instance.TerrainTilemap;
        _isFalling = !terrain.HasTile(terrain.WorldToCell(right)) && !terrain.HasTile(terrain.WorldToCell(left));
    }
}
