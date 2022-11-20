using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLaserScript : MonoBehaviour
{
    public GameObject StartLocationGameObject;
    public GameObject StartLocationSFX;
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
        LineRenderer.SetPosition(0, StartLocationGameObject.transform.position);
        StartLocationSFX.transform.position = StartLocationGameObject.transform.position;
        LineRenderer.SetPosition(1, EndLocationGameObject.transform.position);
        EndLocationSFX.transform.position = EndLocationGameObject.transform.position;
    }
}
