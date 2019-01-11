using UnityEngine;

namespace DwarfGame
{
    public static class Utils
    {
        public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, LayerMask mask, Color colour)
        {
            Debug.DrawRay(origin, direction * distance, colour);
            return Physics2D.Raycast(origin, direction, distance, mask);
        }
        
        public static float MultiRaycast(Vector2 direction, float distance, Vector2[] rayOrigins, LayerMask collisionMask, Color colour)
        {
            float rayDistance = distance;
            RaycastHit2D[] hits = new RaycastHit2D[rayOrigins.Length];
            for (int i = 0; i < rayOrigins.Length; i++)
            {
                hits[i] = Raycast(rayOrigins[i], direction, rayDistance, collisionMask, colour);
            }

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
                distance = hitDistance;
            }

            return distance;
        }
    }
}