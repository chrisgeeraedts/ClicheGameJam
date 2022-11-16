using System;
using UnityEngine;

namespace Assets.Scripts.Underwater2
{
    public enum EnemyMovement
    {
        Static,
        LeftRight,
        UpDown
    }

    public class Enemy : MonoBehaviour
    {
        [SerializeField] string enemyName;
        [SerializeField] int health;
        [SerializeField] int damage;
        [SerializeField] Sprite sprite;
        [SerializeField] EnemyMovement enemyMovement;
        [SerializeField] float xMovespeed, yMovespeed;
        [SerializeField] float boundsLeftX, boundsRightX, boundsBottomY, boundsTopY;
        [SerializeField] bool flipSpriteXOnXFlip, flipSpriteYOnYFlip;

        private Vector3 currentMovement;
        private SpriteRenderer spriteRenderer;

        public int Damage => damage;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            InitializeCurrentMovement();
        }

        private void Update()
        {
            Move();
        }

        private void InitializeCurrentMovement()
        {
            switch (enemyMovement)
            {
                case EnemyMovement.Static:
                    break;
                case EnemyMovement.LeftRight:
                    currentMovement = new Vector2(xMovespeed, 0);
                    break;
                case EnemyMovement.UpDown:
                    currentMovement = new Vector2(0, yMovespeed);
                    break;
                default:
                    break;
            }
        }

        private void Move()
        {
            switch (enemyMovement)
            {
                case EnemyMovement.Static:
                    //No movement
                    break;
                case EnemyMovement.LeftRight:
                    MoveLeftRight();
                    break;
                case EnemyMovement.UpDown:
                    MoveUpDown();
                    break;
                default:
                    break;
            }

            ApplyBounds();
        }

        private void ApplyBounds()
        {
            if ((currentMovement.x > 0 && transform.position.x >= boundsRightX) ||
                (currentMovement.x < 0 && transform.position.x <= boundsLeftX))
            {
                FlipMovementX();
            }

            if ((currentMovement.y > 0 && transform.position.y >= boundsTopY) ||
                (currentMovement.y < 0 && transform.position.y <= boundsBottomY))
            {
                FlipMovementY();
            }
        }

        private void MoveLeftRight()
        {
            transform.position += currentMovement * Time.deltaTime;
        }

        private void MoveUpDown()
        {
            transform.position += currentMovement * Time.deltaTime;
        }

        private void FlipMovementX()
        {
            currentMovement = new Vector3(-currentMovement.x, currentMovement.y);

            if (flipSpriteXOnXFlip)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }

        private void FlipMovementY()
        {
            currentMovement = new Vector3(currentMovement.x, -currentMovement.y);
            if (flipSpriteYOnYFlip)
            {
                spriteRenderer.flipY = !spriteRenderer.flipY;
            }
        }
    }
}
