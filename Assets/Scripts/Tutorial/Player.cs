using UnityEngine;
using Assets.Scripts.Shared;
using static Assets.Scripts.Shared.Enums;

namespace Assets.Scripts.Tutorial
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] float movespeed = 1;
        [SerializeField] Sprite playerIdle;
        [SerializeField] Sprite playerMovingRight;
        [SerializeField] Sprite playerMovingLeft;
        [SerializeField] Sprite playerMovingUp;
        [SerializeField] Sprite playerMovingDown;

        private TutorialManager tutorialManager;
        private SpriteRenderer _renderer;

        private void Start()
        {
            tutorialManager = FindObjectOfType<TutorialManager>();
            _renderer = GetComponent<SpriteRenderer>();
            GetComponent<Rigidbody2D>().gravityScale = 0f;
            SetPlayerActive(true);
        }

        private void Update()
        {
            if(IsPlayerActive())
            {
                HandlePlayerInput();
            }
        }

        private void HandlePlayerInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                tutorialManager.HandlePlayerInput(KeyCode.W);
                if (tutorialManager.CanMoveWithKey(KeyCode.W))
                {
                    var newPosition = new Vector2(transform.position.x, transform.position.y + movespeed * Time.deltaTime);
                    transform.position = newPosition;
                    SwitchPlayerSprite(Direction.Up);
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                tutorialManager.HandlePlayerInput(KeyCode.A);
                if (tutorialManager.CanMoveWithKey(KeyCode.A))
                {
                    var newPosition = new Vector2(transform.position.x - movespeed * Time.deltaTime, transform.position.y);
                    transform.position = newPosition;
                    SwitchPlayerSprite(Direction.Left);
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                tutorialManager.HandlePlayerInput(KeyCode.S);
                if (tutorialManager.CanMoveWithKey(KeyCode.S))
                {
                    var newPosition = new Vector2(transform.position.x, transform.position.y - movespeed * Time.deltaTime);
                    transform.position = newPosition;
                    SwitchPlayerSprite(Direction.Down);
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                tutorialManager.HandlePlayerInput(KeyCode.D);
                if (tutorialManager.CanMoveWithKey(KeyCode.D))
                {
                    var newPosition = new Vector2(transform.position.x + movespeed * Time.deltaTime, transform.position.y);
                    transform.position = newPosition;
                    SwitchPlayerSprite(Direction.Right);
                }
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                tutorialManager.HandlePlayerInput(KeyCode.Mouse0);
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                tutorialManager.HandlePlayerInput(KeyCode.Space);
            }
            else if (Input.anyKey)
            {
                tutorialManager.HandlePlayerInput(KeyCode.Z);
            }
        }

        private void SwitchPlayerSprite(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    _renderer.sprite = playerMovingUp;
                    break;
                case Direction.Down:
                    _renderer.sprite = playerMovingDown;
                    break;
                case Direction.Left:
                    _renderer.sprite = playerMovingLeft;
                    break;
                case Direction.Right:
                    _renderer.sprite = playerMovingRight;
                    break;
                default:
                    break;
            }
        }

        private bool _isActive;
        public void SetPlayerActive(bool active)
        {
            _isActive = active;
        }
        public bool IsPlayerActive()
        {
            return _isActive;
        }
    }
}
