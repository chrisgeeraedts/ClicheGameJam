using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float timeUntillReverse = 120;
    float counter = 0;
    private bool goingLeft = true;

    
    [SerializeField] float MOV;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if(goingLeft)
        {
            MOV = movementSpeed * Time.deltaTime;
            transform.Translate(MOV, 0, 0); 
            counter += Time.deltaTime;

            if (timeUntillReverse < counter)
            {
                goingLeft = false;
            }
        }
        else
        {
            MOV = ((movementSpeed * Time.deltaTime) *-1);
            
            transform.Translate(MOV, 0, 0);
            counter -= Time.deltaTime;

            if (0 >= counter)
            {
                goingLeft = true;
            }
        }
    }
}
