using UnityEngine;

namespace DwarfGame
{
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class WorldItem : MonoBehaviour
    {
        private const string PrefabName = "WorldItemPrefab";
        
        private BoxCollider2D _col;
        private SpriteRenderer _renderer;
        
        public Item Item;

        public static WorldItem CreateWorldItem(Item item, Vector3 position)
        {
            GameObject go = Instantiate(Resources.Load(PrefabName)) as GameObject;
            go.name = item.name;
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