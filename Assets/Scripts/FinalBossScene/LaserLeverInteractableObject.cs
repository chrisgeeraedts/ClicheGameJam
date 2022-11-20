using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;

namespace Assets.Scripts.FinalBossScene 
{
    public class LaserLeverInteractableObject : MonoBehaviour, IInteractable
    {
        public bool Toggled;
        public Sprite NoStateSprite;
        public Sprite YesStateSprite;
        public AudioSource LeverPulledAudio;

        public LaserDamagingZoneScript DamagingZone; 
        public SpriteRenderer LeverSpriteRenderer; 

        public void Toggle(bool toggleState)
        {
            Toggled = toggleState;
            LeverPulledAudio.Play();
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
            return "Laser Control";
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
                DamagingZone.TurnOff();
            }
            else
            { 
                LeverSpriteRenderer.sprite = NoStateSprite;                
                DamagingZone.TurnOn();
            }
        }
    }
}

