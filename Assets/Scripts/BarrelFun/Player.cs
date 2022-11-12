using Assets.Scripts.Shared;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BarrelFun
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float jumpSpeed = 10f;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private float bulletOffset = 0.6f;
        private Rigidbody2D rigidBody;
        private bool facingRight = true;
        private CapsuleCollider2D feetCollider;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            feetCollider = GetComponent<CapsuleCollider2D>();
        }
        private void Update()
        {
            HandlePlayerInput();
        }

        private void HandlePlayerInput()
        {
            HandleHorizontalMovement();
            HandleJump();
            HandleShoot();
        }

        private void HandleShoot()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonUp(0))
            {
                var xOffset = facingRight ? bulletOffset : -bulletOffset;
                var x = transform.position.x + xOffset;
                var y = transform.position.y;
                var instance = Instantiate(bulletPrefab, new Vector2(x,y), Quaternion.identity);
                var rb = instance.GetComponent<Rigidbody2D>();
                var horizontal = facingRight ? bulletSpeed : -bulletSpeed;
                rb.velocity = new Vector2(horizontal, 0);
            }
        }

        private void HandleJump()
        {
            if (!CanJump()) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
            }
        }

        private bool CanJump()
        {
            return feetCollider.IsTouchingLayers(LayerMask.GetMask(Constants.LayerNames.Ground));
        }

        private void HandleHorizontalMovement()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            rigidBody.velocity = new Vector2(horizontal * moveSpeed, rigidBody.velocity.y);

            if (Mathf.Abs(horizontal) > float.Epsilon)
            {
                facingRight = horizontal > 0;
            }
        }
    }
}
