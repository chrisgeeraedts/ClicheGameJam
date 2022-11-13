using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;

public class NPCScript : MonoBehaviour, INPC
{
    [SerializeField] float      m_speed = 4.0f;
    private Rigidbody2D rb;

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
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(IsNPCActive() && !IsNPCPaused())
        {
            rb.velocity = new Vector2(Vector2.right.x * m_speed, rb.velocity.y);
        }
    }

    public GameObject chatBubblePrefab;
    
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

    public void SetIdle()
    {        
        animator.runtimeAnimatorController = Idle;  
    }

     public void SetMoving()
    {        
        animator.runtimeAnimatorController = Moving;  
    }
}
