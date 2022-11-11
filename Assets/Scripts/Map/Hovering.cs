using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hovering : MonoBehaviour
{
    public float MaxUp;
    public float Speed;
    private float StartY;
    private float CurrentY;
    private float TargetY;
    private float ActiveTargetY;

    public Rigidbody body;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    body = GetComponent<Rigidbody2D>();
    //    StartY = gameObject.transform.position.y;
    //    TargetY = StartY + MaxUp;
    //    CurrentY = StartY;
    //}
//
    //bool goingUp = true;
    //// Update is called once per frame
    //void FixedUpdate()
    //{   
    //    if(CurrentY == StartY) {
    //        ActiveTargetY = TargetY;
    //    }
    //    else if (CurrentY == TargetY) {
    //        ActiveTargetY = StartY;
    //    }
//
    //    // check if we hit the top
//
//
    //    Vector3 targetDirection = (currentPos - targetPos).normalized;
    //    body.MovePosition(currentPos + targetDirection * Time.deltaTime);
    //}
}
