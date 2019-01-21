using UnityEngine;

namespace DwarfGame
{
    [CreateAssetMenu]
    public class ItemTool : Item
    {
        public TileClass Class = TileClass.None;
        public int BaseDamage = 1;
        public int ClassDamage = 2;
        
        public override void Use(Vector2 position)
        {
        }
    }
}