using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hovering : MonoBehaviour
{
    public float MaxUp;
    public float Speed;

    private Vector3 pos1;
    private Vector3 pos2;
 
    void Start()
    {
        pos1 = new Vector3(transform.position.x, transform.position.y-MaxUp,0);
        pos2 = new Vector3(transform.position.x, transform.position.y+MaxUp,0);
    }

     void Update() {
         transform.position = Vector3.Lerp (pos1, pos2, Mathf.PingPong(Time.time*Speed, 5f*Speed));
     }
}
