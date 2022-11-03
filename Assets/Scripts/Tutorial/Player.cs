using System;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class Player : MonoBehaviour
    {
        private TutorialManager tutorialManager;

        private void Start()
        {
            tutorialManager = FindObjectOfType<TutorialManager>();
        }

        private void Update()
        {
            HandlePlayerInput();
        }

        private void HandlePlayerInput()
        {
            if (!Input.anyKeyDown) return;

            if (Input.GetKeyDown(KeyCode.W))
            {
                tutorialManager.HandlePlayerInput(KeyCode.W);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                tutorialManager.HandlePlayerInput(KeyCode.A);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                tutorialManager.HandlePlayerInput(KeyCode.S);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                tutorialManager.HandlePlayerInput(KeyCode.D);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                tutorialManager.HandlePlayerInput(KeyCode.Mouse0);
            }

        }
    }
}
