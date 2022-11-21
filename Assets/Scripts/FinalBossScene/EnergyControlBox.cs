using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;

namespace Assets.Scripts.FinalBossScene 
{
    public class EnergyControlBox : MonoBehaviour, IInteractable
    {
        public bool Toggled;
        public Sprite NoStateSprite;
        public Sprite YesStateSprite;
        public AudioSource LeverPulledAudio;
        public FinalBossStage3Script FinalBossStage3Script;
        public int EnergyBeamId;

        public SpriteRenderer LeverSpriteRenderer; 

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
            return "Energy Converter";
        }

        void Start()
        {
        }

        void InternalToggle()
        {
            if(Toggled)
            { 
                LeverPulledAudio.Play();
                LeverSpriteRenderer.sprite = YesStateSprite; 
                FinalBossStage3Script.ActivateEnergyBeam(EnergyBeamId);
            }
        }
    }
}

