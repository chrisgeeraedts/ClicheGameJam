using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Shared
{
    public class PlayerScript : MonoBehaviour, IPlayer, ISpeaker, IGetHealthSystem
    {    
        #region Options
        [Header("Player Options")]
        [SerializeField] private bool Options_ShowHealthBar = false;
        [SerializeField] private bool Options_CanFireGun = false;
        [SerializeField] private bool Options_CanFireHeavyGun = false;
        [SerializeField] private bool Options_CanAttackMelee = false;
        [SerializeField] private bool Options_CanAttackHeavyMelee = false;
        [SerializeField] private bool Options_CanJump = false;
        [SerializeField] private KeyCode InteractionKey = KeyCode.E;
        [Space(10)]
        #endregion

        #region Base
        [Header("Base Configuration")]
        [SerializeField] private Animator Base_Animator;
        [SerializeField] private Rigidbody2D Base_RigidBody2D;   
        [SerializeField] private HealthBarUI Base_HealthBarUI; 
        [Space(10)]
        #endregion

        #region Speaking
        [Header("Speaking Bubbles")]
        [SerializeField] private EasyExpandableTextBox Speaking_Textbox;
        [SerializeField] private float Speaking_TextVisibleDuration;
        [Space(10)]
        #endregion

        #region Death
        [Header("Death Configuration")]
        [SerializeField] private AudioSource AudioSource_Death;
        [SerializeField] RuntimeAnimatorController DeathController; 
        [Space(10)]
        #endregion

        #region Movement  
        [Header("Movement Configuration")]      
        [SerializeField] private bool Movement_FacingRight = true;
        [SerializeField] private bool _movementLocked;    
        [SerializeField] private PlayerMovementMode PlayerMovementMode;
        [SerializeField] private PlayerFacingDirection Movement_PlayerFacingDirection;
        [Space(10)]
        #endregion

        #region Walking
        [Header("Walking Configuration")]  
        [SerializeField] private float Movement_Speed = 10f;
        [SerializeField] private float Movement_JumpForce = 10f;  
        [SerializeField] private PlayerSensor Movement_GroundSensor;
        [SerializeField] private BoxCollider2D Movement_FeetCollider;
        [SerializeField] float Movement_LandGravity;
        [SerializeField] RuntimeAnimatorController LandController; 
        [Space(10)]
        #endregion

        #region Swimming    
        [Header("Swimming Configuration")]      
        [SerializeField] private float Swimming_Speed = 3f;
        [SerializeField] RuntimeAnimatorController WaterController;
        [SerializeField] float Swimming_WaterGravity;
        [SerializeField] GameObject Swimming_Bubbles_Prefab;
        [Space(10)]
        #endregion

        #region GunAttack 
        [Header("Gun Configuration")] 
        [SerializeField] private GameObject Attacking_Gun_BulletPrefab;
        [SerializeField] private GameObject Attacking_Gun_ShootStartPoint;
        [SerializeField] private AudioSource[] AudioSources_Attacking_GunFire;
        [SerializeField] private float Attacking_Gun_BulletSpeed = 10f;
        [SerializeField] private float Attacking_Sword_Cooldown = 0.5f;
        [SerializeField] private float Attacking_HeavySword_Cooldown = 1.0f;
        [SerializeField] private float Attacking_Gun_Cooldown = 0.5f;  
        [Space(10)]
        #endregion

        #region MeleeAttack   
        [Header("Sword Configuration")]
        [SerializeField] private AudioSource[] AudioSources_Attacking_MeleeAttack;
        [Space(10)]
        #endregion

        #region Equipment
        [Header("Equipment")] 
        [SerializeField] public PlayerEquipment PlayerEquipment;
        [Space(10)]
        #endregion

        #region Particles   
        [Header("Particles")]      
        [SerializeField] private ParticleSystem Particles_SwapDirectionDust;
        [SerializeField] private ParticleSystem Particles_LandingDust;
        [Space(10)]
        #endregion


              
        private bool Movement_Grounded = false;
        private bool isAttacking = false;
        private bool attackCompleted = true;


        private void healthSystem_OnDead(object sender, System.EventArgs e)
        {
            Die();
        }
        private void healthSystem_OnHealed(object sender, System.EventArgs e)
        {

        }
        private void healthSystem_OnDamaged(object sender, System.EventArgs e)
        {

        }
        private void healthSystem_OnHealthMaxChanged(object sender, System.EventArgs e)
        {

        }
        private void healthSystem_OnHealthChanged(object sender, System.EventArgs e)
        {

        }


        void Awake(){
            SetPlayerActive(true);
            Base_Animator = GetComponent<Animator>();
            Base_RigidBody2D = GetComponent<Rigidbody2D>();
            Movement_GroundSensor = transform.Find("GroundSensor").GetComponent<PlayerSensor>();        
            Movement_PlayerFacingDirection = PlayerFacingDirection.Right;
            StartSpawningWaterBubbles();
            Speaking_Textbox.Hide();
            _healthSystem = new HealthSystem(100);
            _healthSystem.OnDead += healthSystem_OnDead;
            _healthSystem.OnHealed += healthSystem_OnHealed;
            _healthSystem.OnDamaged += healthSystem_OnDamaged;
            _healthSystem.OnHealthMaxChanged += healthSystem_OnHealthMaxChanged;
            _healthSystem.OnHealthChanged += healthSystem_OnHealthChanged;
            PlayerMovementMode = PlayerMovementMode.Walking;
        }

        void Update ()
        {
            if(IsPlayerActive())
            {
                // Set config correct
                HandleSetup();
                if(PlayerMovementMode != PlayerMovementMode.Dead)
                {
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
                    HandleInteract();

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
            else if(PlayerMovementMode == PlayerMovementMode.Dead && Base_Animator.runtimeAnimatorController != DeathController)
            {
                SetDeadMode();
            }

            if(Options_ShowHealthBar && !Base_HealthBarUI.gameObject.activeSelf)
            {
                Base_HealthBarUI.gameObject.SetActive(true);
            }
            else if(!Options_ShowHealthBar && Base_HealthBarUI.gameObject.activeSelf)
            {
                Base_HealthBarUI.gameObject.SetActive(false);
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

        public void SetDeadMode()
        {
            Base_Animator.runtimeAnimatorController = DeathController; 
            Debug.Log("Activating Dead Mode");
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
                yield return new WaitForSeconds(UnityEngine.Random.Range(3, 6));
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

        private void HandleInteract()
        {
            if (Input.GetKeyDown(InteractionKey))
            {    
                if(currentInteractableEntity != null)
                {
                    currentInteractableEntity.Interact();
                }
            }
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
                            int randomAudioNumber = UnityEngine.Random.Range(0, AudioSources_Attacking_GunFire.Length);
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
                            int randomAudioNumber = UnityEngine.Random.Range(0, AudioSources_Attacking_MeleeAttack.Length); 
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
                            int randomAudioNumber = UnityEngine.Random.Range(0, AudioSources_Attacking_MeleeAttack.Length); 
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
                if(Movement_PlayerFacingDirection == PlayerFacingDirection.Right)
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
                Movement_PlayerFacingDirection = PlayerFacingDirection.Right;
            }
            else if (moveInput < 0)            
            {
                if(Movement_PlayerFacingDirection == PlayerFacingDirection.Right)
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
                Movement_PlayerFacingDirection = PlayerFacingDirection.Left;
            }
        }

        void MoveCharacter(float moveInput)
        {
            
            if (moveInput > 0)
            {
                if(Movement_PlayerFacingDirection == PlayerFacingDirection.Right)
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
                if(Movement_PlayerFacingDirection == PlayerFacingDirection.Right)
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

        

        public void Say(string message)
        {
            Speaking_Textbox.Show(gameObject);
            StartCoroutine(Speaking_Textbox.EasyMessage(message));
            StartCoroutine(HideSay(message));
        }

        IEnumerator HideSay(string message)
        {
            yield return new WaitForSeconds(Speaking_TextVisibleDuration + (0.125f*message.Length)); 
            Speaking_Textbox.Hide();
        }

        public void ShowTooltip(string gameEntityToUse, string useKey)
        {
            string message = string.Format("Press <color=#910000>[{0}]</color> to activate <color=#910000>[{1}]</color>", useKey, gameEntityToUse);
            Speaking_Textbox.Show(gameObject);
            StartCoroutine(Speaking_Textbox.EasyMessage(message, 0f, false, false, 100f));
        }

        public void HideTooltip()
        {
            Speaking_Textbox.Hide();
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

        public event EventHandler<PlayerDeathEventArgs> OnPlayerDeath;
        public void Die()
        {
            PlayerMovementMode = PlayerMovementMode.Dead;
            StopMovement();
            LockMovement();
            AudioSource_Death.Play();
            if(PlayerMovementMode != PlayerMovementMode.Swimming)
            {
                Debug.Log("Bool set");
                Base_Animator.SetBool(PlayerConstants.Animation_Dead, true);
            }
            Options_ShowHealthBar = false;
            OnPlayerDeath?.Invoke(this, new PlayerDeathEventArgs());
        }

        public void JumpOutOfWater()
        {
            Base_Animator.SetTrigger(PlayerConstants.Animation_Jump);
            Movement_Grounded = false;
            Base_Animator.SetBool(PlayerConstants.Animation_Grounded, Movement_Grounded);
            Base_RigidBody2D.velocity = new Vector2(Base_RigidBody2D.velocity.x, Movement_JumpForce);
            Movement_GroundSensor.Disable(0.2f);
        }


        private HealthSystem _healthSystem;
        public HealthSystem GetHealthSystem()
        {
            return _healthSystem;
        }



        IInteractable currentInteractableEntity;

        #region Collisions

        [SerializeField] private string WaterEnterTagName = "WaterEnter";
        [SerializeField] private string WaterExitTagName = "WaterExit";
        [SerializeField] private string KillZoneTagName = "KillZone";
        [SerializeField] private string InteractableTagName = "Interactable";

        void OnTriggerEnter2D(Collider2D other)
        {         
            Debug.Log("Collision with " + other.name);
            if(other.tag == WaterEnterTagName && PlayerMovementMode != PlayerMovementMode.Swimming && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                CreateSwimmingBubbles();               
                PlayerMovementMode = PlayerMovementMode.Swimming;
            }
            else if(other.tag == WaterExitTagName && PlayerMovementMode != PlayerMovementMode.Walking && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                if(PlayerMovementMode == PlayerMovementMode.Swimming)
                {
                    JumpOutOfWater();
                }                
                PlayerMovementMode = PlayerMovementMode.Walking;
            }
            else if(other.tag == KillZoneTagName && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                _healthSystem.Damage(99999);                
            }
            else if(other.tag == InteractableTagName && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                currentInteractableEntity = other.gameObject.GetComponent<IInteractable>();      
                ShowTooltip(currentInteractableEntity.GetObjectName(),InteractionKey.ToString());
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {         
            Debug.Log("Exited Collision with " + other.name);
           if(other.tag == InteractableTagName && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                currentInteractableEntity = null;
                HideTooltip();
            }
        }

        #endregion
    }


    public class PlayerDeathEventArgs : EventArgs
    {
        public PlayerDeathEventArgs(){}
    }
}