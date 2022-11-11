using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public enum FacingDirection
{
    Left,
    Right
}
namespace Assets.Scripts.Escort
{
    public class PlayerScript : MonoBehaviour, Assets.Scripts.Shared.IPlayer {

        [SerializeField] float      m_speed = 4.0f;
        [SerializeField] float      m_jumpForce = 7.5f;

        private Animator            m_animator;
        private Rigidbody2D         m_body2d;
        private Sensor_Player       m_groundSensor;
        private bool                m_grounded = false;
        private float               m_delayToIdle = 0.0f;

        public GameObject chatBubble;

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
        public Light2D SpellLeft;
        public Light2D WeaponLight;

        private FacingDirection facingDirection;

        // Use this for initialization
        void Start ()
        {
            chatBubble.SetActive(false);
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
                if (!isCastingSpell && spellCompleted)
                {
                    MoveCharacter(Input.GetAxis("Horizontal"));
                }

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

                // Spell
                if(Input.GetMouseButtonDown(1))
                {    
                    Spell();
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

        public void Say(string text)
        {
            chatBubble.SetActive(true);
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

        void Spell()
        {
            CancelActions();
            CastSpell();
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

        bool isCastingSpell = false;
        bool spellCompleted = true;
        void CastSpell()
        {
            if(!isCastingSpell && spellCompleted)
            {
                CancelActions();
                StopMovement();

                isCastingSpell = true;
                m_animator.SetTrigger("Spellcast");
                StartCoroutine(SpellFinish());
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

        IEnumerator SpellFinish()
        {      
            Debug.Log("CASTING SPELL");
            SpellCastFx.Play();
            yield return new WaitForSeconds(1.3f);   
            isCastingSpell = false;
            spellCompleted = true; 
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
    }
}