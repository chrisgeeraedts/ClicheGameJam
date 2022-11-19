using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using Assets.Scripts.Shared;

namespace Assets.Scripts.FinalBossScene 
{
    public class FinalBossScript : MonoBehaviour
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

        public bool IsActive = false;
        public void SetActive()
        {
            IsActive = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            Base_Animator = GetComponent<Animator>();
            Base_RigidBody2D = GetComponent<Rigidbody2D>();
            Base_Animator.SetTrigger("Spellcast");
            Speaking_Textbox.Hide();

            IsActive = true;
            
        }


        private Vector3 targetPosition;
        // Update is called once per frame
        void Update()
        {
            if(IsActive)
            {
                FlipCharacter();

                //var step =  Movement_Speed * Time.deltaTime; // calculate distance to move
                //transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
//
                //if (Base_RigidBody2D.velocity.magnitude > 0)
                //{
                //    Base_Animator.SetInteger("AnimState", 1);
                //}
                //else
                //{
                //    Base_Animator.SetInteger("AnimState", 0);
                //}
            }
            
        }

        [Task]
        public void AimAt_Player()
        {            
            //Debug.Log("Aiming at player");
            ThisTask.Succeed();
        }

        [Task]
        public void Attack_Player()
        {            
            //Debug.Log("Attacking player");
            Base_Animator.SetTrigger("Attack");
            ThisTask.Succeed();
        }

        [Task]
        public bool IsPlayerVisible()
        {            
            if(!IsActive) return false;

            //Debug.Log("Checking if player is visible");
            return true;
        }

        [Task]
        public bool IsPlayerInAttackRange()
        {            
            //Debug.Log("Checking if player is in attack range");
            return false;
        }

        [Task]
        public void SetDestination_Player()
        {            
            //Debug.Log("Setting last know destination of player");
            targetPosition = new Vector3(PlayerScript.gameObject.transform.position.x, transform.position.y, 0);
            ThisTask.Succeed();
        }

        [Task]
        public void MoveTo_Destination()
        {            
            //Debug.Log("Moving to last know destination of player");
            ThisTask.Succeed();
        }

        [Task]
        public void DoNothing()
        {            
            // move       
            ThisTask.Succeed();
        }



        public void ToggleSpellcastingVisuals(bool toggled)
        {
            InitialSpellcastingLight.enabled = toggled;
            InitialSpellcastingParticles.gameObject.SetActive(toggled);
        }

        public void Say(string message, float timeBetweenCharacters = 0.125f, bool canSkipText = true, bool waitForButtonClick = true, float timeToWaitAfterTextIsDisplayed = 1f)
        {
            Speaking_Textbox.Show(gameObject, 5f);
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
                transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);         
            }
            else
            {
                Movement_FacingRight = false;
                transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z); 
            }
        }
    }
}