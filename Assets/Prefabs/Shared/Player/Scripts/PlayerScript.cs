using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;
using Assets.Scripts.FinalBossScene;
using Assets.Scripts.Map;

namespace Assets.Scripts.Shared
{
    public class PlayerScript : MonoBehaviour, IPlayer, ISpeaker, IGetHealthSystem
    {
        #region Options
        [Header("Player Options")]
        [SerializeField] public bool Options_ShowCharacterAvatar = false;
        [SerializeField] public bool Options_ShowHealthBar = false;
        [SerializeField] public bool Options_ShowDamageNumbers = false;
        [SerializeField] public bool Options_CanFireGun = false;
        [SerializeField] public bool Options_CanFireHeavyGun = false;
        [SerializeField] public bool Options_CanAttackMelee = false;
        [SerializeField] public bool Options_CanAttackHeavyMelee = false;
        [SerializeField] public bool Options_CanJump = false;
        [SerializeField] public bool Options_ShowTargetingArrow = false;
        [SerializeField] public bool Options_CanChopTrees = false;
        [SerializeField] public bool Options_CanCraftFishingpole = false;
        [SerializeField] public bool Options_HasFishingpole = false;
        [SerializeField] public bool Options_IsImmortal = false;
        [SerializeField] private KeyCode InteractionKey = KeyCode.E;
        [Space(10)]
        #endregion

        #region Base
        [Header("Base Configuration")]
        [SerializeField] private Animator Base_Animator;
        [SerializeField] private Rigidbody2D Base_RigidBody2D;
        
        [SerializeField] private Collider2D Base_WalkingCollider1;
        [SerializeField] private Collider2D Base_WalkingCollider2;
        [SerializeField] private Collider2D Base_SwimmingCollider;

        [SerializeField] private Light2D Base_PlayerLight;
        [Space(10)]
        #endregion

        #region TargetingArrow
        [Header("Targeting Arrow")]
        [SerializeField] private PlayerTargetArrowScript TargetingArrow_Arrow;
        [SerializeField] public GameObject TargetingArrow_Target;
        [SerializeField] private float TargetingArrow_MaximumDistanceToShow = 4f;
        [Space(10)]
        #endregion

        #region Speaking
        [Header("Speaking Bubbles")]
        [SerializeField] private EasyExpandableTextBox Speaking_Textbox;
        [SerializeField] private float Speaking_TextVisibleDuration;
        [SerializeField] private float TextBox_Y_Offset = 5f;
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
        [SerializeField] public PlayerMovementMode PlayerMovementMode;
        [SerializeField] private PlayerFacingDirection Movement_PlayerFacingDirection;
        [SerializeField] private AudioSource[] AudioSources_Jumping;
        [Space(10)]
        #endregion

        #region Walking
        [Header("Walking Configuration")]
        [SerializeField] private float Walking_Light_Intensity = 0.5f;
        [SerializeField] private float Movement_Speed = 10f;
        [SerializeField] private float Movement_JumpForce = 10f;
        [SerializeField] private PlayerSensor Movement_GroundSensor;
        [SerializeField] private BoxCollider2D Movement_FeetCollider;
        [SerializeField] float Movement_LandGravity;
        [SerializeField] RuntimeAnimatorController LandController;
        [SerializeField] RuntimeAnimatorController FishingPoleController;
        [Space(10)]
        #endregion

        #region Swimming    
        [Header("Swimming Configuration")]
        [SerializeField] private float Swimming_Light_Intensity = 2.5f;
        [SerializeField] private float Swimming_Speed = 3f;
        [SerializeField] RuntimeAnimatorController WaterController;
        [SerializeField] float Swimming_WaterGravity;
        [SerializeField] GameObject Swimming_Bubbles_Prefab;
        [SerializeField] AudioSource PlayerAudio_Dive;
        [SerializeField] public float Swimming_Max_Air;
        [SerializeField] public float Swimming_Current_Air;
        
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
        [SerializeField] private float AttackRange = 2f;
        [SerializeField] private float AttackCooldown = 1f;
        [SerializeField] private float AttackDamage = 10f;
        [SerializeField] private float AttackTimeUntillAttackHits = 0.5f;
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

        #region Health   
        [Header("Health")]
        [SerializeField] private HealthBarUI Base_HealthBarUI;
        [SerializeField] private float Health_MaximumHealth;
        [SerializeField] private AudioSource AudioSource_DamageTaken;
        [Space(10)]
        #endregion

        #region Avatar   
        [Header("Avatar")]
        [SerializeField] private PlayerHealthBar PlayerHealthBar;
        [Space(10)]
        #endregion

        #region Damaging
        [Header("Damaging")]
        [SerializeField] private GameObject PlayerDamageNumberPrefab;
        [SerializeField] private Canvas PlayerDamageNumberParent;
        [SerializeField] private GameObject PlayerDamageNumberSpawnLocation;
        [SerializeField] private float DamageImmunityTime = 0.5f;

        [Space(10)]
        #endregion

        private bool Movement_Grounded = false;
        private bool isAttacking = false;
        private bool attackCompleted = true;
        private bool isImmuneToDamage;
        private HealthSystem _healthSystem;
        private IInteractable currentInteractableEntity;
        private bool _isActive;

        #region Events
        public event EventHandler<PlayerInteractedEventArgs> OnPlayerInteracted;
        public event EventHandler<PlayerMilestoneHitEventArgs> OnPlayerMilestoneHit;
        public event EventHandler<PlayerDeathEventArgs> OnPlayerDeath;
        #endregion

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


        void Start()
        {
            SetPlayerActive(true);
            Base_Animator = GetComponent<Animator>();
            Base_RigidBody2D = GetComponent<Rigidbody2D>();
            Movement_GroundSensor = transform.Find("GroundSensor").GetComponent<PlayerSensor>();
            Movement_PlayerFacingDirection = PlayerFacingDirection.Right;
            StartSpawningWaterBubbles();
            Speaking_Textbox.Hide();
            Health_MaximumHealth = MapManager.GetInstance().HeroMaxHP;
            Health_CurrentHealth = MapManager.GetInstance().HeroHP;
            _healthSystem = new HealthSystem(Health_MaximumHealth);
            _healthSystem.Damage(Health_MaximumHealth - Health_CurrentHealth);
            _healthSystem.OnDead += healthSystem_OnDead;
            _healthSystem.OnHealed += healthSystem_OnHealed;
            _healthSystem.OnDamaged += healthSystem_OnDamaged;
            _healthSystem.OnHealthMaxChanged += healthSystem_OnHealthMaxChanged;
            _healthSystem.OnHealthChanged += healthSystem_OnHealthChanged;
            Base_HealthBarUI.SetHealthSystem(_healthSystem);
            PlayerMovementMode = PlayerMovementMode.Walking;
            ActiveDamagingZones = new List<IDamagingZone>();
            ActiveEnemiesInDamageArea = new List<IEnemy>();
            TargetingArrow_Arrow.Setup(this, TargetingArrow_Target, TargetingArrow_MaximumDistanceToShow);
            TargetingArrow_Arrow.Toggle(true);
            PlayerHealthBar.SetFill(Health_CurrentHealth / Health_MaximumHealth);
            PlayerHealthBar.SetProgressText(Health_CurrentHealth + "/" + Health_MaximumHealth);
        }

        void Update()
        {
            if (IsPlayerActive())
            {
                // Set config correct
                HandleSetup();
                if (PlayerMovementMode != PlayerMovementMode.Dead)
                {
                    if (PlayerMovementMode == PlayerMovementMode.Walking)
                    {
                        HandleMovementLand();
                    }
                    else if (PlayerMovementMode == PlayerMovementMode.Swimming)
                    {
                        HandleMovementWater();
                    }

                    FlipCharacter(Input.GetAxis("Horizontal"));
                    HandleJump();
                    HandleAttack();
                    HandleInteract();
                    HandleDamageFromDamagingZones();

                    //Check if character just landed on the ground
                    if (!Movement_Grounded && Movement_GroundSensor.State())
                    {
                        if (PlayerMovementMode == PlayerMovementMode.Walking)
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


        public void SetArrow(GameObject target)
        {
            TargetingArrow_Target = target;
            TargetingArrow_Arrow.Setup(this, TargetingArrow_Target, TargetingArrow_MaximumDistanceToShow);
            TargetingArrow_Arrow.Toggle(true);
        }

        private void HandleSetup()
        {
            SetGravity();
            Base_Animator.SetBool(PlayerConstants.Animation_GunEquiped, PlayerEquipment == PlayerEquipment.Gun);
            if (PlayerMovementMode == PlayerMovementMode.Swimming && Base_Animator.runtimeAnimatorController != WaterController)
            {
                SetSwimmingMode();
            }
            else if (PlayerMovementMode == PlayerMovementMode.Walking && Base_Animator.runtimeAnimatorController != LandController)
            {
                SetWalkingMode();
            }
            else if (PlayerMovementMode == PlayerMovementMode.Dead && Base_Animator.runtimeAnimatorController != DeathController)
            {
                SetDeadMode();
            }

            if (Options_ShowHealthBar && !Base_HealthBarUI.gameObject.activeSelf)
            {
                Base_HealthBarUI.gameObject.SetActive(true);
            }
            else if (!Options_ShowHealthBar && Base_HealthBarUI.gameObject.activeSelf)
            {
                Base_HealthBarUI.gameObject.SetActive(false);
            }

            if (Options_ShowTargetingArrow && !TargetingArrow_Arrow.IsToggled())
            {
                TargetingArrow_Arrow.Setup(this, TargetingArrow_Target, TargetingArrow_MaximumDistanceToShow);
                ToggleTargetingArrow(true);
            }
            else if (!Options_ShowTargetingArrow && TargetingArrow_Arrow.IsToggled())
            {
                ToggleTargetingArrow(false);
            }

            if (Options_ShowCharacterAvatar && !PlayerHealthBar.IsToggled())
            {
                PlayerHealthBar.Toggle(true);
            }
            else if (!Options_ShowCharacterAvatar && PlayerHealthBar.IsToggled())
            {
                PlayerHealthBar.Toggle(false);
            }




        }

        public void ToggleTargetingArrow(bool toggle)
        {
            TargetingArrow_Arrow.Toggle(toggle);
        }

        public void SetSwimmingMode()
        {
            if(Base_PlayerLight != null)
            {
                Base_PlayerLight.intensity = Swimming_Light_Intensity;
            } 
            Base_Animator.runtimeAnimatorController = WaterController;
            if(Base_WalkingCollider1 != null)
            {
                Base_WalkingCollider1.enabled = false;
            }
            if(Base_WalkingCollider2 != null)
            {
                Base_WalkingCollider2.enabled = false;
            }
            if(Base_SwimmingCollider != null)
            {
                Base_SwimmingCollider.enabled = true;
            }
        }

        public void SetWalkingMode()
        {           
            if(Base_PlayerLight != null)
            {
                Base_PlayerLight.intensity = Walking_Light_Intensity;
            } 
            if(Base_WalkingCollider1 != null)
            {
                Base_WalkingCollider1.enabled = true;
            }         
            if(Base_WalkingCollider2 != null)
            {
                Base_WalkingCollider2.enabled = true;
            }
            if(Base_SwimmingCollider != null)
            {
                Base_SwimmingCollider.enabled = false;
            }

            if(Options_HasFishingpole)
            {
                Base_Animator.runtimeAnimatorController = FishingPoleController;
            }
            else
            {                
                Base_Animator.runtimeAnimatorController = LandController;
            }
        }

        public void SetDeadMode()
        {
            Base_Animator.runtimeAnimatorController = DeathController;
        }

        public void StartSpawningWaterBubbles()
        {
            StartCoroutine(SpawnBubbles());
        }

        private IEnumerator SpawnBubbles()
        {
            while (true)
            {
                if (PlayerMovementMode == PlayerMovementMode.Swimming)
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
            Base_Animator.SetInteger(PlayerConstants.Animation_AnimState, 0);
            Base_Animator.SetBool(PlayerConstants.Animation_Grounded, true);
            Base_RigidBody2D.velocity = Vector3.zero;
        }

        public void LockMovement()
        {
            _movementLocked = true;
            Base_Animator.SetInteger(PlayerConstants.Animation_AnimState, 0);
            Base_Animator.SetBool(PlayerConstants.Animation_Grounded, true);
        }

        public void UnlockMovement()
        {
            _movementLocked = false;
            Base_Animator.SetInteger(PlayerConstants.Animation_AnimState, 0);
            Base_Animator.SetBool(PlayerConstants.Animation_Grounded, true);
        }

        public void ToggleGravity(bool toggle)
        {
            if (toggle)
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

        public void Kill()
        {
            isImmuneToDamage = false;
            Damage(99999);
        }

        private void HandleDamageFromDamagingZones()
        {

            if (ActiveDamagingZones != null && ActiveDamagingZones.Count > 0)
            {
                float damageToTake = 0;
                foreach (var activeDamagingZone in ActiveDamagingZones)
                {
                    if (activeDamagingZone != null)
                    {
                        damageToTake += activeDamagingZone.GetDamageOnHit();
                    }
                }

                if (damageToTake > 0)
                {
                    Damage(damageToTake);
                }
            }
        }

        private float Health_CurrentHealth;
        public void Damage(float amount)
        {
            if (!isImmuneToDamage && _isActive && !Options_IsImmortal)
            {
                if (amount > 0)
                {
                    if (MapManager.GetInstance().HasBininiArmor())
                    {
                        amount = amount * 0.8f;
                    }

                    AudioSource_DamageTaken.Play();
                    _healthSystem.Damage(amount);

                    var damageText = Instantiate(PlayerDamageNumberPrefab, PlayerDamageNumberSpawnLocation.transform, false);
                    damageText.transform.SetParent(PlayerDamageNumberParent.transform);
                    damageText.transform.localScale = new Vector3(1, 1, 1);
                    damageText.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

                    damageText.GetComponent<PlayerDamageNumberScript>().ShowText(amount);

                    Health_CurrentHealth = Health_CurrentHealth - amount;
                    if (Health_CurrentHealth < 0)
                        Health_CurrentHealth = 0;

                    PlayerHealthBar.SetFill(Health_CurrentHealth / Health_MaximumHealth);
                    PlayerHealthBar.SetProgressText(Health_CurrentHealth + "/" + Health_MaximumHealth);

                    Base_Animator.SetTrigger(PlayerConstants.Animation_TakeHit);

                    isImmuneToDamage = true;
                    StartCoroutine(RemoveDamageImmunity());
                }
            }
        }

        private IEnumerator RemoveDamageImmunity()
        {
            yield return new WaitForSeconds(DamageImmunityTime);
            isImmuneToDamage = false;
        }


        public void Heal(float amount)
        {
            _healthSystem.Heal(amount);
        }


        private void HandleInteract()
        {
            if (Input.GetKeyDown(InteractionKey))
            {
                if (currentInteractableEntity != null)
                {
                    currentInteractableEntity.Interact();
                    OnPlayerInteracted?.Invoke(this, new PlayerInteractedEventArgs(currentInteractableEntity));
                }
            }
        }

        public bool AttackOnCooldown;
        private void HandleAttack()
        {
            if (!_movementLocked)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonUp(0))
                {
                    if (!isAttacking && attackCompleted && !AttackOnCooldown)
                    {
                        if (PlayerMovementMode == PlayerMovementMode.Walking)
                        {
                            if (PlayerEquipment == PlayerEquipment.Gun && Options_CanFireGun)
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
                            else if (PlayerEquipment == PlayerEquipment.Sword && Options_CanAttackMelee)
                            {
                                isAttacking = true;
                                AttackOnCooldown = true;
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
                    if (!isAttacking && attackCompleted)
                    {
                        if (PlayerMovementMode == PlayerMovementMode.Walking)
                        {
                            if (PlayerEquipment == PlayerEquipment.Gun && Options_CanFireHeavyGun)
                            {
                                isAttacking = true;
                                //Maybe throw grenade?
                                StartCoroutine(AttackFinish(true));
                            }
                            else if (PlayerEquipment == PlayerEquipment.Sword && Options_CanAttackHeavyMelee)
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

        }

        IEnumerator AttackFinish(bool attackWasHeavy)
        {
            if (PlayerEquipment == PlayerEquipment.Gun)
            {
                yield return new WaitForSeconds(Attacking_Gun_Cooldown);
                UnlockMovement();
            }
            else if (PlayerEquipment == PlayerEquipment.Sword)
            {
                if (!attackWasHeavy)
                {
                    yield return new WaitForSeconds(AttackTimeUntillAttackHits);
                    if (ActiveEnemiesInDamageArea.Any())
                    {
                        for (int i = 0; i < ActiveEnemiesInDamageArea.Count; i++)
                        {
                            IEnemy ActiveEnemyInDamageArea = ActiveEnemiesInDamageArea[i];
                            ActiveEnemyInDamageArea.Damage(AttackDamage);
                        }
                    }
                    yield return new WaitForSeconds(Attacking_Sword_Cooldown);
                    AttackOnCooldown = false;
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





        private float KnockBackForceOnX;

        private void HandleJump()
        {
            if (!CanJump()) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (PlayerMovementMode == PlayerMovementMode.Walking)
                {
                    int randomAudioNumber = UnityEngine.Random.Range(0, AudioSources_Jumping.Length);
                    AudioSources_Jumping[randomAudioNumber].Play();
                    Base_Animator.SetTrigger(PlayerConstants.Animation_Jump);
                    Movement_Grounded = false;
                    Base_Animator.SetBool(PlayerConstants.Animation_Grounded, Movement_Grounded);
                    Base_RigidBody2D.velocity = new Vector2(Base_RigidBody2D.velocity.x + KnockBackForceOnX, Movement_JumpForce);
                    Movement_GroundSensor.Disable(0.2f);
                }
                else if(PlayerMovementMode == PlayerMovementMode.Swimming)
                {
                    Base_RigidBody2D.velocity = new Vector2(Base_RigidBody2D.velocity.x, Swimming_Speed);
                }
            }
        }

        private bool CanJump()
        {
            if (!Options_CanJump) return false;
            if (PlayerMovementMode == PlayerMovementMode.Swimming)
            {
                return true;
            }
            else if (PlayerMovementMode == PlayerMovementMode.Walking)
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
            if (_movementLocked) return;

            var horizontal = Input.GetAxisRaw("Horizontal");
            Base_RigidBody2D.velocity = new Vector2((horizontal * Movement_Speed) + KnockBackForceOnX, Base_RigidBody2D.velocity.y);

            if (Mathf.Abs(horizontal) > float.Epsilon)
            {
                Movement_FacingRight = horizontal > 0;
            }
        }

        private void HandleHorizontalMovement_Water()
        {
            if (_movementLocked) return;

            var horizontal = Input.GetAxisRaw("Horizontal");
            Base_RigidBody2D.velocity = new Vector2(horizontal * Swimming_Speed, Base_RigidBody2D.velocity.y);

            if (Mathf.Abs(horizontal) > float.Epsilon)
            {
                Movement_FacingRight = horizontal > 0;
            }
        }

        private void HandleVerticalMovement_Water()
        {
            if (_movementLocked) return;

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
            if (!_movementLocked)
            {
                if (moveInput > 0)
                {
                    if (Movement_PlayerFacingDirection == PlayerFacingDirection.Right)
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        if (PlayerMovementMode == PlayerMovementMode.Walking)
                        {
                            CreateDirectionDust();
                        }
                        else if (PlayerMovementMode == PlayerMovementMode.Swimming)
                        {
                            SpawnBubbles();
                        }
                    }
                    Movement_PlayerFacingDirection = PlayerFacingDirection.Right;
                }
                else if (moveInput < 0)
                {
                    if (Movement_PlayerFacingDirection == PlayerFacingDirection.Right)
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        if (PlayerMovementMode == PlayerMovementMode.Walking)
                        {
                            CreateDirectionDust();
                        }
                        else if (PlayerMovementMode == PlayerMovementMode.Swimming)
                        {
                            SpawnBubbles();
                        }
                    }
                    else
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                    Movement_PlayerFacingDirection = PlayerFacingDirection.Left;
                }
            }
        }

        void MoveCharacter(float moveInput)
        {

            if (moveInput > 0)
            {
                if (Movement_PlayerFacingDirection == PlayerFacingDirection.Right)
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
                if (Movement_PlayerFacingDirection == PlayerFacingDirection.Right)
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
            if (PlayerMovementMode == PlayerMovementMode.Walking)
            {
                Particles_SwapDirectionDust.Play();
            }
        }

        void CreateLandingDust()
        {
            if (PlayerMovementMode == PlayerMovementMode.Walking)
            {
                Particles_LandingDust.Play();
            }
        }


        private bool isShowingSayPopup = false;
        public void Say(string message, float timeBetweenCharacters = 0.125f, bool canSkipText = true, bool waitForButtonClick = true, float timeToWaitAfterTextIsDisplayed = 1f)
        {
            if (!isShowingSayPopup)
            {
                Debug.Log("Blocking more popups");
                isShowingSayPopup = true;
                Speaking_Textbox.Show(gameObject, TextBox_Y_Offset);
                StartCoroutine(Speaking_Textbox.EasyMessage(message, timeBetweenCharacters, canSkipText, waitForButtonClick, timeToWaitAfterTextIsDisplayed));
                StartCoroutine(HideSay(message, timeBetweenCharacters, timeToWaitAfterTextIsDisplayed));
            }
        }

        IEnumerator HideSay(string message, float duration, float timeToWaitAfterTextIsDisplayed)
        {
            yield return new WaitForSeconds((duration * message.Length) + timeToWaitAfterTextIsDisplayed);
            Debug.Log("Can show popups again");
            isShowingSayPopup = false;
            Speaking_Textbox.Hide();
        }

        public void ShowTooltip(string gameEntityToUse, string useKey)
        {
            string message = string.Format("Press <color=#910000>[{0}]</color> to activate <color=#910000>[{1}]</color>", useKey, gameEntityToUse);
            Speaking_Textbox.Show(gameObject, TextBox_Y_Offset);
            StartCoroutine(Speaking_Textbox.EasyMessage(message, 0f, false, false, 100f));
        }

        public void ShowTooltip(string tooltipText)
        {
            Speaking_Textbox.Show(gameObject, TextBox_Y_Offset);
            StartCoroutine(Speaking_Textbox.EasyMessage(tooltipText, 0.125f));
        }

        public void HideTooltip()
        {
            Speaking_Textbox.Hide();
        }

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

        public void Empower(float newDamage)
        {
            AttackDamage = newDamage;
            GetComponent<SpriteRenderer>().color = Color.blue;
            transform.localScale = transform.localScale * 1.1f;
            Movement_JumpForce = 24;
            JumpOutOfWater();
        }

        private void Die()
        {
            PlayerMovementMode = PlayerMovementMode.Dead;
            StopMovement();
            LockMovement();
            AudioSource_Death.Play();
            if (PlayerMovementMode != PlayerMovementMode.Swimming)
            {
                Base_Animator.SetBool(PlayerConstants.Animation_Dead, true);
            }
            Options_ShowHealthBar = false;
            OnPlayerDeath?.Invoke(this, new PlayerDeathEventArgs());
        }

        public void JumpOutOfWater(float power = 1)
        {
            LockMovement();

            Base_Animator.SetTrigger(PlayerConstants.Animation_Jump);
            int randomAudioNumber = UnityEngine.Random.Range(0, AudioSources_Jumping.Length);
            AudioSources_Jumping[randomAudioNumber].Play();
            Movement_Grounded = false;
            //Base_Animator.SetBool(PlayerConstants.Animation_Grounded, Movement_Grounded);
            Base_RigidBody2D.velocity = new Vector2(Base_RigidBody2D.velocity.x, Movement_JumpForce * power);
            Movement_GroundSensor.Disable(0.2f);
            Base_Animator.SetTrigger(PlayerConstants.Animation_Jump);

            UnlockMovement();            
        }

        public bool IsInWater()
        {
            return PlayerMovementMode == PlayerMovementMode.Swimming;
        }

        public void MinorJump()
        {   
            Base_Animator.SetTrigger(PlayerConstants.Animation_Jump);
            Movement_Grounded = false;
            Base_Animator.SetBool(PlayerConstants.Animation_Grounded, Movement_Grounded);
            Base_RigidBody2D.velocity = new Vector2(Base_RigidBody2D.velocity.x, Movement_JumpForce/3f);
            Movement_GroundSensor.Disable(0.2f);
        }

        public void KnockBack(bool pushRight)
        {
            KnockBackForceOnX = 0f;
            if (pushRight)
            {
                KnockBackForceOnX = 5f;
            }
            else
            {
                KnockBackForceOnX = -5f;
            }

            Base_Animator.SetTrigger(PlayerConstants.Animation_Jump);
            Movement_Grounded = false;
            Base_Animator.SetBool(PlayerConstants.Animation_Grounded, Movement_Grounded);
            Base_RigidBody2D.velocity = new Vector2(Base_RigidBody2D.velocity.x + KnockBackForceOnX, Base_RigidBody2D.velocity.y + Movement_JumpForce);
            Movement_GroundSensor.Disable(0.2f);
            StartCoroutine(ResetKnockback());
        }

        IEnumerator ResetKnockback()
        {
            yield return new WaitForSeconds(0.5f);
            KnockBackForceOnX = 0;
        }


        public HealthSystem GetHealthSystem()
        {
            return _healthSystem;
        }


        public bool EnemyInDamagingZone;
        List<IEnemy> ActiveEnemiesInDamageArea;

        #region Collisions

        [SerializeField] private string WaterEnterTagName = "WaterEnter";
        [SerializeField] private string WaterExitTagName = "WaterExit";
        [SerializeField] private string KillZoneTagName = "KillZone";
        [SerializeField] private string InteractableTagName = "Interactable";
        [SerializeField] private string DamagingZoneTagName = "DamagingZone";

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "LaserTarget")
            {
                Damage(10);
            }
            if (other.tag == "Enemy")
            {
                IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    if (!ActiveEnemiesInDamageArea.Any(x => x.GetEnemyKey() == enemy.GetEnemyKey()))
                    {
                        ActiveEnemiesInDamageArea.Add(enemy);
                    }
                }
            }
            else if (other.tag == WaterEnterTagName && PlayerMovementMode != PlayerMovementMode.Swimming && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                CreateSwimmingBubbles();
                PlayerMovementMode = PlayerMovementMode.Swimming;
                if(PlayerAudio_Dive != null)
                {
                    PlayerAudio_Dive.Play();
                }
            }
            else if (other.tag == WaterExitTagName && PlayerMovementMode != PlayerMovementMode.Walking && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                if (PlayerMovementMode == PlayerMovementMode.Swimming)
                {
                    PlayerMovementMode = PlayerMovementMode.Walking;
                    JumpOutOfWater();
                }
                
            }
            else if (other.tag == KillZoneTagName && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                Kill();
            }
            else if (other.tag == "MilestoneCollider" && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                var currentMilestoneEntity = other.gameObject.GetComponent<MilestoneColliderScript>();
                OnPlayerMilestoneHit?.Invoke(this, new PlayerMilestoneHitEventArgs(currentMilestoneEntity));
                Destroy(other);
            }
            else if (other.tag == InteractableTagName && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                currentInteractableEntity = other.gameObject.GetComponent<IInteractable>();
                if (currentInteractableEntity.CanShowInteractionDialog())
                {
                    ShowTooltip(currentInteractableEntity.GetObjectName(), InteractionKey.ToString());
                }
                if (currentInteractableEntity.CanInteract())
                {
                    currentInteractableEntity.ShowInteractibility();
                }
            }
            else if (other.tag == DamagingZoneTagName && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                IDamagingZone damagingZone = other.gameObject.GetComponent<IDamagingZone>();
                if (damagingZone != null)
                {
                    if (!ActiveDamagingZones.Any(x => x.GetZoneKey() == damagingZone.GetZoneKey()))
                    {
                        ActiveDamagingZones.Add(damagingZone);
                    }
                    Damage(damagingZone.GetDamageOnHit());
                }
            }
        }


        List<IDamagingZone> ActiveDamagingZones;

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == InteractableTagName && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                currentInteractableEntity = null;
                HideTooltip();
            }
            else if (other.tag == DamagingZoneTagName && PlayerMovementMode != PlayerMovementMode.Dead)
            {
                IDamagingZone damagingZone = other.gameObject.GetComponent<IDamagingZone>();
                ActiveDamagingZones.Remove(damagingZone);
            }
            else if (other.tag == "Enemy")
            {
                IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
                ActiveEnemiesInDamageArea.Remove(enemy);
            }
        }

        #endregion
    }


    public class PlayerDeathEventArgs : EventArgs
    {
        public PlayerDeathEventArgs() { }
    }

    public class PlayerInteractedEventArgs : EventArgs
    {
        public IInteractable InteractedWith;
        public PlayerInteractedEventArgs(IInteractable interactedWith)
        {
            InteractedWith = interactedWith;
        }
    }

    public class PlayerMilestoneHitEventArgs : EventArgs
    {
        public MilestoneColliderScript MilestoneCollider;
        public PlayerMilestoneHitEventArgs(MilestoneColliderScript milestoneCollider)
        {
            MilestoneCollider = milestoneCollider;
        }
    }
}

