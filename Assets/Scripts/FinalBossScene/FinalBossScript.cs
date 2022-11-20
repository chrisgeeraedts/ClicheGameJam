using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using System;
using Assets.Scripts.Shared;
using Assets.Scripts.Map;
using UnityEngine.EventSystems;

namespace Assets.Scripts.FinalBossScene 
{
    public interface IEnemy
    {
        string GetEnemyKey();
        void Damage(float amount);
        GameObject GetGameObject();
    }
    public class BossDeathEventArgs : EventArgs 
    {
        public BossDeathEventArgs(){}
    }

    public class FinalBossScript : MonoBehaviour, IEnemy
    {
        #region Base
        [Header("Base Configuration")]
        [SerializeField] private Animator Base_Animator;
        [SerializeField] private Rigidbody2D Base_RigidBody2D;   
        [SerializeField] private PlayerScript PlayerScript;   
        [Space(10)]
        #endregion

        #region Movement  
        [Header("Movement Configuration")]      
        [SerializeField] private bool Movement_FacingRight = true;
        [SerializeField] private bool _movementLocked;    
        [SerializeField] public PlayerMovementMode PlayerMovementMode;
        [Space(10)]
        #endregion

        #region Walking
        [Header("Walking Configuration")]  
        [SerializeField] private float Movement_Speed = 10f;
        #endregion

        #region Display and Animation
        [Header("Display and Animation")]  
        [SerializeField] private UnityEngine.Rendering.Universal.Light2D InitialSpellcastingLight;
        [SerializeField] private ParticleSystem InitialSpellcastingParticles;
        [SerializeField] private EasyExpandableTextBox Speaking_Textbox;
        [SerializeField] private float Speaking_TextVisibleDuration;
        #endregion

        #region Attacking
        [Header("Attacking")]  
        [SerializeField] private float AttackRange = 3f;
        [SerializeField] private float AttackCooldown = 1f;
        [SerializeField] private float AttackDamage = 10f;
        [SerializeField] private float AttackTimeUntillAttackHits = 0.5f;
        #endregion
        
        #region Death
        [Header("Death Configuration")]
        [SerializeField] private AudioSource AudioSource_Death;
        [Space(10)]
        #endregion

        #region Health   
        [Header("Health")]      
        [SerializeField] private GameObject BossDamageNumberPrefab;
        [SerializeField] private float Health_MaximumHealth = 100;
        [SerializeField] private AudioSource AudioSource_DamageTaken;
        
        [Space(10)]
        #endregion

        #region Avatar   
        [Header("Avatar")]      
        [SerializeField] private BossHealthBar BossHealthBar; 
        [Space(10)]
        #endregion

        private HealthSystem _healthSystem;

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

        string _enemyKey;
        public string GetEnemyKey()
        {
            return _enemyKey;   
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public bool IsActive = false;
        public void SetActive()
        {
            IsActive = true;
        }

        public bool PlayerInDamagingZone = false;
        public bool AttackOnCooldown = false;


        // Start is called before the first frame update
        void Start()
        {
            _enemyKey = System.Guid.NewGuid().ToString();
            Base_Animator = GetComponent<Animator>();
            Base_RigidBody2D = GetComponent<Rigidbody2D>();
            Base_Animator.SetTrigger("Spellcast");
            Speaking_Textbox.Hide();
            _healthSystem = new HealthSystem(MapManager.GetInstance().BossMaxHP);
            _healthSystem.OnDead += healthSystem_OnDead;
            _healthSystem.OnHealed += healthSystem_OnHealed;
            _healthSystem.OnDamaged += healthSystem_OnDamaged;
            _healthSystem.OnHealthMaxChanged += healthSystem_OnHealthMaxChanged;
            _healthSystem.OnHealthChanged += healthSystem_OnHealthChanged; 
            _healthSystem.Damage(MapManager.GetInstance().BossMaxHP - MapManager.GetInstance().BossHP);
            BossHealthBar.SetFill(MapManager.GetInstance().GetBossHPForFill());
        }


        private Vector3 targetPosition;
        // Update is called once per frame
        void Update()
        {   
            Debug.Log(Base_RigidBody2D.velocity.magnitude);
            if(IsActive)
            {
                FlipCharacter();
                MoveBoss();
            }
            
        }

        [Task]
        public void AimAt_Player()
        {            
            ThisTask.Succeed();
        }

        [Task]
        public void Attack_Player()
        {            
            InternalAttackPlayer();
            ThisTask.Succeed();
        }

        [Task]
        public bool IsPlayerVisible()
        {            
            if(!IsActive) return false;
            return true;
        }

        [Task]
        public bool IsPlayerInAttackRange()
        {            
            if(PlayerScript.PlayerMovementMode == PlayerMovementMode.Dead) return false;
            if(!IsActive) return false;
            
            return (Mathf.Abs(transform.position.x - PlayerScript.gameObject.transform.position.x)) < AttackRange;
        }

        [Task]
        public void SetDestination_Player()
        {            
            targetPosition = new Vector3(PlayerScript.gameObject.transform.position.x, transform.position.y, 0);
            ThisTask.Succeed();
        }

        [Task]
        public void MoveTo_Destination()
        {            
            ThisTask.Succeed();
        }

        [Task]
        public void DoNothing()
        {            
            // move       
            ThisTask.Succeed();
        }

        public void InternalAttackPlayer()
        {            
            if(!AttackOnCooldown)
            {
                AttackOnCooldown = true;
                Base_Animator.SetTrigger("Attack");
                StartCoroutine(CompleteAttack());                
            }
        }

        IEnumerator CompleteAttack() {
            
            yield return new WaitForSeconds(AttackTimeUntillAttackHits);
            if(PlayerInDamagingZone)
            {
                PlayerScript.Damage(AttackDamage);
                PlayerScript.KnockBack(transform.position.x < PlayerScript.gameObject.transform.position.x);
            }
            StartCoroutine(ReturnAttackCooldown());
        }

        IEnumerator ReturnAttackCooldown() {
            yield return new WaitForSeconds(AttackCooldown);
            AttackOnCooldown = false;
        }





        private float Health_CurrentHealth;
        private bool isImmuneToDamage = false;
        public void Damage(float amount) 
        {
            if(!isImmuneToDamage)
            {
                AudioSource_DamageTaken.Play();
                _healthSystem.Damage(amount);
                
                if(amount > 0)
                {
                    Health_CurrentHealth = Health_CurrentHealth - amount;
                    if(Health_CurrentHealth < 0)
                    Health_CurrentHealth = 0;

                    MapManager.GetInstance().BossHP = MapManager.GetInstance().BossHP - amount;

                    BossHealthBar.SetFill(MapManager.GetInstance().BossHP / MapManager.GetInstance().BossMaxHP);
                    BossHealthBar.SetProgressText(MapManager.GetInstance().BossHP+"/"+MapManager.GetInstance().BossMaxHP);

                    Base_Animator.SetTrigger(PlayerConstants.Animation_TakeHit);

                    isImmuneToDamage = true;
                    StartCoroutine(RemoveDamageImmunity());
                }
            }
        }

        private IEnumerator RemoveDamageImmunity() 
        {           
            yield return new WaitForSeconds(0.5f);            
            isImmuneToDamage = false;
        }



        public void ToggleSpellcastingVisuals(bool toggled)
        {
            InitialSpellcastingLight.enabled = toggled;
            InitialSpellcastingParticles.gameObject.SetActive(toggled);
        }

        public void Say(string message, float timeBetweenCharacters = 0.125f, bool canSkipText = true, bool waitForButtonClick = true, float timeToWaitAfterTextIsDisplayed = 1f)
        {
            Speaking_Textbox.Show(gameObject, 6.5f);
            StartCoroutine(Speaking_Textbox.EasyMessage(message));
            StartCoroutine(HideSay(message));
        }

        IEnumerator HideSay(string message)
        {
            yield return new WaitForSeconds(Speaking_TextVisibleDuration + (0.125f*message.Length)); 
            Speaking_Textbox.Hide();
        }


        void FlipCharacter()
        {
            if(transform.position.x < PlayerScript.gameObject.transform.position.x)
            {                
                Movement_FacingRight = true;                
                transform.localScale = new Vector3 (Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);         
            }
            else
            {
                Movement_FacingRight = false;
                transform.localScale = new Vector3 (-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); 
            }
        }

        void MoveBoss()
        {
            if(!_movementLocked)
            {
                Base_Animator.SetBool("Grounded", true);
                Base_Animator.SetInteger(PlayerConstants.Animation_AnimState, 1);
                var step =  Movement_Speed * Time.deltaTime; // calculate distance to move                
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            }
        }
      
        public event EventHandler<BossDeathEventArgs> OnBossDeath;
        
        private void Die()
        {
            IsActive = false;
            PlayerMovementMode = PlayerMovementMode.Dead;
            StopMovement();
            LockMovement();
            AudioSource_Death.Play();
            Base_Animator.SetTrigger("Death");
            OnBossDeath?.Invoke(this, new BossDeathEventArgs());
        }

        public void StopMovement()
        {
            Base_RigidBody2D.velocity = Vector3.zero;
        }
        
        public void LockMovement()
        {
           _movementLocked = true;
        }

        void OnTriggerEnter2D(Collider2D other)
        {         
            if(IsActive && other.tag == "Player")
            {
                PlayerInDamagingZone = true;
            }            
        }

        void OnTriggerExit2D(Collider2D other)
        {         
            if(IsActive && other.tag == "Player")
            {
                PlayerInDamagingZone = false;
            }   
        }
    }
}