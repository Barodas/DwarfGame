using UnityEngine;

namespace DwarfGame
{
    /// <summary>
    /// Blueprint of an item. The SO contains the default stats and functionality that are used by the InstanceItem.
    /// </summary>
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public Sprite ItemSprite;
        public int StackLimit = 1;
        public int Durability;

        /// <summary>
        /// Allows more advanced items to set up any data store information they need
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual ItemParams Initialise(ItemParams args)
        {
            return args;
        }
        
        /// <summary>
        /// Generally treated as placing the item in some way
        /// </summary>
        /// <param name="args">Contains various input arguments based on target object</param>
        public virtual ItemParams RightClickUse(ItemParams args)
        {
            args.ResolutionType = ResolutionType.None;
            return args;
        }
        
        /// <summary>
        /// Generally treated as swinging the item at the target position
        /// </summary>
        /// <param name="args">Contains various input arguments based on target object</param>
        public virtual ItemParams LeftClickUse(ItemParams args)
        {
            TilemapManager.Instance.DamageTile(TileLayer.Terrain,
                TilemapManager.Instance.TerrainTilemap.WorldToCell(
                    args.TargetPosition),
                args.Damage, 
                args.HitDirection);
            
            return args;
        }
        
        public static void LeftClickUseEmpty(ItemParams args)
        {
            TilemapManager.Instance.DamageTile(TileLayer.Terrain,
                TilemapManager.Instance.TerrainTilemap.WorldToCell(
                    args.TargetPosition),
                args.Damage, 
                args.HitDirection);
        }
    }
}