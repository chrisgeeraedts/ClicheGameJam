using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    public GameObject startVFX;
    public GameObject endVFX;

    private Quaternion rotation;
    private List<ParticleSystem> startVFXPS = new List<ParticleSystem>();
    private List<ParticleSystem> endVFXPS = new List<ParticleSystem>();

    void Start()
    {     
        FillLists();
        EnableLaser();
    }

    void EnableLaser ()
    {        
        lineRenderer.enabled = true;

        for (int i=0; i<startVFXPS.Count; i++)
        {
            startVFXPS[i].Play();
        }

        for (int i=0; i<endVFXPS.Count; i++)
        {
            endVFXPS[i].Play();
        }
    }

    void DisableLaser ()
    {
        lineRenderer.enabled = false;

        for (int i=0; i<startVFXPS.Count; i++)
        {
            startVFXPS[i].Stop();
        }

        for (int i=0; i<endVFXPS.Count; i++)
        {
            endVFXPS[i].Stop();
        }
    }




    void FillLists()
    {
        for(int i=0; i<startVFX.transform.childCount; i++)
        {
            var ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps != null)
                startVFXPS.Add(ps);
        }

        for(int i=0; i<endVFX.transform.childCount; i++)
        {
            var ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps != null)
                endVFXPS.Add(ps);
        }
    }
}
