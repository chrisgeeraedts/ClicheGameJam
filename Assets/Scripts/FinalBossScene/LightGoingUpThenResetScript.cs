using System.Collections;
using System.Collections.Generic;
using UnityEngine;




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FinalBossScene 
{
    public class LightGoingUpThenResetScript : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 10;
        [SerializeField] private float timeUntillReverse = 120;
        float counter = 0;
        private bool goingUp = true;
        private UnityEngine.Rendering.Universal.Light2D light;

        
        [SerializeField] float MOV;

        // Start is called before the first frame update
        void Start()
        {
            light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        }


        // Update is called once per frame
        void Update()
        {
            if(goingUp)
            {
                light.enabled = true;
                MOV = movementSpeed * Time.deltaTime;
                transform.Translate(0, MOV, 0); 
                counter += Time.deltaTime;

                if (timeUntillReverse < counter)
                {
                    goingUp = false;
                }
            }
            else
            {
                light.enabled = false;
                MOV = ((movementSpeed * Time.deltaTime) *-1);
                
                transform.Translate(0, MOV, 0);
                counter -= Time.deltaTime;

                if (0 >= counter)
                {
                    goingUp = true;
                }
            }
        }
    }
}