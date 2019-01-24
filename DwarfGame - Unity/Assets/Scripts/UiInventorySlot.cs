using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DwarfGame
{
    public class UiInventorySlot : MonoBehaviour
    {
        private Image _image;
        private TextMeshProUGUI _text;
        public GameObject DurabilityBarContainer;
        public RectTransform DurabilityBar;

        private float _durabilityRange;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<TextMeshProUGUI>();

            _durabilityRange = DurabilityBar.rect.width;
        }

        public void ClearSlot()
        {
            _image.sprite = null;
            _text.enabled = false;
            DurabilityBarContainer.SetActive(false);
        }

        public void UpdateSlot(Sprite newSprite, int textValue, float durabilityPercent = 0)
        {
            // Icon
            _image.sprite = newSprite;

            // Stack size text
            if (textValue > 1)
            {
                _text.enabled = true;
                _text.text = textValue.ToString();
            }
            else
            {
                _text.enabled = false;
            }
            
            // Durability bar
            if (durabilityPercent > 0)
            {
                DurabilityBarContainer.SetActive(true);
                DurabilityBar.sizeDelta = new Vector2(_durabilityRange * durabilityPercent, DurabilityBar.sizeDelta.y);
            }
            else
            {
                DurabilityBarContainer.SetActive(false);
            }
            
        }
    }
}