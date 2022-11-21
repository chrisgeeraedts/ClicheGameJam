using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;

public class NPCScript : MonoBehaviour, INPC
{
    [SerializeField] float      m_speed = 4.0f;
    private Rigidbody2D rb;
    [SerializeField] private EasyExpandableTextBox Speaking_Textbox;

    public RuntimeAnimatorController Idle;
    public RuntimeAnimatorController Moving;
    private Animator animator;

    private bool _isActive;
    public void SetNPCActive(bool active)
    {
        _isActive = active;
        if(!IsNPCPaused())
        {
            animator.runtimeAnimatorController = Moving;  
        }
    }

        

    public bool IsNPCActive()
    {
        return _isActive;
    }

     private bool _isPaused;
    public void SetNPCPaused(bool Paused)
    {
        _isPaused = Paused;
        if(_isPaused)
        {
            SetIdle();
        }
        else
        {
            SetMoving();
        }
    }

    public bool IsNPCPaused()
    {
        return _isPaused;
    }
    
    void Start()
    {
        Speaking_Textbox.Hide();
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(IsNPCActive() && !IsNPCPaused())
        {
            rb.velocity = new Vector2(Vector2.right.x * m_speed, rb.velocity.y);
        }
        if(rb.velocity.magnitude > 0)
        {
            animator.runtimeAnimatorController = Moving; 
        }
        else
        {            
            animator.runtimeAnimatorController = Idle; 
        }
    }

    private bool isShowingSayPopup = false;
    public void Say(string text)
    {if(!isShowingSayPopup)
        {                
            isShowingSayPopup = true;
            Debug.Log("NPC SHOULD SAY: " + text);
            Speaking_Textbox.Show(gameObject, 3f);
            StartCoroutine(Speaking_Textbox.EasyMessage(text, 0.1f, false, false, 5f));
            StartCoroutine(HideSay(text, 0.1f, 5f));
        }
    }

    IEnumerator HideSay(string message, float duration, float timeToWaitAfterTextIsDisplayed )
    {
        yield return new WaitForSeconds((duration*message.Length)+timeToWaitAfterTextIsDisplayed); 
        isShowingSayPopup = false;
        Speaking_Textbox.Hide();
    }

    public void SetIdle()
    {        
        animator.runtimeAnimatorController = Idle;  
    }

     public void SetMoving()
    {        
        animator.runtimeAnimatorController = Moving;  
    }
}
