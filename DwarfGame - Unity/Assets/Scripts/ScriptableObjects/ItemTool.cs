using UnityEngine;

namespace DwarfGame
{
    /// <summary>
    /// An Item that is used as a tool.
    /// </summary>
    [CreateAssetMenu]
    public class ItemTool : Item
    {
        public TileClass Class = TileClass.None;
        public int BaseDamage = 1;
        public int ClassDamage = 2;
        
        // TODO: Item Durability using IntStore
        
        public override ResolutionParams LeftClickUse(TargetParams args)
        {
            args.Damage = BaseDamage;
            if (args.TileClass != TileClass.None && args.TileClass == Class)
            {
                args.Damage = ClassDamage;
            }

            base.LeftClickUse(args);
            
            return new ResolutionParams{TargetType = args.TargetType, Position = args.TargetPosition, Damage = args.Damage, StackSize = args.StackSize, IntStore = args.IntStore};
        }
    }
}