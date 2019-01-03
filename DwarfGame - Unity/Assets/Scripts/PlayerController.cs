using UnityEngine;
using UnityEngine.Tilemaps;

namespace DwarfGame
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerVariables PlayerVars;
        public GameObject BodySprite;
        
        private Tilemap _terrain;
    
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
                Bounds targetBounds = Bounds;
                Vector2 direction = Vector2.right * input * (PlayerVars.MoveSpeedBase * PlayerVars.MovespeedMultiplier[PlayerVars.MoveSpeedCurModifier]);
                targetBounds.Center += direction;

                TileBase upper, lower;
                upper = _terrain.GetTile(_terrain.WorldToCell(direction.x < 0 ? targetBounds.TopLeft : targetBounds.TopRight));
                lower = _terrain.GetTile(_terrain.WorldToCell(direction.x < 0 ? targetBounds.BottomLeft : targetBounds.BottomRight));

                if (upper != null)
                {
                    // TODO: Reduce distance to prevent collision
                }
                else if (lower != null)
                {
                    
                }
                
                if(_terrain.HasTile(_terrain.WorldToCell(targetBounds.)) || _terrain.HasTile(_terrain.WorldToCell(targetFeet)))
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
            return _terrain.HasTile(_terrain.WorldToCell(Bounds.TopLeft)) || _terrain.HasTile(_terrain.WorldToCell(Bounds.TopRight));
        }
    
        private bool IsFalling()
        {
            return !_terrain.HasTile(_terrain.WorldToCell(Bounds.BottomLeft)) && !_terrain.HasTile(_terrain.WorldToCell(Bounds.BottomRight));
        }
    }

}