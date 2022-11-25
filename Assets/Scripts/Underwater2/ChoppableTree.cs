using Assets.Scripts.Shared;
using UnityEngine;

namespace Assets.Scripts.Underwater2
{
    public class ChoppableTree : MonoBehaviour, IInteractable
    {
        private bool isChopped = false;

        public bool CanInteract()
        {
            var player = FindObjectOfType<PlayerScript>();
            return !isChopped && player.Options_CanChopTrees;
        }

        public bool CanShowInteractionDialog()
        {
            return !isChopped;
        }

        public string GetObjectName()
        {
            return "Tree Chopping";
        }

        public void Interact()
        {
            FindObjectOfType<PlayerScript>().Options_CanCraftFishingpole = true;
            isChopped = true;
        }

        public void ShowInteractibility()
        {
            if (!isChopped)
            {
                Debug.Log("Can interact with " + GetObjectName());
            }
        }
    }
}
