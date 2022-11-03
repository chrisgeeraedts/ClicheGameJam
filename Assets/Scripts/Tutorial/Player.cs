using System;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float movespeed = 1;

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
            if (!Input.anyKey) return;

            if (Input.GetKey(KeyCode.W))
            {
                tutorialManager.HandlePlayerInput(KeyCode.W);
                if (tutorialManager.CanMoveWithKey(KeyCode.W))
                {
                    var newPosition = new Vector2(transform.position.x, transform.position.y + movespeed * Time.deltaTime);
                    transform.position = newPosition;
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (tutorialManager.CanMoveWithKey(KeyCode.A))
                {
                    var newPosition = new Vector2(transform.position.x - movespeed * Time.deltaTime, transform.position.y);
                    transform.position = newPosition;
                }
                tutorialManager.HandlePlayerInput(KeyCode.A);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                tutorialManager.HandlePlayerInput(KeyCode.S);
                if (tutorialManager.CanMoveWithKey(KeyCode.S))
                {
                    var newPosition = new Vector2(transform.position.x, transform.position.y - movespeed * Time.deltaTime);
                    transform.position = newPosition;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                tutorialManager.HandlePlayerInput(KeyCode.D);
                if (tutorialManager.CanMoveWithKey(KeyCode.D))
                {
                    var newPosition = new Vector2(transform.position.x + movespeed * Time.deltaTime, transform.position.y);
                    transform.position = newPosition;
                }
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                tutorialManager.HandlePlayerInput(KeyCode.Mouse0);
            }
        }
    }
}
