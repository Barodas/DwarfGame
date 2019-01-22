using UnityEngine;

namespace DwarfGame
{
    /// <summary>
    /// Blueprint of an item. The SO contains the default stats and functionality that are used by the InventoryItem.
    /// </summary>
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public Sprite ItemSprite;
        public int StackLimit = 1;

        public virtual void Use(Vector2 position)
        {
        }
    }
}