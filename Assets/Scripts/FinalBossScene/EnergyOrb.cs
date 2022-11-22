using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;

namespace Assets.Scripts.FinalBossScene 
{
    public class EnergyOrb : MonoBehaviour, IInteractable
    {
        public bool Toggled;
        public AudioSource LeverPulledAudio;
        public FinalBossStage3Script FinalBossStage3Script;

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
            return "Energy Sphere";
        }

        void Start()
        {
        }

        void InternalToggle()
        {
            if(Toggled)
            { 
                LeverPulledAudio.Play();
                FinalBossStage3Script.ActivateEnergySphere();
            }
        }
    }
}

