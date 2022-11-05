using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Map
{
    public class MapNode : MonoBehaviour
    {
        [SerializeField] GameObject imageHolder;
        [SerializeField] TextMeshProUGUI textField;

        private MinigameInfo minigameInfo;
        private int level;

        public void SetInfo(MinigameInfo minigameInfo, int level)
        {
            this.minigameInfo = minigameInfo;
            this.level = level;
        }

        public int Level { get { return level; } }

        private void Start()
        {
            SetMinigameInfo();
        }

        private void SetMinigameInfo()
        {
            imageHolder.GetComponent<SpriteRenderer>().sprite = minigameInfo.MapSprite;
            textField.text = minigameInfo.MinigameName;
        }
    }
}
