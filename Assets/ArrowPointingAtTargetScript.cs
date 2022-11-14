using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointingAtTargetScript : MonoBehaviour
{
    public GameObject ObjectToTarget;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        isReady = true;
    }
    private bool isReady = false;
    void FixedUpdate()
    {
        if(isReady)
        {   
            float distance = Vector3.Distance (ObjectToTarget.transform.position, Player.transform.position);
            if(distance > 5f)
            {
                Vector3 direction = (ObjectToTarget.transform.position - Player.transform.position).normalized;    
                Vector3 neutralDir = transform.up;
                float angle = Vector3.SignedAngle(neutralDir, direction, Vector3.forward) + 90f;
                direction = Quaternion.AngleAxis(angle, Vector3.forward) * neutralDir;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                transform.position = Player.transform.position;
            }
            

            
        }
        else
        {
            //rb.velocity = Vector3.zero;
        }
    }
}
