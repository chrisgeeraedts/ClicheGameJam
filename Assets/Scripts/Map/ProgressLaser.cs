using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLaser : MonoBehaviour
{
    public GameObject EndLocationGameObject;
    public GameObject EndLocationSFX;
    private LineRenderer LineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer.SetPosition(1, EndLocationGameObject.transform.position);
        EndLocationSFX.transform.position = EndLocationGameObject.transform.position;
    }
}
