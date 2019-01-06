using UnityEngine;
using UnityEngine.Tilemaps;

namespace DwarfGame
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerVariables PlayerVars;
        public GameObject BodySprite;
        public LayerMask CollisionMask;

        private Tilemap _terrain;
        private Rigidbody2D _rb;
        private BoxCollider2D _col;

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
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<BoxCollider2D>();
        }
    
        private void Update()
        {
            //if(_isJumping)
            //{
            //    if(transform.position.y >= _targetJumpHeight || HasHitCeiling())
            //    {
            //        _isJumping = false;
            //    }
            //    else
            //    {
            //        _rb.velocity = new Vector2(_rb.velocity.x, PlayerVars.JumpSpeedBase);
            //    }
            //}
            //else
            //{
            //    _rb.velocity = new Vector2(_rb.velocity.x, -PlayerVars.FallSpeedBase);
            //
            //    if (Input.GetKey(KeyCode.Space))
            //    {
            //        _targetJumpHeight = transform.position.y + (PlayerVars.JumpHeightBase * PlayerVars.JumpHeightModifier[PlayerVars.JumpHeightCurModifier]) + 0.2f;
            //        _isJumping = true;
            //    }
            //}
            
            float input = Input.GetAxisRaw("Horizontal");
            if (input != 0)
            {
                // TODO: Try moving these into FixedUpdate()
                float distance = PlayerVars.MoveSpeedBase;
                if (input > 0)
                {
                    float rayDistance = distance + _col.bounds.extents.x;

                    Vector2 center = _col.bounds.center;
                    Vector2 upper = new Vector2(center.x, center.y + _col.bounds.extents.y);
                    Vector2 lower = new Vector2(center.x, center.y - _col.bounds.extents.y);

                    RaycastHit2D[] hits = new RaycastHit2D[3];
                    hits[0] = Physics2D.Raycast(upper, Vector2.right, rayDistance, CollisionMask);
                    hits[1] = Physics2D.Raycast(center, Vector2.right, rayDistance, CollisionMask);
                    hits[2] = Physics2D.Raycast(lower, Vector2.right, rayDistance, CollisionMask);

                    float hitDistance = rayDistance + 1f;
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null)
                        {
                            hitDistance = hit.distance < hitDistance ? hit.distance : hitDistance;
                        }
                    }

                    if (hitDistance < rayDistance)
                    {
                        distance = hitDistance - _col.bounds.extents.x;
                    }
                }
                else
                {
                    float rayDistance = distance + _col.bounds.extents.x;

                    Vector2 center = _col.bounds.center;
                    Vector2 upper = new Vector2(center.x, center.y + _col.bounds.extents.y);
                    Vector2 lower = new Vector2(center.x, center.y - _col.bounds.extents.y);

                    RaycastHit2D[] hits = new RaycastHit2D[3];
                    hits[0] = Physics2D.Raycast(upper, Vector2.left, rayDistance, CollisionMask);
                    hits[1] = Physics2D.Raycast(center, Vector2.left, rayDistance, CollisionMask);
                    hits[2] = Physics2D.Raycast(lower, Vector2.left, rayDistance, CollisionMask);

                    float hitDistance = rayDistance + 1f;
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null)
                        {
                            hitDistance = hit.distance < hitDistance ? hit.distance : hitDistance;
                        }
                    }

                    if (hitDistance < rayDistance)
                    {
                        distance = hitDistance - _col.bounds.extents.x;
                    }
                    distance = -distance;
                }

                transform.Translate(Vector3.right * distance);
            }
            else
            {
                //_rb.velocity = new Vector2(0, _rb.velocity.y);
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