using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLaserStartEnd : MonoBehaviour
{
    public GameObject StartPoint;
    public GameObject EndPoint;

    private void Start()
    {
        StartPoint.GetComponent<SpriteRenderer>().enabled = false;        
        EndPoint.GetComponent<SpriteRenderer>().enabled = false;
    }
}
