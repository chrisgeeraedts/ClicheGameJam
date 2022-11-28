using UnityEngine;
using TMPro;
using System;
using System.Collections;
using Assets.Scripts.Map;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Shop
{
    public class Shopkeeper : MonoBehaviour
    {
        [SerializeField] GameObject lootboxOpenerContainer;

        #region Speaking
        [Header("Speaking Bubbles")]
        [SerializeField] EasyExpandableTextBox Speaking_Textbox;
        [SerializeField] GameObject Speaking_Textbox_SpawnPoint;

        [SerializeField] GameObject HealingTextPrefab;
        [SerializeField] GameObject HeroHealingTextSpawnPoint;
        [SerializeField] AudioSource HeroHealingAudio;
        [SerializeField] GameObject HeroHealingContainer;

        [SerializeField] int AppleHealingAmount = 10;
        #endregion

        public bool isShowingText = false;
        private List<string> adviceList = new List<string>();
        private int totalNumberOfAdvices;
        private float timeBetweenCharacters = 0.033f;
        private float waitTime = 1.5f;

        IEnumerator canShowTextAgain(float waitTime, string messageSend, float durationPerCharacter)
        {
            yield return new WaitForSeconds((durationPerCharacter * messageSend.Length) + waitTime);
            isShowingText = false;
        }

        public void SetCannotAffordItemText(string itemName)
        {
            if (!isShowingText)
            {
                isShowingText = true;
                Speaking_Textbox.Show(Speaking_Textbox_SpawnPoint, 0f);
                string message = $"It seems you don't have enough coins to buy <color=#dd0000>{itemName}</color>";
                StartCoroutine(Speaking_Textbox.EasyMessage(message, timeBetweenCharacters, false, false, waitTime));
                StartCoroutine(canShowTextAgain(waitTime - 1f, message, timeBetweenCharacters));
            }
        }

        private void Update(){
            if (Input.GetKeyDown(KeyCode.E))
            {
                ReturnToMap();
            }
        }

        public void SetBrokenVaseText()
        {
            if (!isShowingText)
            {
                isShowingText = true;
                Speaking_Textbox.Show(Speaking_Textbox_SpawnPoint, 0f);
                string message = $"Sure, break my furniture and pay me with money you find in there.";
                StartCoroutine(Speaking_Textbox.EasyMessage(message, timeBetweenCharacters, false, false, waitTime));
                StartCoroutine(canShowTextAgain(waitTime - 1f, message, timeBetweenCharacters));
            }

            GlobalAchievementManager.GetInstance().SetAchievementCompleted(21);
        }

        public void SetPurchaseText(string itemName)
        {
            string message = String.Empty;

            if (itemName.Equals("Bikini armor", StringComparison.InvariantCultureIgnoreCase))
            {
                message = $"I don't think that will fit you{Environment.NewLine}Would you believe there are people who are fully armored by that ?";
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(3);
                MapManager.GetInstance().BikiniArmorBought();
            }
            else if (itemName.Equals("Lootbox", StringComparison.InvariantCultureIgnoreCase))
            {
                lootboxOpenerContainer.SetActive(true);
                lootboxOpenerContainer.GetComponent<LootBoxOpener>().StartLootbox();

                GlobalAchievementManager.GetInstance().SetAchievementCompleted(28);
            }
            else if (itemName.Equals("Apple", StringComparison.InvariantCultureIgnoreCase))
            {
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(8);

                MapManager.GetInstance().Heal(AppleHealingAmount);
                var healingText = Instantiate(HealingTextPrefab, HeroHealingTextSpawnPoint.transform, false);
                healingText.GetComponent<HealingNumberScript>().ShowText(AppleHealingAmount);
                HeroHealingAudio.Play();
                
                RestockItem(itemName);
            }
            else if (itemName.Equals("Cheat Codes", StringComparison.InvariantCultureIgnoreCase))
            {
                message = "Just so you know, the Koh Nah Mih code in that manual only works when you try it in my shop.";
            }
            else if (itemName.Equals("Fishing Pole", StringComparison.InvariantCultureIgnoreCase))
            {
                MapManager.GetInstance().HasFishingPole = true;
                message = "Good luck on your fishing adventure !";
            }
            else if (itemName.Equals("Golden Gun", StringComparison.InvariantCultureIgnoreCase))
            {
                MapManager.GetInstance().HasGoldenGun = true;
                message = "That gun sure looks fancy !";
            }
            else if (itemName.Equals("Sally's Advice", StringComparison.InvariantCultureIgnoreCase))
            {
                message = GetShopkeepersAdvice();
                RestockItem(itemName);
            }
            else if (itemName.Equals("Silly Hat", StringComparison.InvariantCultureIgnoreCase))
            {
                MapManager.GetInstance().HasSillyHat = true;
                FindObjectOfType<HatManager>().SwapHats();
                message = $"To be honest, I like your crown better.{Environment.NewLine}But don't let me tell you how to live your life!";
            }

            if (!isShowingText && !String.IsNullOrEmpty(message))
            {
                isShowingText = true;
                Speaking_Textbox.Show(Speaking_Textbox_SpawnPoint, 0f);

                Debug.Log($"Message {message}");

                StartCoroutine(Speaking_Textbox.EasyMessage(message, timeBetweenCharacters, false, false, waitTime));
                StartCoroutine(canShowTextAgain(waitTime - 1f, message, timeBetweenCharacters));
            }
        }

        private void RestockItem(string itemName)
        {
            var shopItems = FindObjectsOfType<ShopItem>();

            foreach (var item in shopItems)
            {
                if (item.ItemName.Equals(itemName, StringComparison.InvariantCultureIgnoreCase))
                {
                    item.Restock();
                }
            }
        }

        private string GetShopkeepersAdvice()
        {
            if (adviceList.Count == 0)
            {
                InitializeAdviceList();
            }

            var adviceIndex = Random.Range(0, adviceList.Count);
            var advice = adviceList[adviceIndex];
            adviceList.RemoveAt(adviceIndex);
            return advice;
        }

        public void ReturnToMap()
        {
            MapManager.GetInstance().FinishMinigame(true);
            SceneManager.LoadScene(Constants.SceneNames.MapScene);
        }

        void Start()
        {
            Speaking_Textbox.Hide();
            InitializeAdviceList();
        }

        private void InitializeAdviceList()
        {
            adviceList.Add("You never know when you need a fishing pole");
            adviceList.Add("Maybe you should learn how to swim");
            adviceList.Add("A Golden Gun is a Golden Gun, but a lootbox can contain ANYTHING, even a Golden Gun !");
            adviceList.Add("Don't forget to save often. Wait, what does that even mean?");
            adviceList.Add("I heard you can find coins that aren't even visible. How exciting !");

            totalNumberOfAdvices = adviceList.Count + 1;
            adviceList.Add($"Did you know there are {totalNumberOfAdvices} different advices I can give you? I am quite knowlegable !");
        }
    }
}
