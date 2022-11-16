using System;
using System.Collections;
using System.Collections.Generic;
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

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
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
        }

        private void HandleOxygen()
        {
            if (IsInAirOrOnGround())
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy == null) return;

            TakeDamage(enemy.Damage);
        }

        private void TakeDamage(int damage)
        {
            if (immune) return;
            health-=damage;
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
