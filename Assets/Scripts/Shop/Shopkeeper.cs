using UnityEngine;
using TMPro;
using System;
using Assets.Scripts.Map;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;

namespace Assets.Scripts.Shop
{
    public class Shopkeeper : MonoBehaviour
    {
        #region Speaking
        [Header("Speaking Bubbles")]
        [SerializeField] private EasyExpandableTextBox Speaking_Textbox;
        [SerializeField] private GameObject Speaking_Textbox_SpawnPoint;
        #endregion

        public void SetCannotAffordItemText(string itemName)
        {
            Speaking_Textbox.Show(Speaking_Textbox_SpawnPoint, 0f);
            StartCoroutine(Speaking_Textbox.EasyMessage($"It seems you don't have enough coins to buy <color=#fede34>{itemName}</color>", 0.075f, false, false, 3f));
        }

        public void SetBrokenVaseText()
        {            
            Speaking_Textbox.Show(Speaking_Textbox_SpawnPoint, 0f);
            StartCoroutine(Speaking_Textbox.EasyMessage($"Sure, break my furniture and pay me with money you find in there.", 0.075f, false, false, 3f));
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(21);
        }

        public void SetPurchaseText(string itemName)
        {            
            Speaking_Textbox.Show(Speaking_Textbox_SpawnPoint, 0f);
            StartCoroutine(Speaking_Textbox.EasyMessage($"Thank you for buying <color=#fede34>{itemName}</color>{Environment.NewLine}It will bring you much joy !", 0.075f, false, false, 3f));

            if (itemName.Equals("Bikini armor", StringComparison.InvariantCultureIgnoreCase))
            {
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(3);
            }
            else if (itemName.Equals("Lootbox", StringComparison.InvariantCultureIgnoreCase))
            {
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(28);
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
