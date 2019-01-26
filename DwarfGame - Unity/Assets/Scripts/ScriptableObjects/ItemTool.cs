using System.Collections.Generic;
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

        public override ItemParams Initialise(ItemParams args)
        {
            if (args.CurrentDurability <= 0)
            {
                args.CurrentDurability = Durability;
            }
            
            return args;
        }

        public override ItemParams LeftClickUse(ItemParams args)
        {
            // Calc Damage amount
            args.Damage = BaseDamage;
            if (args.TileClass != TileClass.None && args.TileClass == Class)
            {
                args.Damage = ClassDamage;
            }

            // Apply Damage
            base.LeftClickUse(args);
            
            // Durability
            args.CurrentDurability -= 1;
            if (args.CurrentDurability <= 0)
            {
                --args.StackSize;
            }
            
            return args;
        }
    }
}