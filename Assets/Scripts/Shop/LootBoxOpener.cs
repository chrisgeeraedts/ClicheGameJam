using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

namespace Assets.Scripts.Shop
{
    public class LootBoxOpener : MonoBehaviour
    {
        [SerializeField] Image lootBoxImage;
        [SerializeField] List<ShopItemSO> shopItems;
        [SerializeField] float timeBeforeWinningItem = 4;
        [SerializeField] float timeBetweenItems = 0.1f;
        [SerializeField] float hideOverlayDelay = 2f;

        private int totalLootboxWeight;
        private ShopItemSO wonItem;

        public void StartLootbox()
        {
            StartCoroutine(LoopRandomItems());
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
            //TODO: Pass on to shopkeeper as a 'bought' item
            StartCoroutine(HideLootboxOverlay());
        }

        private void ShowRandomItem()
        {
            var itemToShow = shopItems[Random.Range(0, shopItems.Count)];
            lootBoxImage.sprite = itemToShow.ItemIcon;
        }

        private IEnumerator HideLootboxOverlay()
        {
            yield return new WaitForSeconds(hideOverlayDelay);

            gameObject.SetActive(false);
        }

        private void Start()
        {
            totalLootboxWeight = shopItems.Sum(si => si.LootboxWeight);
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
                    lootBoxImage.sprite = wonItem.ItemIcon;
                    return;
                }
            }

        }
    }
}
