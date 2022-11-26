using Assets.Scripts.Map;
using Assets.Scripts.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Underwater2
{
    public class FishingPier : MonoBehaviour, IInteractable
    {
        public bool CanInteract()
        {
            var player = FindObjectOfType<PlayerScript>();
            return player.Options_HasFishingpole;
        }

        public bool CanShowInteractionDialog()
        {
            var player = FindObjectOfType<PlayerScript>();
            return player.Options_HasFishingpole;
        }

        public string GetObjectName()
        {
            return "Fishingpole";
        }

        public void Interact()
        {
            var player = FindObjectOfType<PlayerScript>();
            player.SetPlayerActive(false);
            MapManager.GetInstance().SpawnPlayerAtPierInUnderwater = true;
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.FishingScene);
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
