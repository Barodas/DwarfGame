using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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
        public Inventory PlayerInventory;

        private Tilemap _terrain;
        private BoxCollider2D _col;

        private VerticalState State { get; set; } = VerticalState.Grounded;

        private float _targetJumpHeight;
        private float _moveInput;
        private bool _canSwing = true;

        private Vector2 Center => _col.bounds.center;
        private Vector2 Extents => _col.bounds.extents;

        private Dictionary<Vector2, HitDirection> _normalToHitDirection = new Dictionary<Vector2, HitDirection>
        {
            {new Vector2(0, 1), HitDirection.Top},
            {new Vector2(0, -1), HitDirection.Bottom},
            {new Vector2(-1, 0), HitDirection.Left},
            {new Vector2(1, 0), HitDirection.Right}
        };
        
        private void Start()
        {
            _terrain = TilemapManager.Instance.TerrainTilemap;
            _col = GetComponent<BoxCollider2D>();
        }
    
        private void Update()
        {
            switch (State)
            {
                case VerticalState.Grounded:
                    if (Input.GetKey(KeyCode.Space))
                    {
                        _targetJumpHeight = Center.y + Extents.y + (PlayerVars.JumpHeightBase * PlayerVars.JumpHeightModifier[PlayerVars.JumpHeightCurModifier]) + 0.2f;
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
            
            
            // Player focus raycast
            Vector2 rayDirection = ((Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition) - Center).normalized;
            float rayDistance = Mathf.Min(((Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition) - Center).magnitude, PlayerVars.PlayerReach);
            RaycastHit2D hit = Utils.Raycast(Center, rayDirection, rayDistance, CollisionMask, Color.red);
            
            // Block removal test code
            if(Input.GetMouseButton(0))
            {
                if (_canSwing)
                {
                    Vector2 targetPosition = hit.point;
                    if (hit.collider != null)
                    {
                        targetPosition += (-hit.normal * 0.1f);
                    }
                    TilemapManager.Instance.DamageTile(TileLayer.Terrain,
                        TilemapManager.Instance.TerrainTilemap.WorldToCell(
                            targetPosition),
                        20, _normalToHitDirection[hit.normal]);

                    StartCoroutine(SwingTimer());
                }
            }
            
            // Block Placement from inventory
            if (Input.GetMouseButtonDown(1))
            {
                // TODO: Do not allow placement if the player occupies the target tile
                Vector2 targetPosition = hit.point;
                if (hit.collider != null)
                {
                    targetPosition += (hit.normal * 0.1f);
                }
                PlayerInventory.UseSelectedItem(targetPosition);
            }
            
            // UI Slot selection // TODO: Should this be in a separate script?
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayerInventory.ChangeSelectedSlot(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayerInventory.ChangeSelectedSlot(1);
            }

            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
            if (scroll != 0)
            {
                if (scroll > 0)
                {
                    PlayerInventory.ChangeSelectedSlot(PlayerInventory.SelectedSlot - 1);
                }
                else
                {
                    PlayerInventory.ChangeSelectedSlot(PlayerInventory.SelectedSlot + 1);
                }
            }
        }

        private void FixedUpdate()
        {
            // Vertical state
            switch (State)
            {
                case VerticalState.Grounded:
                {
                    // Check if we should be falling 
                    float groundDistance = Utils.MultiRaycast(Vector2.down, PlayerVars.FallSpeedBase + Extents.y,
                        new Vector2[]
                        {
                            new Vector2(_col.bounds.center.x - Extents.x + EdgeRayOffset, Center.y),
                            Center,
                            new Vector2(Center.x + Extents.x - EdgeRayOffset, Center.y)
                        }, CollisionMask, Color.blue);
                    if (groundDistance - Extents.y > 0)
                    {
                        State = VerticalState.Falling;
                    }
                    break;
                }
                case VerticalState.Jumping:
                    // Clamp distance to targetHeight
                    float targetDistance = PlayerVars.JumpSpeedBase + Extents.y;
                    bool reachedPeak = false;
                    if (Center.y + targetDistance >= _targetJumpHeight)
                    {
                        targetDistance = _targetJumpHeight - Center.y;
                        reachedPeak = true;
                    }
                    // Check for collision if return from raycasts is lower than targetdistance we must have hit something, set falling
                    float jumpDistance = Utils.MultiRaycast(Vector2.up, targetDistance,
                        new Vector2[]
                        {
                            new Vector2(Center.x - Extents.x + EdgeRayOffset, Center.y),
                            Center,
                            new Vector2(Center.x + Extents.x - EdgeRayOffset, Center.y)
                        }, CollisionMask, Color.blue);
                    // Change state to falling 
                    if (jumpDistance < targetDistance || reachedPeak)
                    {
                        State = VerticalState.Falling;
                    }
                    // apply jumpDistance
                    jumpDistance -= Extents.y;
                    if (jumpDistance > 0)
                    {
                        transform.Translate(Vector2.up * jumpDistance);
                    }
                    break;
                case VerticalState.Falling:
                    float fallingDistance = Utils.MultiRaycast(Vector2.down, PlayerVars.FallSpeedBase + Extents.y,
                        new Vector2[]
                        {
                            new Vector2(Center.x - Extents.x + EdgeRayOffset, Center.y),
                            Center,
                            new Vector2(Center.x + Extents.x - EdgeRayOffset, Center.y)
                        }, CollisionMask, Color.blue);
                    fallingDistance -= Extents.y;
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

            // Horizontal state
            if (_moveInput != 0)
            {
                float distance = PlayerVars.MoveSpeedBase + Extents.x;
                Vector2 direction = _moveInput > 0 ? Vector2.right : Vector2.left;

                distance = Utils.MultiRaycast(direction, distance,
                    new Vector2[]
                    {
                        new Vector2(Center.x, Center.y + Extents.y - EdgeRayOffset),
                        Center,
                        new Vector2(Center.x, Center.y - Extents.y + EdgeRayOffset),
                    }, CollisionMask, Color.blue);
                distance -= Extents.x;

                transform.Translate(direction * distance);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            WorldItem worldItem = other.gameObject.GetComponent<WorldItem>();
            if (worldItem != null)
            {
                worldItem.AddToInventory(PlayerInventory);
            }
        }

        private IEnumerator SwingTimer()
        {
            _canSwing = false;
            yield return new WaitForSecondsRealtime(PlayerVars.SwingSpeed);
            _canSwing = true;
        }
    }

}