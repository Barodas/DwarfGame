﻿using UnityEditor;
using UnityEngine;

namespace DwarfGame
{
    [CustomPropertyDrawer(typeof(InventoryItem))]
    public class InventoryItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            Rect contentPosition = EditorGUI.PrefixLabel(position, label);
            contentPosition.width *= 0.75f;
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("Item"), GUIContent.none);
            contentPosition.x += contentPosition.width;
            contentPosition.width /= 3f;
            EditorGUIUtility.labelWidth = 14f;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("StackSize"),
                new GUIContent("#"));
            EditorGUI.EndProperty();
        }
    }
}