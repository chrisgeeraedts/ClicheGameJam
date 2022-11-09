using UnityEngine;
using TMPro;

namespace Assets.Scripts.Map
{
    public class MapNode : MonoBehaviour
    {
        [SerializeField] GameObject minigameImageHolder;
        [SerializeField] TextMeshProUGUI textField;
        [SerializeField] GameObject selectionHightlightHolder;
        [SerializeField] GameObject minigameLockedHolder;
        [SerializeField] GameObject minigameWonHolder;

        private MinigameInfo minigameInfo;

        public void SetInfo(MinigameInfo minigameInfo)
        {
            this.minigameInfo = minigameInfo;
        }

        public MinigameInfo MinigameInfo { get => minigameInfo; }

        private void Start()
        {
            SetMinigameInfo();
        }

        public void SetSelected(bool isSelected)
        {
            selectionHightlightHolder.SetActive(isSelected);
        }

        public void SetLocked(bool isLocked)
        {
            minigameLockedHolder.SetActive(isLocked);
        }

        public void SetWon(bool isWon)
        {
            minigameWonHolder.SetActive(isWon);
        }

        private void SetMinigameInfo()
        {
            minigameImageHolder.GetComponent<SpriteRenderer>().sprite = minigameInfo.MapSprite;
            textField.text = minigameInfo.MinigameName;
        }
    }
}
