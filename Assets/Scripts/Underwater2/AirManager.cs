using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;
using Assets.Scripts.Map;
using Assets.Scripts.Shared;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Linq;

namespace Assets.Scripts.Underwater2
{
    public class AirManager : MonoBehaviour
    {
        public PlayerScript PlayerScript;
        public float MaxAir;
        public float CurrentAir;
        public BreathTimer BreathTimer;

        // Start is called before the first frame update
        void Start()
        {
            BreathTimer.SetFill(1, "100%"); 
        }

        
        private int nextUpdate= 1;
        // Update is called once per frame
        void Update()
        {
            if(Time.time>=nextUpdate)  // If the next update is reached
            {             
                if(PlayerScript.IsInWater())
                {
                    Debug.Log("Breath Tick: " + CurrentAir + "/" + MaxAir);
                    float progressValue = (float)(CurrentAir/MaxAir);  
                    CurrentAir = CurrentAir -1;
                    nextUpdate=Mathf.FloorToInt(Time.time)+1;  
                    BreathTimer.SetFill(progressValue, progressValue + "%"); 
                }   
            }
        }

        public void GiveAir()
        {
            CurrentAir = MaxAir;
        }
    }
}