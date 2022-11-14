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
        [SerializeField] TextMeshProUGUI chatBubbleText;

        public void SetCannotAffordItemText(string itemName)
        {
            chatBubbleText.text = $"It seems you don't have enough coins to buy {itemName}";
        }

        public void SetBrokenVaseText()
        {
            chatBubbleText.text = $"Sure, break my furniture and pay me with money you find in there.";
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(21);
        }

        public void SetPurchaseText(string itemName)
        {
            chatBubbleText.text = $"Thank you for buying {itemName}{Environment.NewLine}It will bring you much joy !";
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
    }
}
