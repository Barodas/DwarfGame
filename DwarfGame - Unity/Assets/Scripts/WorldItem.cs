using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfGame
{
    public class WorldItem : MonoBehaviour
    {
        private BoxCollider2D _col;
        private SpriteRenderer _renderer;
        public Item Item;

        private void Start()
        {
            _col = GetComponent<BoxCollider2D>();
            _col.isTrigger = true;

            _renderer = GetComponent<SpriteRenderer>();
            _renderer.sprite = Item.ItemSprite;
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