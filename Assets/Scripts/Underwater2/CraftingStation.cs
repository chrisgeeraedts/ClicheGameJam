using Assets.Scripts.Shared;
using UnityEngine;

namespace Assets.Scripts.Underwater2
{
    public class CraftingStation : MonoBehaviour, IInteractable
    {
        private bool hasCrafted = false;

        public bool CanInteract()
        {
            var player = FindObjectOfType<PlayerScript>();
            return !hasCrafted && player.Options_CanCraftFishingpole;
        }

        public bool CanShowInteractionDialog()
        {
            var player = FindObjectOfType<PlayerScript>();
            return !hasCrafted && player.Options_CanCraftFishingpole;
        }

        public string GetObjectName()
        {
            return "Fishingpole Crafting";
        }

        public void Interact()
        {
            FindObjectOfType<PlayerScript>().Options_HasFishingpole = true;
            hasCrafted = true;
        }

        public void ShowInteractibility()
        {
            if (CanInteract())
            {
                Debug.Log("Can interact with " + GetObjectName());
            }
        }
    }
}
