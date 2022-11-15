using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Map
{
    public class MapNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] GameObject minigameImageHolder;
        [SerializeField] TextMeshProUGUI textField;
        [SerializeField] GameObject selectionHightlightHolder;
        [SerializeField] GameObject minigameLockedHolder;
        [SerializeField] GameObject minigameWonHolder;
        [SerializeField] public Button MinigameButton;        

        public int X;
        public int Y;

        private MinigameInfo minigameInfo;

        public void DisableButton()
        {
            MinigameButton.interactable = false;
        }

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
            MinigameButton.interactable = !isLocked;
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

        public void OnPointerEnter (PointerEventData eventData) 
        {
            if(MinigameButton.interactable && !minigameInfo.IsFinished)
            {
                //SetSelected(true);
                OnMouseEntered?.Invoke(this, new MouseEnterEventArgs(X, Y));
            }
        }

        public void OnPointerExit(PointerEventData eventData) 
        {
            if(MinigameButton.interactable && !minigameInfo.IsFinished)
            {
                //SetSelected(false);
            }
            
        }

        public event EventHandler<MouseEnterEventArgs> OnMouseEntered;
    }

    public class MouseEnterEventArgs : EventArgs
    {
        public MouseEnterEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;
    }
}
