using UnityEngine;
using Assets.Scripts.Shared;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Tutorial
{
    public class LeverControl : MonoBehaviour, IInteractable
    {
        public bool Toggled;
        public Sprite NoStateSprite;
        public Sprite YesStateSprite;
        public AudioSource LeverPulledAudio;
        public AudioSource BridgeDroppedAudio;
        public GameObject BridgeObject;
        public GameObject LeverTutorialInteractable;

        public SpriteRenderer LeverSpriteRenderer;

        public void Toggle(bool toggleState)
        {
            Toggled = toggleState;
            LeverPulledAudio.Play();
            InternalToggle();
        }

        public void ShowInteractibility()
        {
            if (!Toggled)
            {
                Debug.Log("Can interact with " + GetObjectName());
            }
        }

        public void Interact()
        {
            if (!Toggled)
            {
                Debug.Log("Interacting with " + GetObjectName());
                Toggle(true);
                LeverTutorialInteractable.SetActive(false);
            }
        }

        public bool CanInteract()
        {
            return !Toggled;
        }
        public bool CanShowInteractionDialog()
        {
            return !Toggled;
        }

        public string GetObjectName()
        {
            return "Lever Control";
        }

        void Start()
        {
            InternalToggle(false);
            BridgeObject.SetActive(false);
        }

        void InternalToggle(bool playAudio = true)
        {
            if (Toggled)
            {
                LeverSpriteRenderer.sprite = YesStateSprite;
            }
            else
            {
                LeverSpriteRenderer.sprite = NoStateSprite;
            }

            if (playAudio)
            {
                BridgeDroppedAudio.Play();
            }
            BridgeObject.SetActive(true);
        }
    }
}

