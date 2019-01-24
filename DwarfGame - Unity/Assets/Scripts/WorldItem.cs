﻿using UnityEngine;

namespace DwarfGame
{
    /// <summary>
    /// Container for an Item that has been dropped in the world in Item form.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class WorldItem : MonoBehaviour
    {
        private const string PrefabName = "WorldItemPrefab";
        
        private BoxCollider2D _col;
        private SpriteRenderer _renderer;
        
        public InventoryItem Item;

        public static WorldItem CreateWorldItem(InventoryItem item, Vector3 position)
        {
            GameObject go = Instantiate(Resources.Load(PrefabName)) as GameObject;
            go.name = item.Item.name;
            go.transform.position = position + new Vector3(0.5f, 0.5f, 0); // Offset to center of tile
            WorldItem worldItem = go.GetComponent<WorldItem>();
            worldItem.Item = item;
            return worldItem;
        }
        
        private void Start()
        {
            _col = GetComponent<BoxCollider2D>();
            _col.isTrigger = true;

            _renderer = GetComponent<SpriteRenderer>();
            _renderer.sprite = Item.ItemSprite;
            
            Item.Initialise();
        }

        public void AddToInventory(Inventory inventory)
        {
            if (inventory.AddItemToInventory(Item))
            {
                Destroy(gameObject);
            }
        }
    }
}