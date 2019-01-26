using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfGame
{
    /// <summary>
    /// Serialized version of InstanceItem for interacting in the inspector
    /// </summary>
    [Serializable]
    public class SerializedInstanceItem
    {
        public Item Item;
        public int StackSize;
        public int CurrentDurability;

        public Dictionary<string, int> IntStore = new Dictionary<string, int>();
        
        public SerializedInstanceItem(InstanceItem instanceItem)
        {
            Item = instanceItem.Item;
            StackSize = instanceItem.StackSize;
            CurrentDurability = instanceItem.CurrentDurability;
            IntStore = instanceItem.IntStore;
        }
    }
}