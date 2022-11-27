using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;

public class CantJumpCollider : MonoBehaviour
{
    public PlayerScript PlayerScript;
    public bool CanJump;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerScript.Options_CanJump = CanJump;
    }
}
