using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using Assets.Scripts.Shared;

namespace Assets.Scripts.Shared
{
    public enum FacingDirection
    {
        Left,
        Right
    }

    public static class PlayerConstants
    {
        public static string Animation_Attack = "Attack";
        public static string Animation_HeavyAttack = "HeavyAttack";
        public static string Animation_GunEquiped = "EquippedGun";
        public static string Animation_Grounded = "Grounded";
        public static string Animation_Jump = "Jump";
        public static string Animation_AnimState = "AnimState";
    }

    public enum PlayerEquipment
    {
        Sword,
        Gun
    }

    public enum PlayerMovementMode
    {
        Walking,
        Swimming
    }

    public class PlayerScript : MonoBehaviour, Assets.Scripts.Shared.IPlayer 
    {    
        [SerializeField] private bool Options_CanFireGun = false;
        [SerializeField] private bool Options_CanFireHeavyGun = false;
        [SerializeField] private bool Options_CanAttackMelee = false;
        [SerializeField] private bool Options_CanAttackHeavyMelee = false;
        [SerializeField] private bool Options_CanJump = false;

        [SerializeField] private Animator Base_Animator;
        [SerializeField] private Rigidbody2D Base_RigidBody2D;      

        #region Movement        
        private bool Movement_FacingRight = true;
        private bool _movementLocked;    
        private PlayerMovementMode PlayerMovementMode;
        #endregion

        #region Walking
        [SerializeField] private float Movement_Speed = 10f;
        [SerializeField] private float Movement_JumpForce = 10f;  
        [SerializeField] private PlayerSensor Movement_GroundSensor;
        [SerializeField] private BoxCollider2D Movement_FeetCollider;
        [SerializeField] float Movement_LandGravity;
        private bool Movement_Grounded = false;
        private float Movement_DelayToIdle = 0.0f;
        private FacingDirection Movement_FacingDirection;
        [SerializeField] RuntimeAnimatorController LandController; 
        #endregion

        #region Swimming        
        [SerializeField] private float Swimming_Speed = 3f;
        [SerializeField] RuntimeAnimatorController WaterController;
        [SerializeField] float Swimming_WaterGravity;
        [SerializeField] GameObject Swimming_Bubbles_Prefab;
        #endregion

        #region GunAttack 
        [SerializeField] private GameObject Attacking_Gun_BulletPrefab;
        [SerializeField] private GameObject Attacking_Gun_ShootStartPoint;
        [SerializeField] private AudioSource[] AudioSources_Attacking_GunFire;
        [SerializeField] private float Attacking_Gun_BulletSpeed = 10f;
        [SerializeField] private float Attacking_Sword_Cooldown = 0.5f;
        [SerializeField] private float Attacking_HeavySword_Cooldown = 1.0f;
        [SerializeField] private float Attacking_Gun_Cooldown = 0.5f;        
        private bool isAttacking = false;
        private bool attackCompleted = true;
        #endregion

        #region MeleeAttack   
        [SerializeField] private AudioSource[] AudioSources_Attacking_MeleeAttack;
        #endregion

        #region Equipment
        [SerializeField] public PlayerEquipment PlayerEquipment;
        #endregion

        #region Particles        
        [SerializeField] private ParticleSystem Particles_SwapDirectionDust;
        [SerializeField] private ParticleSystem Particles_LandingDust;
        #endregion


        void Awake(){
            SetPlayerActive(true);
            Base_Animator = GetComponent<Animator>();
            Base_RigidBody2D = GetComponent<Rigidbody2D>();
            Movement_GroundSensor = transform.Find("GroundSensor").GetComponent<PlayerSensor>();        
            Movement_FacingDirection = FacingDirection.Right;
            StartSpawningWaterBubbles();
        }

        void Update ()
        {
            if(IsPlayerActive())
            {
                // Set config correct
                HandleSetup();

                
                if(PlayerMovementMode == PlayerMovementMode.Walking)
                {
                    HandleMovementLand();
                }
                else if(PlayerMovementMode == PlayerMovementMode.Swimming)
                {                    
                    HandleMovementWater();
                }
                
                FlipCharacter(Input.GetAxis("Horizontal"));
                
                HandleJump();
                HandleAttack();

                //Check if character just landed on the ground
                if (!Movement_Grounded && Movement_GroundSensor.State())
                {
                    if(PlayerMovementMode == PlayerMovementMode.Walking)
                    {
                        CreateLandingDust();
                    }
                    
                    Movement_Grounded = true;
                    Base_Animator.SetBool(PlayerConstants.Animation_Grounded, Movement_Grounded);
                }

                //Check if character just started falling
                if (Movement_Grounded && !Movement_GroundSensor.State())
                {
                    Movement_Grounded = false;
                    Base_Animator.SetBool(PlayerConstants.Animation_Grounded, Movement_Grounded);
                } 


                if (Base_RigidBody2D.velocity.magnitude > 0)
                {
                    Base_Animator.SetInteger(PlayerConstants.Animation_AnimState, 1);
                }
                else
                {
                    Base_Animator.SetInteger(PlayerConstants.Animation_AnimState, 0);
                }
            }
        }

        private void HandleSetup()
        {
            Base_Animator.SetBool(PlayerConstants.Animation_GunEquiped, PlayerEquipment == PlayerEquipment.Gun);
            if(PlayerMovementMode == PlayerMovementMode.Swimming && Base_Animator.runtimeAnimatorController != WaterController)
            {
                SetSwimmingMode();
            }
            else if(PlayerMovementMode == PlayerMovementMode.Walking && Base_Animator.runtimeAnimatorController != LandController)
            {
                SetWalkingMode();
            }
        }
       
        public void SetSwimmingMode()
        {
            Base_Animator.runtimeAnimatorController = WaterController; 
            Debug.Log("Activating Water Mode");
        }

        public void SetWalkingMode()
        {
            Base_Animator.runtimeAnimatorController = LandController; 
            Debug.Log("Activating Land Mode");
        }

        

        public void StartSpawningWaterBubbles()
        {
            StartCoroutine(SpawnBubbles());
        }

        private IEnumerator SpawnBubbles() {
            while(true) {
                if(PlayerMovementMode == PlayerMovementMode.Swimming)
                {
                    CreateSwimmingBubbles();
                }
                yield return new WaitForSeconds(Random.Range(3, 6));
            }
        }


        public void CreateSwimmingBubbles()
        {
            Instantiate(Swimming_Bubbles_Prefab, gameObject.transform.position, gameObject.transform.rotation);
        }

        public void StopMovement()
        {
            Base_RigidBody2D.velocity = Vector3.zero;
        }
        
        public void LockMovement()
        {
           _movementLocked = true;
        }

        public void UnlockMovement()
        {
           _movementLocked = false;
                Debug.Log("UNLOCKED");
        }

        public void ToggleGravity(bool toggle)
        {
            if(toggle)
            {
                Base_RigidBody2D.gravityScale = 1f;
            }
            else
            {
                Base_RigidBody2D.gravityScale = 0f;
            }
            
        }

        public void Reposition(Vector3 newPos)
        {
            gameObject.transform.position = newPos;
        }

        private void HandleAttack()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonUp(0))
            {      
                if(!isAttacking && attackCompleted)
                {
                    if(PlayerMovementMode == PlayerMovementMode.Walking)
                    {
                        if(PlayerEquipment == PlayerEquipment.Gun && Options_CanFireGun)
                        {
                            isAttacking = true;  
                            Base_Animator.SetTrigger(PlayerConstants.Animation_Attack);
                            var bulletInstance = Instantiate(
                                Attacking_Gun_BulletPrefab, 
                                Attacking_Gun_ShootStartPoint.transform.position, 
                                Quaternion.identity);

                            // select random audio source
                            int randomAudioNumber = Random.Range(0, AudioSources_Attacking_GunFire.Length);
                            AudioSources_Attacking_GunFire[randomAudioNumber].Play();
                            var bulletRigidBody = bulletInstance.GetComponent<Rigidbody2D>();
                            var horizontal = Movement_FacingRight ? Attacking_Gun_BulletSpeed : -Attacking_Gun_BulletSpeed;
                            bulletRigidBody.velocity = new Vector2(horizontal, 0);
                            StartCoroutine(AttackFinish(false));
                        }
                        else if(PlayerEquipment == PlayerEquipment.Sword && Options_CanAttackMelee)
                        {                        
                            isAttacking = true;  
                            LockMovement();
                            Base_Animator.SetTrigger(PlayerConstants.Animation_Attack);
                            // select random audio source
                            int randomAudioNumber = Random.Range(0, AudioSources_Attacking_MeleeAttack.Length); 
                            AudioSources_Attacking_MeleeAttack[randomAudioNumber].Play();
                            StartCoroutine(AttackFinish(false));
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonUp(1))
            {      
                if(!isAttacking && attackCompleted)
                {
                    if(PlayerMovementMode == PlayerMovementMode.Walking)
                    {
                        if(PlayerEquipment == PlayerEquipment.Gun && Options_CanFireHeavyGun)
                        {
                            isAttacking = true;  
                            //Maybe throw grenade?
                            StartCoroutine(AttackFinish(true));
                        }
                        else if(PlayerEquipment == PlayerEquipment.Sword && Options_CanAttackHeavyMelee)
                        {                        
                            isAttacking = true;  
                            LockMovement();
                            Base_Animator.SetTrigger(PlayerConstants.Animation_HeavyAttack);
                            // select random audio source
                            int randomAudioNumber = Random.Range(0, AudioSources_Attacking_MeleeAttack.Length); 
                            AudioSources_Attacking_MeleeAttack[randomAudioNumber].Play();
                            StartCoroutine(AttackFinish(true));
                        }
                    }
                }
            }
        }

        IEnumerator AttackFinish(bool attackWasHeavy)
        {
            if(PlayerEquipment == PlayerEquipment.Gun)
            {
                yield return new WaitForSeconds(Attacking_Gun_Cooldown);  
                UnlockMovement();
            }
            else if(PlayerEquipment == PlayerEquipment.Sword)
            {
                if(!attackWasHeavy)
                {
                    yield return new WaitForSeconds(Attacking_Sword_Cooldown);  
                    UnlockMovement();
                }
                else
                {                    
                    yield return new WaitForSeconds(Attacking_HeavySword_Cooldown);  
                    UnlockMovement();
                }
            }
            isAttacking = false;
            attackCompleted = true; 
        }

        private void HandleJump()
        {
            if (!CanJump()) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(PlayerMovementMode == PlayerMovementMode.Walking)
                {                
                    Base_Animator.SetTrigger(PlayerConstants.Animation_Jump);
                    Movement_Grounded = false;
                    Base_Animator.SetBool(PlayerConstants.Animation_Grounded, Movement_Grounded);
                    Base_RigidBody2D.velocity = new Vector2(Base_RigidBody2D.velocity.x, Movement_JumpForce);
                    Movement_GroundSensor.Disable(0.2f);
                }
            }
        }

        private bool CanJump()
        {
            if(!Options_CanJump) return false;
            if(PlayerMovementMode == PlayerMovementMode.Swimming)
            {
                return true;
            }
            else if(PlayerMovementMode == PlayerMovementMode.Walking)
            {                
                return Movement_Grounded;
            }
            return false;
        }

        private void HandleMovementLand()
        {
            HandleHorizontalMovement_Land();            
        }

        private void HandleMovementWater()
        {
            HandleHorizontalMovement_Water();
                HandleVerticalMovement_Water();
        }

        private void HandleHorizontalMovement_Land()
        {
            if(_movementLocked) return;

            var horizontal = Input.GetAxisRaw("Horizontal");
            Base_RigidBody2D.velocity = new Vector2(horizontal * Movement_Speed, Base_RigidBody2D.velocity.y);

            if (Mathf.Abs(horizontal) > float.Epsilon)
            {
                Movement_FacingRight = horizontal > 0;
            }
        }

        private void HandleHorizontalMovement_Water()
        {
            if(_movementLocked) return;

            var horizontal = Input.GetAxisRaw("Horizontal");
            Base_RigidBody2D.velocity = new Vector2(horizontal * Swimming_Speed, Base_RigidBody2D.velocity.y);

            if (Mathf.Abs(horizontal) > float.Epsilon)
            {
                Movement_FacingRight = horizontal > 0;
            }
        }

        private void HandleVerticalMovement_Water()
        {
            if(_movementLocked) return;

            var vertical = Input.GetAxisRaw("Vertical");
            Base_RigidBody2D.velocity = new Vector2(Base_RigidBody2D.velocity.x, vertical * Swimming_Speed);
        }

        private void SetGravity()
        {
            if (PlayerMovementMode == PlayerMovementMode.Walking)
            {
                Base_RigidBody2D.gravityScale = Movement_LandGravity;
            }
            else
            {
                Base_RigidBody2D.gravityScale = Swimming_WaterGravity;
            }
        }

        void FlipCharacter(float moveInput)
        {
            if (moveInput > 0)
            {
                if(Movement_FacingDirection == FacingDirection.Right)
                {
                    transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);                    
                    if(PlayerMovementMode == PlayerMovementMode.Walking)
                    {
                        CreateDirectionDust();
                    }                    
                    else if(PlayerMovementMode == PlayerMovementMode.Swimming)
                    {
                        SpawnBubbles();
                    }
                }
                Movement_FacingDirection = FacingDirection.Right;
            }
            else if (moveInput < 0)            
            {
                if(Movement_FacingDirection == FacingDirection.Right)
                {
                    transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    if(PlayerMovementMode == PlayerMovementMode.Walking)
                    {
                        CreateDirectionDust();
                    }                    
                    else if(PlayerMovementMode == PlayerMovementMode.Swimming)
                    {
                        SpawnBubbles();
                    }
                }
                else
                {
                    transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                Movement_FacingDirection = FacingDirection.Left;
            }
        }

        void MoveCharacter(float moveInput)
        {
            
            if (moveInput > 0)
            {
                if(Movement_FacingDirection == FacingDirection.Right)
                {
                    Base_RigidBody2D.velocity = new Vector2(moveInput * Movement_Speed, Base_RigidBody2D.velocity.y);
                }
                else
                {
                    Base_RigidBody2D.velocity = new Vector2(-moveInput * Movement_Speed, Base_RigidBody2D.velocity.y);
                }
            }
            else if (moveInput < 0)            
            {
                if(Movement_FacingDirection == FacingDirection.Right)
                {
                    Base_RigidBody2D.velocity = new Vector2(-moveInput * Movement_Speed, Base_RigidBody2D.velocity.y);
                }
                else
                {
                    Base_RigidBody2D.velocity = new Vector2(moveInput * Movement_Speed, Base_RigidBody2D.velocity.y);
                }
            }
        }

        void CreateDirectionDust()
        {
            if(PlayerMovementMode == PlayerMovementMode.Walking)
            {
                Particles_SwapDirectionDust.Play();
            }
        }

        void CreateLandingDust()
        {
            if(PlayerMovementMode == PlayerMovementMode.Walking)
            {
                Particles_LandingDust.Play();
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {         
            if(other.tag == "WaterEnter" && PlayerMovementMode != PlayerMovementMode.Swimming)
            {
                PlayerMovementMode = PlayerMovementMode.Swimming;
                CreateSwimmingBubbles();
                Debug.Log("Entered Water");
            }
            else if(other.tag == "WaterExit" && PlayerMovementMode != PlayerMovementMode.Walking)
            {
                if(PlayerMovementMode == PlayerMovementMode.Swimming)
                {
                    Base_Animator.SetTrigger(PlayerConstants.Animation_Jump);
                    Movement_Grounded = false;
                    Base_Animator.SetBool(PlayerConstants.Animation_Grounded, Movement_Grounded);
                    Base_RigidBody2D.velocity = new Vector2(Base_RigidBody2D.velocity.x, Movement_JumpForce);
                    Movement_GroundSensor.Disable(0.2f);
                }
                
                PlayerMovementMode = PlayerMovementMode.Walking;
                Debug.Log("Exited Water");
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