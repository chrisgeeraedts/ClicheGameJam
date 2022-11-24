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
        public GameObject BridgeObject;

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
            InternalToggle();
            BridgeObject.SetActive(false);
        }

        void InternalToggle()
        {
            if (Toggled)
            {
                LeverSpriteRenderer.sprite = YesStateSprite;
            }
            else
            {
                LeverSpriteRenderer.sprite = NoStateSprite;
            }

            BridgeObject.SetActive(true);
        }
    }
}

