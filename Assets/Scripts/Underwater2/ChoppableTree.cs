using Assets.Scripts.Shared;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Underwater2
{
    public class ChoppableTree : MonoBehaviour, IInteractable
    {
        private bool isChopped = false;
        public Sprite ChoppedTree;
        [SerializeField] GameObject CraftingStation;

        public bool CanInteract()
        {
            var player = FindObjectOfType<PlayerScript>();
            return !isChopped && player.Options_CanChopTrees && !MapManager.GetInstance().HasFishingPole;
        }

        public bool CanShowInteractionDialog()
        {
            var player = FindObjectOfType<PlayerScript>();
            return !isChopped && player.Options_CanChopTrees && !MapManager.GetInstance().HasFishingPole;
        }

        public string GetObjectName()
        {
            return "Tree Chopping";
        }

        public void Interact()
        {
            FindObjectOfType<PlayerScript>().Options_CanCraftFishingpole = true;
            GetComponent<SpriteRenderer>().sprite = ChoppedTree;
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(6);            
            FindObjectOfType<PlayerScript>().SetArrow(CraftingStation);
            isChopped = true;
        }

        public void ShowInteractibility()
        {
            if (!isChopped)
            {
                Debug.Log("Can interact with " + GetObjectName());
            }
        }

        void Start()
        {
            if(MapManager.GetInstance().HasFishingPole)
            {
                isChopped = true;
                GetComponent<SpriteRenderer>().sprite = ChoppedTree;
            }
        }
    }
}
