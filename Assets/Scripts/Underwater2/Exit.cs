using Assets.Scripts.Map;
using Assets.Scripts.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Underwater2
{
    public class Exit : MonoBehaviour, IInteractable
    {
        public bool CanInteract()
        {
            return true;
        }

        public bool CanShowInteractionDialog()
        {
            return true;
        }

        public string GetObjectName()
        {
            return "Exit";
        }

        public void Interact()
        {
            MapManager.GetInstance().FinishMinigame(true);
            SceneManager.LoadScene(Constants.SceneNames.MapScene);
        }

        public void ShowInteractibility()
        {
            Debug.Log("Can interact with " + GetObjectName());
        }
    }
}
