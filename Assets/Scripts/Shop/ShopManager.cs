using UnityEngine;
using TMPro;
using Assets.Scripts.Map;

namespace Assets.Scripts.Shop
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI coinsText;

        private void Start()
        {
            UpdateCoinsText(MapManager.GetInstance().Coins);
        }
        public void UpdateCoinsText(int numberOfCoins)
        {
            coinsText.text = $"<color=#fede34>{numberOfCoins}</color>";
        }
    }
}
