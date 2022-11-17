using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Escort
{
    public enum FacingDirection
    {
        Left,
        Right
    }

    public class PlayerScript : MonoBehaviour, Assets.Scripts.Shared.IPlayer {

        [SerializeField] float      m_speed = 4.0f;
        [SerializeField] float      m_jumpForce = 7.5f;

        private Animator            m_animator;
        private Rigidbody2D         m_body2d;
        private Sensor_Player       m_groundSensor;
        private bool                m_grounded = false;
        private float               m_delayToIdle = 0.0f;

        public GameObject chatBubblePrefab;

        public AudioSource SpellCastFx;
        public AudioSource Attack1Fx;
        public AudioSource Attack2Fx;
        public AudioSource Attack3Fx;
        public AudioSource Attack4Fx;
        public AudioSource Attack5Fx;
        public AudioSource HurtFx;
        public AudioSource DeathFx;    
        public AudioSource JumpFx;
        public AudioSource LandFx;
        public AudioSource StepFx;
        
        public ParticleSystem SwapDirectionDust;
        public ParticleSystem LandingDust;
        private float SpellBaseIntensity;

        private FacingDirection facingDirection;

        // Use this for initialization
        void Start ()
        {
            m_animator = GetComponent<Animator>();
            m_body2d = GetComponent<Rigidbody2D>();
            m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Player>();        
            facingDirection = FacingDirection.Right;
        }


        // Update is called once per frame
        void Update ()
        {
            if(IsPlayerActive())
            {
                // Flip
                FlipCharacter(Input.GetAxis("Horizontal"));

                // Move
                MoveCharacter(Input.GetAxis("Horizontal"));

                //Check if character just landed on the ground
                if (!m_grounded && m_groundSensor.State())
                {
                    LandFx.Play(0);
                    CreateLandingDust();
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
                
                //Set AirSpeed in animator
                m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);


                // Jump
                if (Input.GetKeyDown("space") && m_grounded)
                {    
                    Jump();
                }

                // Attack
                if(Input.GetMouseButtonDown(0))
                {    
                    Attack();
                }
                
                //Idle
                else
                {            
                    StepFx.Stop();
                    // Prevents flickering transitions to idle
                    m_delayToIdle -= Time.deltaTime;
                        if(m_delayToIdle < 0)
                            m_animator.SetInteger("AnimState", 0);
                }

            }
        }
        private int chatBubbleLevel = 0;
        private GameObject activeBubble;
        public void Say(string text, float showDuration)
        {
            if(activeBubble != null)
            {
                Destroy(activeBubble);
            }
            // Create a new chatbubble
            GameObject chatBubble = Instantiate(chatBubblePrefab, new Vector3(gameObject.transform.position.x + 0.6f, gameObject.transform.position.y + 1.71f, 0), Quaternion.identity);
            chatBubble.GetComponent<ChatBubbleScript>().Say(text, chatBubbleLevel, gameObject, showDuration);
            activeBubble = chatBubble;
            chatBubbleLevel++;
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
                    CreateDirectionDust();
                }
                facingDirection = FacingDirection.Right;
            }
            else if (moveInput < 0)            
            {
                if(facingDirection == FacingDirection.Right)
                {
                    transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    CreateDirectionDust();
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
            CancelActions();

            JumpFx.Play(0);
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        void Attack()
        {
            DoAttack();
        }

        void Hurt()
        {
            HurtFx.Play(0);
            m_animator.SetTrigger("Hurt");
        }

        void Death()
        {
            DeathFx.Play(0);
            m_animator.SetTrigger("Death");
        }

        bool isAttacking = false;
        bool attackCompleted = true;
        void DoAttack()
        {
            if(!isAttacking && attackCompleted)
            {
                CancelActions();
                isAttacking = true;           
                PlayAttackAudio();
                m_animator.SetTrigger("Attack");
                StartCoroutine(AttackFinish());
            }
        }

        AudioSource targetAttackAudio;
        void PlayAttackAudio()
        {
            int randomNumber = Random.Range(1, 5);
            if(randomNumber == 1)            
                targetAttackAudio = Attack1Fx;
            if(randomNumber == 2)  
                targetAttackAudio = Attack2Fx;
            if(randomNumber == 3)  
                targetAttackAudio = Attack3Fx;
            if(randomNumber == 4)  
                targetAttackAudio = Attack4Fx;
            if(randomNumber == 5)  
                targetAttackAudio = Attack5Fx;
        }
        
        IEnumerator AttackFinish()
        {
            Debug.Log("ATTACK");
            yield return new WaitForSeconds(0.6f);
            targetAttackAudio.Play();        
            yield return new WaitForSeconds(0.5f);  
            isAttacking = false;
            attackCompleted = true; 
        }


        void CancelActions()
        {
            if(targetAttackAudio != null)
            {
                targetAttackAudio.Stop();
            }
            SpellCastFx.Stop();
        }
        
        void StopMovement()
        {
            m_body2d.velocity = Vector3.zero;
        }

        void CreateDirectionDust()
        {
            SwapDirectionDust.Play();
        }

        void CreateLandingDust()
        {
            LandingDust.Play();
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

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}