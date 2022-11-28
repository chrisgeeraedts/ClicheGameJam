using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Underwater2
{
    public class AirBubbles : MonoBehaviour
    {
        public AirManager AirManager;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            AirManager.GiveAir();
        }
    }
}