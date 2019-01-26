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
        public int Durability;

        /// <summary>
        /// Allows more advanced items to set up any data store information they need
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual ResolutionParams Initialise(TargetParams args)
        {
            return new ResolutionParams(args);
        }
        
        /// <summary>
        /// Generally treated as placing the item in some way
        /// </summary>
        /// <param name="args">Contains various input arguments based on target object</param>
        public virtual ResolutionParams RightClickUse(TargetParams args)
        {
            ResolutionParams resolution = new ResolutionParams(args);
            resolution.ResolutionType = ResolutionType.None;
            
            return resolution;
        }
        
        /// <summary>
        /// Generally treated as swinging the item at the target position
        /// </summary>
        /// <param name="args">Contains various input arguments based on target object</param>
        public virtual ResolutionParams LeftClickUse(TargetParams args)
        {
            TilemapManager.Instance.DamageTile(TileLayer.Terrain,
                TilemapManager.Instance.TerrainTilemap.WorldToCell(
                    args.TargetPosition),
                args.Damage, 
                args.HitDirection);
            
            return new ResolutionParams(args);
        }
    }
}