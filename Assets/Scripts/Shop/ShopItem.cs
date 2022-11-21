﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Map;

namespace Assets.Scripts.Shop
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] ShopItemSO shopItemSo;
        [SerializeField] SpriteRenderer itemIcon;
        [SerializeField] TextMeshProUGUI priceText;
        [SerializeField] TextMeshProUGUI itemText;
        [SerializeField] Sprite soldOutSprite;
        [SerializeField] AudioClip soldClip;
        [SerializeField] Button BuyButton;

        private bool bought = false;

        private void Start()
        {
            itemIcon.sprite = shopItemSo.ItemIcon;
            priceText.text = shopItemSo.Price.ToString();
            itemText.text = shopItemSo.ItemName;
        }

        public void OnClick()
        {
            if (bought) return;

            Debug.Log($"Tried to buy {shopItemSo.ItemName}");
            if (MapManager.GetInstance().SpendCoins(shopItemSo.Price))
            {
                BuyButton.interactable = false;
                itemIcon.sprite = soldOutSprite;
                bought = true;
                AudioSource.PlayClipAtPoint(soldClip, transform.position);
                FindObjectOfType<Shopkeeper>()?.SetPurchaseText(shopItemSo.ItemName);
            }
            else
            {
                FindObjectOfType<Shopkeeper>()?.SetCannotAffordItemText(shopItemSo.ItemName);
            }
        }
    }
}
