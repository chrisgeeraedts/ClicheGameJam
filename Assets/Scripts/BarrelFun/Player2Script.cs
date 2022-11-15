using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using Assets.Scripts.Shared;

namespace Assets.Scripts.BarrelFun
{
    public enum FacingDirection
    {
        Left,
        Right
    }

    public class Player2Script : MonoBehaviour, Assets.Scripts.Shared.IPlayer {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float jumpSpeed = 10f;
        [SerializeField] private float m_speed = 10f;
        [SerializeField] private float m_jumpForce = 10f;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private GameObject ShootStartPoint;
        [SerializeField] private AudioSource GunFireAudio;
        [SerializeField] private BoxCollider2D feetCollider;
        
        private Animator            m_animator;
        private Rigidbody2D         m_body2d;
        private Sensor_Player       m_groundSensor;
        private bool                m_grounded = false;
        private float               m_delayToIdle = 0.0f;
        private bool facingRight = true;
        private float SpellBaseIntensity;
        private FacingDirection facingDirection;

        void Awake(){
            m_animator = GetComponent<Animator>();
            m_body2d = GetComponent<Rigidbody2D>();
            m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Player>();        
            facingDirection = FacingDirection.Right;
        }

        void Update ()
        {
            if(IsPlayerActive())
            {
                HandleHorizontalMovement();
                FlipCharacter(Input.GetAxis("Horizontal"));
                HandleJump();
                HandleShoot();

                if (m_body2d.velocity.magnitude > 0)
                {
                    m_delayToIdle = 0.05f;
                    m_animator.SetInteger("AnimState", 1);
                }
                else
                {
                    m_animator.SetInteger("AnimState", 0);
                }
            }
        }
       
        public void StopMovement()
        {
            m_body2d.velocity = Vector3.zero;
        }

        public void ToggleGravity(bool toggle)
        {
            if(toggle)
            {
                m_body2d.gravityScale = 1f;
            }
            else
            {
                m_body2d.gravityScale = 0f;
            }
            
        }

        public void Reposition(Vector3 newPos)
        {
            gameObject.transform.position = newPos;
        }

        private void HandleShoot()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonUp(0))
            {
                //var xOffset = facingRight ? bulletOffset : -bulletOffset;
                var bulletSpawnPoint = ShootStartPoint.transform.position;                
                m_animator.SetTrigger("Attack");
                var instance = Instantiate(bulletPrefab, bulletSpawnPoint, Quaternion.identity);
                GunFireAudio.Play();
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
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, jumpSpeed);
                m_groundSensor.Disable(0.2f);
            }
        }

        private bool CanJump()
        {
            bool canJump = feetCollider.IsTouchingLayers(LayerMask.GetMask(Constants.LayerNames.Ground));
            m_animator.SetBool("Grounded", canJump);
            return canJump;
        }

        private void HandleHorizontalMovement()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            m_body2d.velocity = new Vector2(horizontal * moveSpeed, m_body2d.velocity.y);

            if (Mathf.Abs(horizontal) > float.Epsilon)
            {
                facingRight = horizontal > 0;
            }
        }


        void FlipCharacter(float moveInput)
        {

            if (moveInput > 0)
            {
                if(facingDirection == FacingDirection.Right)
                {
                    transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                facingDirection = FacingDirection.Right;
            }
            else if (moveInput < 0)            
            {
                if(facingDirection == FacingDirection.Right)
                {
                    transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                facingDirection = FacingDirection.Left;
            }

            
        }

        void MoveCharacter(float moveInput)
        {
            
            if (moveInput > 0)
            {
                if(facingDirection == FacingDirection.Right)
                {
                    m_body2d.velocity = new Vector2(moveInput * m_speed, m_body2d.velocity.y);
                }
                else
                {
                    m_body2d.velocity = new Vector2(-moveInput * m_speed, m_body2d.velocity.y);
                }
            }
            else if (moveInput < 0)            
            {
                if(facingDirection == FacingDirection.Right)
                {
                    m_body2d.velocity = new Vector2(-moveInput * m_speed, m_body2d.velocity.y);
                }
                else
                {
                    m_body2d.velocity = new Vector2(moveInput * m_speed, m_body2d.velocity.y);
                }
            }
        }

        void Jump()
        {

            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
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