using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
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
        private float _moveInput;

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
            if(_isJumping)
            {
                
            }
            else
            {
                if (Input.GetKey(KeyCode.Space)) // TODO: Only allow jumping when grounded
                {
                    _targetJumpHeight = transform.position.y + (PlayerVars.JumpHeightBase * PlayerVars.JumpHeightModifier[PlayerVars.JumpHeightCurModifier]) + 0.2f;
                    _isJumping = true;
                }
            }
            
            _moveInput = Input.GetAxisRaw("Horizontal");
            
            if(Input.GetMouseButtonDown(0))
            {
                TilemapManager.Instance.TerrainTilemap.SetTile(TilemapManager.Instance.TerrainTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)), null);
    
                //TileBase tile = TilemapManager.Instance.TerrainTilemap.GetTile(TilemapManager.Instance.TerrainTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
                //if (tile != null)
                //{
                //    Debug.Log(tile.name);
                //}
            }
    
            if(_moveInput < 0)
            {
                BodySprite.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if(_moveInput > 0)
            {
                BodySprite.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        private void FixedUpdate()
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
                float distance = PlayerVars.FallSpeedBase;
                float rayDistance = distance + _col.bounds.extents.y;
                
                Vector2 center = _col.bounds.center;
                Vector2 left = new Vector2(center.x - _col.bounds.extents.x, center.y);
                Vector2 right = new Vector2(center.x + _col.bounds.extents.x, center.y);
                
                RaycastHit2D[] hits = new RaycastHit2D[3];
                hits[0] = Raycast(left, Vector2.down, rayDistance, CollisionMask, Color.blue);
                hits[1] = Raycast(center, Vector2.down, rayDistance, CollisionMask, Color.blue);
                hits[2] = Raycast(right, Vector2.down, rayDistance, CollisionMask, Color.blue);
                
                float hitDistance = rayDistance + 1f;
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null && hit.distance > 0) // 0 distance removes hits on edges of our collider
                    {
                        hitDistance = hit.distance < hitDistance ? hit.distance : hitDistance;
                    }
                }
                
                if (hitDistance < rayDistance)
                {
                    distance = hitDistance - _col.bounds.extents.y;
                }
            
                if (distance > 0)
                {
                    transform.Translate(Vector3.down * distance);   
                }
            }

            if (_moveInput != 0)
            {
                float distance = PlayerVars.MoveSpeedBase;
                if (_moveInput > 0)
                {
                    float rayDistance = distance + _col.bounds.extents.x;

                    Vector2 center = _col.bounds.center;
                    Vector2 upper = new Vector2(center.x, center.y + _col.bounds.extents.y);
                    Vector2 lower = new Vector2(center.x, center.y - _col.bounds.extents.y);

                    RaycastHit2D[] hits = new RaycastHit2D[3];
                    hits[0] = Raycast(upper, Vector2.right, rayDistance, CollisionMask, Color.cyan);
                    hits[1] = Raycast(center, Vector2.right, rayDistance, CollisionMask, Color.cyan);
                    hits[2] = Raycast(lower, Vector2.right, rayDistance, CollisionMask, Color.cyan);

                    float hitDistance = rayDistance + 1f;
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null && hit.distance > 0) // TODO: Figure out how to handle raycasts on adjacent floor/wall
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
                    hits[0] = Raycast(upper, Vector2.left, rayDistance, CollisionMask, Color.cyan);
                    hits[1] = Raycast(center, Vector2.left, rayDistance, CollisionMask, Color.cyan);
                    hits[2] = Raycast(lower, Vector2.left, rayDistance, CollisionMask, Color.cyan);

                    float hitDistance = rayDistance + 1f;
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null && hit.distance > 0) // 0 distance removes hits on edges of our collider
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

        // TODO: Put this into a util class, add a bool for drawing rays
        public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, LayerMask mask, Color colour)
        {
            Debug.DrawRay(origin, direction * distance, colour);
            return Physics2D.Raycast(origin, direction, distance, mask);
        }
    }

}