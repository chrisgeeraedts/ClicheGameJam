using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared 
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        
        public void Interact()
        {
            // Do stuff
            Debug.Log("Interacting with " + GetObjectName());
        }
        public string GetObjectName()
        {
            return "Gate Lever";
        }
    }
}
