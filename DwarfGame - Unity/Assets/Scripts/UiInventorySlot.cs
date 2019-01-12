﻿using UnityEngine;
using UnityEngine.UI;

namespace DwarfGame
{
    public class UiInventorySlot : MonoBehaviour
    {
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        public void UpdateSprite(Sprite newSprite)
        {
            _image.sprite = newSprite;
        }
    }
}