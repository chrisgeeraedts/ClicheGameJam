using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;

namespace Assets.Scripts.FinalBossScene 
{
    public class ElevatorScript : MonoBehaviour, IInteractable
    {
        public bool Toggled;
        public AudioSource LeverPulledAudio;
        public GameObject Elevator; 
        public float MoveUpAmount;
        public float Speed;

        public void Toggle(bool toggleState)
        {
            Toggled = toggleState;
            LeverPulledAudio.Play();   
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
        public string GetObjectName()
        {
            return "Elevator";
        }

        void Start()
        {
            targetPosition = new Vector3(Elevator.transform.position.x, Elevator.transform.position.y+MoveUpAmount,0);
        }

        private Vector3 targetPosition;

        void Update() {
            if(Toggled)
            {
                var step =  Speed * Time.deltaTime; // calculate distance to move
                Elevator.transform.position = Vector3.MoveTowards(Elevator.transform.position, targetPosition, step);
            }
        }

        
    }
}

