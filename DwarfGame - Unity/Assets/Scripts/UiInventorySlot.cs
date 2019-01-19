using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DwarfGame
{
    public class UiInventorySlot : MonoBehaviour
    {
        private Image _image;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void ClearSlot()
        {
            _image.sprite = null;
            _text.enabled = false;
        }

        public void UpdateSlot(Sprite newSprite, int textValue)
        {
            _image.sprite = newSprite;

            if (textValue > 1)
            {
                _text.enabled = true;
                _text.text = textValue.ToString();
            }
            else
            {
                _text.enabled = false;
            }
        }
    }
}