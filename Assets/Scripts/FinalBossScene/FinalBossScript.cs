using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FinalBossScene 
{
    public class FinalBossScript : MonoBehaviour
    {
        #region Base
        [Header("Base Configuration")]
        [SerializeField] private Animator Base_Animator;
        [SerializeField] private Rigidbody2D Base_RigidBody2D;   
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
        [SerializeField] private float Movement_JumpForce = 10f;  
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            Base_Animator = GetComponent<Animator>();
            Base_RigidBody2D = GetComponent<Rigidbody2D>();
            Base_Animator.SetTrigger("Spellcast");
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}