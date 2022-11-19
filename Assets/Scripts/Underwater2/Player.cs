using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Underwater2
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float underwaterHorizontalSpeed = 2f;
        [SerializeField] float landHorizontalSpeed = 4f;
        [SerializeField] float jumpSpeed = 1f;
        [SerializeField] float jumpCooldown = 1f;
        [SerializeField] float landMovementAboveY = 18.5f;
        [SerializeField] float waterGravity, normalGravity;
        [SerializeField] TextMeshProUGUI healthText, oxygenText;
        [SerializeField] float oxygenDepletionRate, oxygenRechargeRate;

        private Rigidbody2D rigidBody;
        private bool facingRight = true;
        private bool canJump = true;
        private int health = 10;
        private bool immune = false;
        private float immuneTime = 1;
        private float oxygen = 100;
        private bool isActive = true;
        private bool hasFishingpole = false;
        private bool inUnderwaterBreathableLocation = false;

        public bool CanChop;
        public bool CanCraft;

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }

        //TODO NICE: Rewrite to events that UI components can subscribe to
        public bool ChopActionAvailable()
        {
            var result = CanChop && IsNearTree();
            return result;
        }

        public bool CraftActionAvailable()
        {
            var result = CanCraft && IsNearHouse();
            return result;
        }

        public bool FishingActionAvailable()
        {
            var result = hasFishingpole && IsNearWater();
            return result;
        }

        public void SetHasFishingpole(bool hasFishingpole)
        {
            //TODO: Store in MapManager
            this.hasFishingpole = hasFishingpole;
        }

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!isActive) return;

            HandlePlayerMovement();
            SetGravity();
            HandleOxygen();
            HandleChopTree();
            HandleCraftFishingpole();
            HandleStartFishing();
        }

        private void HandleChopTree()
        {
            if (!Input.GetKeyDown(KeyCode.C)) return;
            if (!ChopActionAvailable()) return;

            Debug.Log("Tree chopped");
            //TODO: Achievement trigger
            //TODO: Show player he has "some wood"
            CanChop = false;
            CanCraft = true;
        }

        private bool IsNearTree()
        {
            return transform.position.x > 99 && transform.position.x < 101;
        }

        private void HandleCraftFishingpole()
        {
            if (!Input.GetKeyDown(KeyCode.C)) return;
            if (!CraftActionAvailable()) return;

            Debug.Log("Crafted fishingpole !");
            //TODO: Show player he has a fishing pole & should go near water to start fishing
            CanCraft = false;
            hasFishingpole = true;
        }

        private bool IsNearHouse()
        {
            return transform.position.x > 102.5f && transform.position.x < 107.5f;
        }

        private void HandleStartFishing()
        {
            if (!Input.GetKeyDown(KeyCode.F)) return;
            if (!FishingActionAvailable()) return;

            Debug.Log("Let's go fishing !");
            //TODO: Switch to fishing game
        }

        private bool IsNearWater()
        {
            return transform.position.x > 95f && transform.position.x < 96f;
        }

        private void HandleOxygen()
        {
            if (CanBreathe())
            {
                oxygen += oxygenRechargeRate * Time.deltaTime;
            }
            else
            {
                oxygen -= oxygenDepletionRate * Time.deltaTime;
            }

            oxygen = Mathf.Clamp(oxygen, 0, 100);

            if (oxygen <= 0)
            {
                Lose();
            }

            oxygenText.text = $"Oxygen: {(int)oxygen}";
        }

        private bool CanBreathe()
        {
            var result = IsInAirOrOnGround() || inUnderwaterBreathableLocation;
            return result;
        }

        private void SetGravity()
        {
            if (IsInAirOrOnGround())
            {
                rigidBody.gravityScale = normalGravity;
            }
            else
            {
                rigidBody.gravityScale = waterGravity;
            }
        }

        private bool IsInAirOrOnGround()
        {
            return transform.position.y >= landMovementAboveY;
        }

        private void HandlePlayerMovement()
        {
            if (IsInAirOrOnGround())
            {
                HandleLandMovement();
            }
            else
            {
                HandleUnderwaterMovement();
            }
        }

        private void HandleLandMovement()
        {
            HandleHorizontalMovement(landHorizontalSpeed);
        }

        private void HandleUnderwaterMovement()
        {
            HandleHorizontalMovement(underwaterHorizontalSpeed);
            HandleJump();
        }

        private void HandleJump()
        {
            if (!canJump) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x * Time.deltaTime, jumpSpeed * Time.deltaTime);
                canJump = false;
                StartCoroutine(ResetJumpAfterCooldown());
            }
        }

        private IEnumerator ResetJumpAfterCooldown()
        {
            yield return new WaitForSeconds(jumpCooldown);
            Debug.Log("Reset jump");
            canJump = true;
        }


        private void HandleHorizontalMovement(float moveSpeed)
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            rigidBody.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rigidBody.velocity.y);

            if (Mathf.Abs(horizontal) > float.Epsilon)
            {
                facingRight = horizontal > 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //TODO: Add locations to breathe with trigger colliders on them
            Debug.Log("Can breathe");
            inUnderwaterBreathableLocation = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("Can NOT breathe");
            inUnderwaterBreathableLocation = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy == null) return;

            TakeDamage(enemy.Damage);
        }

        private void TakeDamage(int damage)
        {
            if (immune) return;
            health -= damage;
            healthText.text = $"Health: {health}";

            if (health <= 0)
            {
                Lose();
            }

            immune = true;
            StartCoroutine(ImmunityTimer());
        }

        private void Lose()
        {
            Debug.Log("You lose");
        }

        IEnumerator ImmunityTimer()
        {
            yield return new WaitForSeconds(immuneTime);
            immune = false;
        }
    }
}
