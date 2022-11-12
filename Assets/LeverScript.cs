using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public bool Toggled;
    public Sprite NoStateSprite;
    public Sprite YesStateSprite;
    public AudioSource LeverPulledAudio;

    public void Toggle(bool toggleState)
    {
        Toggled = toggleState;
        LeverPulledAudio.Play();
        InternalToggle();        
    }

    void Start()
    {
        InternalToggle();
    }

    void InternalToggle()
    {
        if(Toggled)
        {
            GetComponent<SpriteRenderer>().sprite = YesStateSprite;
        }
        else
        { 
            GetComponent<SpriteRenderer>().sprite = NoStateSprite;
        }
    }
}
