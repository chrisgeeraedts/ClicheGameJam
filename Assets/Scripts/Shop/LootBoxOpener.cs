using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using TMPro;

namespace Assets.Scripts.Shop
{
    public class LootBoxOpener : MonoBehaviour
    {
        [SerializeField] Image lootBoxImage;
        [SerializeField] List<ShopItemSO> shopItems;
        [SerializeField] float timeBeforeWinningItem = 4;
        [SerializeField] float timeBetweenItems = 0.1f;
        //[SerializeField] float hideOverlayDelay = 2f;
        [SerializeField] TextMeshProUGUI prizeTextField;

        private int totalLootboxWeight;
        private ShopItemSO wonItem;
        private Shopkeeper shopkeeper;

        public void ClaimPrize()
        {
            gameObject.SetActive(false);
            shopkeeper.SetPurchaseText(wonItem.ItemName);
        }

        public void StartLootbox()
        {
            StartCoroutine(LoopRandomItems());
        }

        private void Start()
        {
            totalLootboxWeight = shopItems.Sum(si => si.LootboxWeight);
            shopkeeper = FindObjectOfType<Shopkeeper>();
            wonItem = shopItems[0];
        }

        private IEnumerator LoopRandomItems()
        {
            var timeElapsed = 0f;
            while (timeElapsed <= timeBeforeWinningItem)
            {
                ShowRandomItem();
                timeElapsed += timeBetweenItems;

                yield return new WaitForSeconds(timeBetweenItems);
            }

            SetWonItem();
            Debug.Log($"You won {wonItem.ItemName}");
        }

        private void ShowRandomItem()
        {
            var itemToShow = shopItems[Random.Range(0, shopItems.Count)];
            ShowItem(itemToShow);
        }

        private void ShowItem(ShopItemSO itemToShow)
        {
            lootBoxImage.sprite = itemToShow.ItemIcon;
            prizeTextField.text = itemToShow.ItemName;
        }

        private void SetWonItem()
        {
            var randomIndex = Random.Range(0, totalLootboxWeight);
            var currentIndex = 0;

            foreach (var item in shopItems)
            {
                currentIndex += item.LootboxWeight;
                if (randomIndex <= currentIndex)
                {
                    wonItem = item;
                    ShowItem(wonItem);
                    return;
                }
            }
        }
    }
}
