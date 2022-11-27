using Assets.Scripts.Shared;
using UnityEngine;

namespace Assets.Scripts.Underwater2
{
    public class CraftingStation : MonoBehaviour, IInteractable
    {
        private bool hasCrafted = false;
        [SerializeField] GameObject Pier;

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
            FindObjectOfType<PlayerScript>().Say("I now have the fishing pole!", 0.075f, false, true, 3f);
            FindObjectOfType<PlayerScript>().SetArrow(Pier);
            FindObjectOfType<PlayerScript>().SetWalkingMode();
            FindObjectOfType<PlayerScript>().MinorJump();
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
