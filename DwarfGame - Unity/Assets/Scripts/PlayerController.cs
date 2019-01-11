using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Tilemaps;

namespace DwarfGame
{
    public enum VerticalState
    {
        Grounded,
        Jumping,
        Falling
    };
    
    public class PlayerController : MonoBehaviour
    {
        private const float EdgeRayOffset = 0.001f;
        
        public PlayerVariables PlayerVars;
        public GameObject BodySprite;
        public LayerMask CollisionMask;

        private Tilemap _terrain;
        private Rigidbody2D _rb;
        private BoxCollider2D _col;

        private VerticalState _verticalState = VerticalState.Grounded;

        private VerticalState State
        {
            get { return _verticalState; }
            set
            {
                _verticalState = value;
                Debug.LogFormat("State changed to {0}", value);
            }
        }
    
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
            switch (State)
            {
                case VerticalState.Grounded:
                    if (Input.GetKey(KeyCode.Space))
                    {
                        _targetJumpHeight = _col.bounds.center.y + _col.bounds.extents.y + (PlayerVars.JumpHeightBase * PlayerVars.JumpHeightModifier[PlayerVars.JumpHeightCurModifier]) + 0.2f;
                        State = VerticalState.Jumping;
                    }
                    break;
                case VerticalState.Jumping:
                    break;
                case VerticalState.Falling:
                    break;
            }
    
            _moveInput = Input.GetAxisRaw("Horizontal");
            
            if(_moveInput < 0)
            {
                BodySprite.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if(_moveInput > 0)
            {
                BodySprite.transform.localScale = new Vector3(1, 1, 1);
            }
            
            
            // Block removal test code
            if(Input.GetMouseButtonDown(0))
            {
                TilemapManager.Instance.TerrainTilemap.SetTile(TilemapManager.Instance.TerrainTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)), null);
    
                //TileBase tile = TilemapManager.Instance.TerrainTilemap.GetTile(TilemapManager.Instance.TerrainTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
                //if (tile != null)
                //{
                //    Debug.Log(tile.name);
                //}
            }
        }

        private void FixedUpdate()
        {
            switch (State)
            {
                case VerticalState.Grounded:
                {
                    // Check if we should be falling 
                    float groundDistance = MultiRaycast(Vector2.down, PlayerVars.FallSpeedBase + _col.bounds.extents.y,
                        new Vector2[]
                        {
                            new Vector2(_col.bounds.center.x - _col.bounds.extents.x + EdgeRayOffset, _col.bounds.center.y),
                            _col.bounds.center,
                            new Vector2(_col.bounds.center.x + _col.bounds.extents.x - EdgeRayOffset, _col.bounds.center.y)
                        });
                    if (groundDistance - _col.bounds.extents.y > 0)
                    {
                        State = VerticalState.Falling;
                    }
                    break;
                }
                case VerticalState.Jumping:
                    // Clamp distance to targetHeight
                    float targetDistance = PlayerVars.JumpSpeedBase + _col.bounds.extents.y;
                    bool reachedPeak = false;
                    if (_col.bounds.center.y + targetDistance >= _targetJumpHeight)
                    {
                        targetDistance = _targetJumpHeight - _col.bounds.center.y;
                        reachedPeak = true;
                    }
                    // Check for collision if return from raycasts is lower than targetdistance we must have hit something, set falling
                    float jumpDistance = MultiRaycast(Vector2.up, targetDistance,
                        new Vector2[]
                        {
                            new Vector2(_col.bounds.center.x - _col.bounds.extents.x + EdgeRayOffset, _col.bounds.center.y),
                            _col.bounds.center,
                            new Vector2(_col.bounds.center.x + _col.bounds.extents.x - EdgeRayOffset, _col.bounds.center.y)
                        });
                    // Change state to falling 
                    if (jumpDistance < targetDistance || reachedPeak)
                    {
                        State = VerticalState.Falling;
                    }
                    // apply jumpDistance
                    jumpDistance -= _col.bounds.extents.y;
                    if (jumpDistance > 0)
                    {
                        transform.Translate(Vector2.up * jumpDistance);
                    }
                    break;
                case VerticalState.Falling:
                    float fallingDistance = MultiRaycast(Vector2.down, PlayerVars.FallSpeedBase + _col.bounds.extents.y,
                        new Vector2[]
                        {
                            new Vector2(_col.bounds.center.x - _col.bounds.extents.x + EdgeRayOffset, _col.bounds.center.y),
                            _col.bounds.center,
                            new Vector2(_col.bounds.center.x + _col.bounds.extents.x - EdgeRayOffset, _col.bounds.center.y)
                        });
                    fallingDistance -= _col.bounds.extents.y;
                    if (fallingDistance >= 0.001f)
                    {
                        transform.Translate(Vector2.down * fallingDistance);
                    }
                    else
                    {
                        // If we have stopped falling, change state to Grounded
                        State = VerticalState.Grounded;
                    }
                    break;
            }

            if (_moveInput != 0)
            {
                float distance = PlayerVars.MoveSpeedBase;
                if (_moveInput > 0)
                {
                    float rayDistance = distance + _col.bounds.extents.x;

                    Vector2 center = _col.bounds.center;
                    Vector2 upper = new Vector2(center.x, center.y + _col.bounds.extents.y - EdgeRayOffset);
                    Vector2 lower = new Vector2(center.x, center.y - _col.bounds.extents.y + EdgeRayOffset);

                    RaycastHit2D[] hits = new RaycastHit2D[3];
                    hits[0] = Raycast(upper, Vector2.right, rayDistance, CollisionMask, Color.cyan);
                    hits[1] = Raycast(center, Vector2.right, rayDistance, CollisionMask, Color.cyan);
                    hits[2] = Raycast(lower, Vector2.right, rayDistance, CollisionMask, Color.cyan);

                    float hitDistance = rayDistance + 1f;
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null && hit.distance > 0)
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
                    Vector2 upper = new Vector2(center.x, center.y + _col.bounds.extents.y - EdgeRayOffset);
                    Vector2 lower = new Vector2(center.x, center.y - _col.bounds.extents.y + EdgeRayOffset);

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

        private float MultiRaycast(Vector2 direction, float distance, Vector2[] rayOrigins)
        {
            float rayDistance = distance;
            RaycastHit2D[] hits = new RaycastHit2D[rayOrigins.Length];
            for (int i = 0; i < rayOrigins.Length; i++)
            {
                hits[i] = Raycast(rayOrigins[i], direction, rayDistance, CollisionMask, Color.blue);
            }

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
                distance = hitDistance;
            }

            return distance;
        }

        // TODO: Put this into a util class, add a bool for drawing rays
        public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, LayerMask mask, Color colour)
        {
            Debug.DrawRay(origin, direction * distance, colour);
            return Physics2D.Raycast(origin, direction, distance, mask);
        }
    }

}