using UnityEngine;

namespace DwarfGame
{
    public struct Bounds
    {
        public Vector2 Center;
        public Vector2 Size;

        public Vector2 Extents
        {
            get => Size * 0.5f;
            set => Size = value * 2f;
        }

        public float Left => Center.x - Extents.x;
        public float Right => Center.x + Extents.x;
        public float Top => Center.y + Extents.y;
        public float Bottom => Center.y - Extents.y;

        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);
        
        public Bounds(Bounds bounds)
        {
            Center = bounds.Center;
            Size = bounds.Size;
        }

        public Bounds(Vector2 center, Vector2 size)
        {
            Center = center;
            Size = size;
        }

        /// <summary>
        /// Constructor for basic blocks, assumes (1, 1) size
        /// </summary>
        /// <param name="center"></param>
        public Bounds(Vector2 center)
        {
            Center = center;
            Size = new Vector2(1, 1);
        }

        /// <summary>
        /// Check if target is colliding with this Bounds
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsOverlapping(Bounds target)
        {
            if (Left < target.Right &&
                Right > target.Left &&
                Top > target.Bottom &&
                Bottom < target.Top)
            {
                return true;
            }

            return false;
        }
    }
}