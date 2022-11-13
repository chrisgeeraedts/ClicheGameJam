using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    [CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop Item")]
    public class ShopItemSO : ScriptableObject
    {
        [SerializeField] string itemName;
        [SerializeField] int price;
        [SerializeField] Sprite itemIcon;

        public string ItemName => itemName;
        public int Price => price;
        public Sprite ItemIcon => itemIcon;
    }
}
