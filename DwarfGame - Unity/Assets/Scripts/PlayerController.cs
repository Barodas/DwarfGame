using UnityEngine;
using UnityEngine.Tilemaps;

namespace DwarfGame
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerVariables PlayerVars;
        public GameObject BodySprite;
        
        private Tilemap _terrain;
    
        private bool _isFalling;
        private bool _isJumping;
    
        private float _targetJumpHeight;
    
        private void Start()
        {
            _terrain = TilemapManager.Instance.TerrainTilemap;
        }
    
        // TODO: Try tracking the 4 corners of the character model and using those for checks in each direction rather than current single checks
    
        private void Update()
        {
            CheckGround();
    
            if(_isJumping)
            {
                if(transform.position.y >= _targetJumpHeight || HasHitCeiling())
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
                    
                    
                    if (Input.GetKey(KeyCode.Space))
                    {
                        _targetJumpHeight = transform.position.y + (PlayerVars.JumpHeightBase * PlayerVars.JumpHeightModifier[PlayerVars.JumpHeightCurModifier]) + 0.2f;
                        _isJumping = true;
                    }
                }
            }
            
            float input = Input.GetAxisRaw("Horizontal");
            if (input != 0)
            {
                Vector3 direction = Vector3.right * input * (PlayerVars.MoveSpeedBase * PlayerVars.MovespeedMultiplier[PlayerVars.MoveSpeedCurModifier]);
                Vector3 target = transform.position + direction;
                float playerWidthOffset = direction.x > 0 ? PlayerVars.PlayerWidth + 0.01f : -PlayerVars.PlayerWidth - 0.01f;
                target.x += playerWidthOffset;
    
                Vector3 targetHead;
                Vector3 targetFeet = targetHead = target;
                targetHead.y += PlayerVars.PlayerHeightFromCenter;
                targetFeet.y -= 0.49f;
    
                if(_terrain.HasTile(_terrain.WorldToCell(targetHead)) || _terrain.HasTile(_terrain.WorldToCell(targetFeet)))
                {
                    target.x = Mathf.RoundToInt(target.x);
                    target.x -= playerWidthOffset;
    
                    direction = target - transform.position;
                }
    
                transform.Translate(direction);
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
        
        private bool HasHitCeiling()
        {
            Vector3 right = transform.position + new Vector3(PlayerVars.PlayerWidth, PlayerVars.PlayerHeightFromCenter, 0);
            Vector3 left = transform.position + new Vector3(-PlayerVars.PlayerWidth, PlayerVars.PlayerHeightFromCenter, 0);
    
            return _terrain.HasTile(_terrain.WorldToCell(right)) || _terrain.HasTile(_terrain.WorldToCell(left));
        }
    
        private void CheckGround()
        {
            Vector3 right = transform.position + new Vector3(PlayerVars.PlayerWidth, -0.55f, 0);
            Vector3 left = transform.position + new Vector3(-PlayerVars.PlayerWidth, -0.55f, 0);
            
            _isFalling = !_terrain.HasTile(_terrain.WorldToCell(right)) && !_terrain.HasTile(_terrain.WorldToCell(left));
        }
    }

}