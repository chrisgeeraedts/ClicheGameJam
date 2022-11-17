using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared 
{
    public interface IInteractable {

        void Interact();
        string GetObjectName();
        void ShowInteractibility();
    }
}