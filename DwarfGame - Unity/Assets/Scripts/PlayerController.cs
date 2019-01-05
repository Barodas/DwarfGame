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

        private Bounds _bounds;

        public Bounds Bounds
        {
            get
            {
                _bounds.Center = transform.position;
                return _bounds;
            }
            private set => _bounds = value;
        }
    
        private void Start()
        {
            _terrain = TilemapManager.Instance.TerrainTilemap;
            Bounds = new Bounds(transform.position);
        }
    
        private void Update()
        { 
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
                if (IsFalling())
                {
                    Vector2 direction = Vector2.down * PlayerVars.FallSpeedBase;
                    Bounds targetBounds = new Bounds(Bounds);
                    targetBounds.Center += direction;
                    if (_terrain.HasTile(_terrain.WorldToCell(targetBounds.BottomLeft)))
                    {
                        float tileCenter = Mathf.Round(targetBounds.BottomLeft.y);
                        direction.y = (tileCenter + 0.5f + targetBounds.Extents.y) - targetBounds.Center.y;
                    }
                    else if (_terrain.HasTile(_terrain.WorldToCell(targetBounds.BottomRight)))
                    {
                        float tileCenter = Mathf.Round(targetBounds.BottomRight.y);
                        direction.y = (tileCenter + 0.5f + targetBounds.Extents.y) - targetBounds.Center.y;
                    }

                    transform.Translate(direction);
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
                // TODO: figure out how to access the tiles position from within the tile.
                Bounds targetBounds = new Bounds(Bounds);
                Vector2 direction = Vector2.right * input * (PlayerVars.MoveSpeedBase * PlayerVars.MovespeedMultiplier[PlayerVars.MoveSpeedCurModifier]);
                targetBounds.Center += direction;

                if(_terrain.HasTile(_terrain.WorldToCell(targetBounds.TopRight)) || _terrain.HasTile(_terrain.WorldToCell(targetBounds.BottomRight)))
                {
                    float tileCenter = (float)System.Math.Round(targetBounds.Right, System.MidpointRounding.AwayFromZero);
                    direction.x = (tileCenter - 0.5f - targetBounds.Extents.x) - targetBounds.Center.x;
                }
                else if(_terrain.HasTile(_terrain.WorldToCell(targetBounds.TopLeft)) || _terrain.HasTile(_terrain.WorldToCell(targetBounds.BottomLeft)))
                {
                    float tileCenter = (float)System.Math.Round(targetBounds.Left, System.MidpointRounding.AwayFromZero);
                    direction.x = (tileCenter + 0.5f + targetBounds.Extents.x) - targetBounds.Center.x;
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
            return _terrain.HasTile(_terrain.WorldToCell(Bounds.TopLeft)) || _terrain.HasTile(_terrain.WorldToCell(Bounds.TopRight));
        }
    
        private bool IsFalling()
        {
            Vector2 fallingThreshold = Vector2.down * 0.01f;
            return !_terrain.HasTile(_terrain.WorldToCell(Bounds.BottomLeft + fallingThreshold)) && !_terrain.HasTile(_terrain.WorldToCell(Bounds.BottomRight + fallingThreshold));
        }
    }

}