using UnityEditor;
using UnityEngine;

namespace DwarfGame
{
    [CustomEditor(typeof(Inventory))]
    public class InventoryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        
            Inventory inventory = (Inventory)target;
            if(GUILayout.Button("Clear Inventory"))
            {
                inventory.ClearInventory();
            }
        }
    }
}