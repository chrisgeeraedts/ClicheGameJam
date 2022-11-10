using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHeroScript : MonoBehaviour
{
    public int speed;
    private Animator animator;
    public RuntimeAnimatorController IdleController;
    public RuntimeAnimatorController JumpingController;
    public RuntimeAnimatorController FallingController;
    public RuntimeAnimatorController RunningController;
    public GameObject BackgroundFadeObject;
    private Animator PortalAnimator;
    private Rigidbody2D rb;
    private AudioSource teleportAudio;
    public AudioSource BackgroundMusic;

    public GameObject[] Positions;

    void Awake()
    {
        Reset();
    }

    // Start is called before the first frame update
    void Start()
    {
        teleportAudio = GetComponent<AudioSource>();
        transform.position = Positions[0].transform.position;
        rb = GetComponent<Rigidbody2D>();
        PortalAnimator = BackgroundFadeObject.GetComponent<Animator>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = FallingController;  
    }

    // Have the character go through fases
    // add in UPDATE: do this if phase == 1 (animation, movement)
    // Move from 1 phase to another with COLLISIONS.
    // Kill Colliders once hit, only reset at the end.

    public int phase = -1;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(phase);
        if(phase == 0)
        {
            animator.runtimeAnimatorController = FallingController;  
            Vector3 dir = (Positions[0].transform.position - transform.position).normalized * speed;
            rb.velocity = dir;   
        }  
        if(phase == 1)
        {
            animator.runtimeAnimatorController = RunningController;  
            Vector3 dir = (Positions[1].transform.position - transform.position).normalized * speed;
            rb.velocity = dir;   
        }   
        if(phase == 2)
        {
            animator.runtimeAnimatorController = RunningController;  
            Vector3 dir = (Positions[2].transform.position - transform.position).normalized * speed;
            rb.velocity = dir;   
        } 
        if(phase == 3)
        {
            animator.runtimeAnimatorController = JumpingController;  
            Vector3 dir = (Positions[3].transform.position - transform.position).normalized * speed;
            rb.velocity = dir;   
        }   
        if(phase == 4)
        {
            animator.runtimeAnimatorController = FallingController;  
            Vector3 dir = (Positions[4].transform.position - transform.position).normalized * speed;
            rb.velocity = dir;   
        }    
        if(phase == 5)
        {
            animator.runtimeAnimatorController = RunningController;  
            Vector3 dir = (Positions[5].transform.position - transform.position).normalized * speed;
            rb.velocity = dir;   
        }    
        if(phase == 6)
        {
            animator.runtimeAnimatorController = IdleController; 
        }   
        if(phase == 7)
        {
            animator.runtimeAnimatorController = JumpingController;  
            Vector3 dir = (Positions[6].transform.position - transform.position).normalized * speed;
            rb.velocity = dir;  
        }     
        if(phase == 8)
        {
            if(!AudioStopped)
            {
                BackgroundMusic.Stop();
                teleportAudio.time = 1.1f;
                teleportAudio.Play();
                AudioStopped =true;
            }
            animator.runtimeAnimatorController = JumpingController;  
            Vector3 dir = (Positions[7].transform.position - transform.position).normalized * speed * 2f;
            rb.velocity = dir;  
        }    
        if(phase == 9)
        {
            rb.velocity = Vector3.zero;
            if(gameObject.transform.localScale.x > 0.01f)
            {
                  gameObject.transform.localScale -= new Vector3(0.02f,0.02f,0);
            }
            else
            {
                phase = 10;
            }
        }  
        if(phase == 10)
        {
            ClosePortal();
            phase = 11;
        } 
    }

    private bool AudioStopped = false;

	void OnTriggerEnter2D(Collider2D other){

		if(other.tag == "NPCCollider"){
            phase = other.GetComponent<NPCColliderScript>().Phase + 1;
			other.gameObject.SetActive(false);
		}
	}


    public void Reset()
    {     
        AudioStopped = false;
        phase = 0;
        for(int i = 0; i < Positions.Length; i++)
        {
            Positions[i].SetActive(true);
        }
    }
    

    private void MoveToStep(int position)
    { 
        Vector3 dir = (Positions[position].transform.position - transform.position).normalized * speed;
        rb.velocity = dir;
    }

    public void StartGameAnimation()
    {        
        transform.position = Positions[6].transform.position;
        phase = 7;
        PortalAnimator.SetBool("PortalVisible", true);        
    }

    void ClosePortal()
    {
        PortalAnimator.SetBool("PortalVisible", false);
        Reset();
    }

}
