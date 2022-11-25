using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;

namespace Assets.Scripts.FinalBossScene 
{
    public class BridgeLever : MonoBehaviour, IInteractable
    {
        public bool Toggled;
        public Sprite NoStateSprite;
        public Sprite YesStateSprite;
        public AudioSource LeverPulledAudio;

        public SpriteRenderer LeverSpriteRenderer; 
        public Assets.Scripts.Escort.MinigameManager manager;

        public void Toggle(bool toggleState)
        {
            Toggled = toggleState;
            InternalToggle();        
        }

        public void ShowInteractibility()
        {
            if(!Toggled)
            {
                Debug.Log("Can interact with " + GetObjectName());
            }
        }
        
        public void Interact()
        {
            if(!Toggled)
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
            return "Bridge";
        }

        void Start()
        {
            InternalToggle();
        }

        void InternalToggle()
        {
            if(Toggled)
            {
                LeverSpriteRenderer.sprite = YesStateSprite;   
                manager.LeverActivated();
            }
            else
            { 
                LeverSpriteRenderer.sprite = NoStateSprite; 
                LeverPulledAudio.Play();
                manager.LeverActivated();
            }
        }
    }
}

