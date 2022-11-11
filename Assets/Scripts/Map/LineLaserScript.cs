using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLaserScript : MonoBehaviour
{
    public GameObject StartGameObject;
    public GameObject EndGameObject;
    private LineRenderer lineRenderer;
    

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Physics.IgnoreLayerCollision(0, 10);
        Physics.IgnoreLayerCollision(0, 10);
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(StartGameObject.transform.position, EndGameObject.transform.position);
        //Debug.Log(hit.collider.name);
        Draw2DRay(StartGameObject.transform.position, EndGameObject.transform.position);
    }

    void Draw2DRay(Vector2 startPos, Vector2 EndPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, EndPos);
    }
}
