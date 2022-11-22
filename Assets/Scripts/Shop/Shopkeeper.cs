using UnityEngine;
using TMPro;
using System;
using System.Collections;
using Assets.Scripts.Map;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;

namespace Assets.Scripts.Shop
{
    public class Shopkeeper : MonoBehaviour
    {
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

        private bool isShowingText = false; 

        IEnumerator canShowTextAgain(float waitTime, string messageSend, float durationPerCharacter)
        {        
            yield return new WaitForSeconds((durationPerCharacter*messageSend.Length)+waitTime); 
            isShowingText = false;
        }

        public void SetCannotAffordItemText(string itemName)
        {
            if(!isShowingText)
            {
                isShowingText = true;
                Speaking_Textbox.Show(Speaking_Textbox_SpawnPoint, 0f);
                string message = $"It seems you don't have enough coins to buy <color=#dd0000>{itemName}</color>";
                StartCoroutine(Speaking_Textbox.EasyMessage(message, 0.075f, false, false, 3f));
                StartCoroutine(canShowTextAgain(2f, message, 0.075f));
            }
        }

        public void SetBrokenVaseText()
        {            
            if(!isShowingText)
            {
                isShowingText = true;
                Speaking_Textbox.Show(Speaking_Textbox_SpawnPoint, 0f);
                string message = $"Sure, break my furniture and pay me with money you find in there.";
                StartCoroutine(Speaking_Textbox.EasyMessage(message, 0.075f, false, false, 3f));
                StartCoroutine(canShowTextAgain(2f, message, 0.075f));
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(21);
            }
        }

        public void SetPurchaseText(string itemName)
        {
            string message = $"Thank you for buying <color=#fede34>{itemName}</color>{Environment.NewLine}It will bring you much joy !";

            if (itemName.Equals("Bikini armor", StringComparison.InvariantCultureIgnoreCase))
            {
                message = $"I don't think that will fit you{Environment.NewLine}Would you believe there are people fully armored by that ?";
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(3);
            }
            else if (itemName.Equals("Lootbox", StringComparison.InvariantCultureIgnoreCase))
            {
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(28);
            }
            else if (itemName.Equals("Apple", StringComparison.InvariantCultureIgnoreCase))
            {
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(8);                
                MapManager.GetInstance().Heal(AppleHealingAmount);
                
                var healingText = Instantiate(HealingTextPrefab, HeroHealingTextSpawnPoint.transform, false);
                healingText.GetComponent<HealingNumberScript>().ShowText(AppleHealingAmount);


                HeroHealingAudio.Play();
            }

            if (!isShowingText)
            {
                isShowingText = true;
                Speaking_Textbox.Show(Speaking_Textbox_SpawnPoint, 0f);

                StartCoroutine(Speaking_Textbox.EasyMessage(message, 0.075f, false, false, 3f));
                StartCoroutine(canShowTextAgain(2f, message, 0.075f));
            }
        }

        public void ReturnToMap()
        {
            MapManager.GetInstance().FinishMinigame(true);
            SceneManager.LoadScene(Constants.SceneNames.MapScene);
        }

        void Start()
        {
            Speaking_Textbox.Hide();
        }
    }
}
