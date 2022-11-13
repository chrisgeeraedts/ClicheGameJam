using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Shop
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] ShopItemSO shopItemSo;
        [SerializeField] SpriteRenderer itemIcon;
        [SerializeField] TextMeshProUGUI priceText;
        [SerializeField] Sprite soldOutSprite;


        private void Start()
        {
            itemIcon.sprite = shopItemSo.ItemIcon;
            priceText.text = shopItemSo.Price.ToString();
        }

        void OnClick()
        { 
            //Add button component
            //Buy
        }
    }
}
