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
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private GameObject ShootStartPoint;
        


        private Animator            m_animator;
        private Rigidbody2D         m_body2d;
        private Sensor_Player       m_groundSensor;
        private bool                m_grounded = false;
        private float               m_delayToIdle = 0.0f;
        private bool facingRight = true;
        private CapsuleCollider2D feetCollider;

 
        private float SpellBaseIntensity;

        private FacingDirection facingDirection;

        // Use this for initialization
        void Start ()
        {
            m_animator = GetComponent<Animator>();
            m_body2d = GetComponent<Rigidbody2D>();
            m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Player>();        
            facingDirection = FacingDirection.Right;
            feetCollider = GetComponent<CapsuleCollider2D>();
        }

        // Update is called once per frame
        void Update ()
        {
            if(IsPlayerActive())
            {
                HandleHorizontalMovement();
                FlipCharacter(Input.GetAxis("Horizontal"));
                HandleJump();
                HandleShoot();





               //Check if character just landed on the ground
                if (!m_grounded && m_groundSensor.State())
                {
                    //LandFx.Play(0);
                    //CreateLandingDust();
                    m_grounded = true;
                    m_animator.SetBool("Grounded", m_grounded);
                }

                //Check if character just started falling
                if (m_grounded && !m_groundSensor.State())
                {
                    m_grounded = false;
                    m_animator.SetBool("Grounded", m_grounded);
                } 

                if(m_body2d.velocity.magnitude > 0)
                {
                    m_delayToIdle = 0.05f;
                    m_animator.SetInteger("AnimState", 1);
                }
                else{
                    m_animator.SetInteger("AnimState", 0);
                }

                // Jump         
                if (Input.GetKeyDown("space") && m_grounded)
                {    
                    HandleJump();
                }//Idle
                else
                {            
                    //StepFx.Stop();
                    // Prevents flickering transitions to idle
                    m_delayToIdle -= Time.deltaTime;
                        if(m_delayToIdle < 0)
                            m_animator.SetInteger("AnimState", 0);
                }


               //if(!Input.anyKey && !m_grounded && m_groundSensor.State()){
               //    StopMovement();                
               //}

               //// Flip
               //FlipCharacter(Input.GetAxis("Horizontal"));

               //// Move
               //MoveCharacter(Input.GetAxis("Horizontal"));

               ////Check if character just landed on the ground
               //if (!m_grounded && m_groundSensor.State())
               //{
               //    m_grounded = true;
               //    m_animator.SetBool("Grounded", m_grounded);
               //}

               ////Check if character just started falling
               //if (m_grounded && !m_groundSensor.State())
               //{
               //    m_grounded = false;
               //    m_animator.SetBool("Grounded", m_grounded);
               //} 

               //if(m_body2d.velocity.magnitude > 0)
               //{
               //    m_delayToIdle = 0.05f;
               //    m_animator.SetInteger("AnimState", 1);
               //}
               //else{
               //    m_animator.SetInteger("AnimState", 0);
               //}
               //
               ////Set AirSpeed in animator
               //m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);


               //// Jump
               //if (Input.GetKeyDown("space") && m_grounded)
               //{    
               //    Jump();
               //}

               //else
               //{      
               //    // Prevents flickering transitions to idle
               //    m_delayToIdle -= Time.deltaTime;
               //        if(m_delayToIdle < 0)
               //            m_animator.SetInteger("AnimState", 0);
               //}

            }
        }
       
        private void HandleShoot()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonUp(0))
            {
                //var xOffset = facingRight ? bulletOffset : -bulletOffset;
                var bulletSpawnPoint = ShootStartPoint.transform.position;
                var instance = Instantiate(bulletPrefab, bulletSpawnPoint, Quaternion.identity);
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
            return feetCollider.IsTouchingLayers(LayerMask.GetMask(Constants.LayerNames.Ground));
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
//
        //void MoveCharacter(float moveInput)
        //{
        //    
        //    if (moveInput > 0)
        //    {
        //        if(facingDirection == FacingDirection.Right)
        //        {
        //            m_body2d.velocity = new Vector2(moveInput * m_speed, m_body2d.velocity.y);
        //        }
        //        else
        //        {
        //            m_body2d.velocity = new Vector2(-moveInput * m_speed, m_body2d.velocity.y);
        //        }
        //    }
        //    else if (moveInput < 0)            
        //    {
        //        if(facingDirection == FacingDirection.Right)
        //        {
        //            m_body2d.velocity = new Vector2(-moveInput * m_speed, m_body2d.velocity.y);
        //        }
        //        else
        //        {
        //            m_body2d.velocity = new Vector2(moveInput * m_speed, m_body2d.velocity.y);
        //        }
        //    }
        //}
//
        //void Jump()
        //{
        //    m_animator.SetTrigger("Jump");
        //    m_grounded = false;
        //    m_animator.SetBool("Grounded", m_grounded);
        //    m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        //    m_groundSensor.Disable(0.2f);
        //}
//
        //void Attack()
        //{
        //    
        //}
//
        //void StopMovement()
        //{
        //    m_body2d.velocity = Vector3.zero;
        //}

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