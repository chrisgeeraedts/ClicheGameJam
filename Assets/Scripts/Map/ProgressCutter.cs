using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressCutter : MonoBehaviour
{
    public GameObject StartLocationGameObject;
    public GameObject EndLocationGameObject;
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
        LineRenderer.SetPosition(1, EndLocationGameObject.transform.position);
    }
}
