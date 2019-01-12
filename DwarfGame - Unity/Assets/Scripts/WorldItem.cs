using UnityEngine;

namespace DwarfGame
{
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class WorldItem : MonoBehaviour
    {
        private BoxCollider2D _col;
        private SpriteRenderer _renderer;

        [SerializeField]
        private Item _item;
        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                _renderer.sprite = _item.ItemSprite;
            }
        }

        public static WorldItem CreateWorldItem(Item item)
        {
            GameObject go = new GameObject(item.name, typeof(WorldItem));
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