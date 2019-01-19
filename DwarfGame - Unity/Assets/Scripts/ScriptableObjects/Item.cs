using UnityEngine;

namespace DwarfGame
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public Sprite ItemSprite;
        public int StackLimit = 1;
    }
}