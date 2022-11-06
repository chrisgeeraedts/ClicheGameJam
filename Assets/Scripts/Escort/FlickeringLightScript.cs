using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlickeringLightScript : MonoBehaviour
{
    public bool isActive;
    private float baseIntensity;
    public float firstValue;
    public float secondValue;
    private UnityEngine.Rendering.Universal.Light2D renderLight;
    // Start is called before the first frame update
    void Start()
    {
        baseIntensity = GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity;
        GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0;
        renderLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        queue = new Queue<float>();
        StartCoroutine(TimerLight());
    }

    private Queue<float> queue; // TODO: initialize
    public int smoothing = 5;

    public void ToggleActive(bool active)
    {
        isActive = active;

        if  (active)
        {
            GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = baseIntensity;
        }
        else
        {
            GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0;
        }
        
    }

    IEnumerator TimerLight()
    {
       var sum = 0f;

       while (true)
       {
            if(isActive)
            {
                while (queue.Count > smoothing)
                {
                    sum -= queue.Dequeue();
                }
                var newValue = Random.Range(firstValue, secondValue);
                queue.Enqueue(newValue);
                sum += newValue;
                renderLight.intensity = sum / queue.Count;
            }
          

           yield return new WaitForEndOfFrame();
       }
    }
}
