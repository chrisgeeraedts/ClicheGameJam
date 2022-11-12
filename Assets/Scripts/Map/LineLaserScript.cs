using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [RequireComponent (typeof (LineRenderer))]
 [ExecuteInEditMode]

public class LineLaserScript : MonoBehaviour
{
    public GameObject StartGameObject;
    public GameObject EndGameObject;
    private LineRenderer lineRenderer;
    public string sortingLayer;
    public bool Toggled;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        lineRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
        lineRenderer.sortingOrder = spriteRenderer.sortingOrder;
    }
    private Renderer getMeshRenderer()
    {
        return gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Toggled)
        {
            Draw2DRay(StartGameObject.transform.position, EndGameObject.transform.position);

            if(getMeshRenderer().sortingLayerName != sortingLayer && sortingLayer != ""){
                //Debug.Log("Forcing sorting layer: "+sortingLayer);
                getMeshRenderer().sortingLayerName = sortingLayer;
            }
        }
    }

    void Draw2DRay(Vector2 startPos, Vector2 EndPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, EndPos);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
