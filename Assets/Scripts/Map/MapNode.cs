using UnityEngine;
using TMPro;

namespace Assets.Scripts.Map
{
    public class MapNode : MonoBehaviour
    {
        [SerializeField] GameObject imageHolder;
        [SerializeField] TextMeshProUGUI textField;
        [SerializeField] GameObject selectionHolder;

        private MinigameInfo minigameInfo;
        private int level;
        private bool isSelected = false;

        public void SetInfo(MinigameInfo minigameInfo, int level)
        {
            this.minigameInfo = minigameInfo;
            this.level = level;
        }

        public int Level { get { return level; } }
        public MinigameInfo MinigameInfo { get { return minigameInfo; } }

        private void Start()
        {
            SetMinigameInfo();
        }

        public void SetSelected(bool isSelected)
        {
            selectionHolder.SetActive(isSelected);
        }

        private void SetMinigameInfo()
        {
            imageHolder.GetComponent<SpriteRenderer>().sprite = minigameInfo.MapSprite;
            textField.text = minigameInfo.MinigameName;
        }
    }
}
