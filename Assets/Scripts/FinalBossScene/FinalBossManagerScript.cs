using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;
using System;

namespace Assets.Scripts.FinalBossScene 
{
    public class FinalBossManagerScript : MonoBehaviour
    {
        [SerializeField] private PlayerScript PlayerScript;
        [SerializeField] private FinalBossScript FinalBossScript;
        [SerializeField] private AudioSource Phase1Music;
        [SerializeField] private AudioSource Phase2Music;
        [SerializeField] private AudioSource Phase3Music; 
        public Assets.Scripts.FinalBossScene.TimerProgressBar timeMeter;
        
        public int PlaytimeInSeconds;
        private int CurrentTimeInSeconds;
        private bool timerStarted = false;

        // Start is called before the first frame update
        void Start()
        {
            BattleStage = 0;
            MainBattleStage = 0;
            PlayerScript.OnPlayerInteracted +=PlayerScript_OnPlayerInteracted;

            CurrentTimeInSeconds = PlaytimeInSeconds;
            float progressValue = (float)(CurrentTimeInSeconds/60f);
            timeMeter.InitFill(progressValue, "05:00");
        }

        private void PlayerScript_OnPlayerInteracted(object sender, PlayerInteractedEventArgs e)
        {
            if(e.InteractedWith.GetObjectName() == "Lever1_InteractableZone")
            {
                BattleStage = 2;
            }            
        }

        private int MainBattleStage = -1;
        private int BattleStage = -1;

        private int nextUpdate=1;
        // Update is called once per frame
        void Update()
        {
            if(MainBattleStage == 0)
            {
                
                if(CurrentTimeInSeconds <= 0)
                {
                    Lose();
                }

                if(Time.time>=nextUpdate)  // If the next update is reached
                {
                    float progressValue = (float)(CurrentTimeInSeconds/60f);
                    nextUpdate=Mathf.FloorToInt(Time.time)+1;    
                    TimeSpan time = TimeSpan.FromSeconds(CurrentTimeInSeconds);

                    timeMeter.SetFill(progressValue, "Time until ritual completion: " + time.ToString(@"mm\:ss"));
                    CurrentTimeInSeconds = CurrentTimeInSeconds - 1;
                }

                 if(BattleStage == 0)
                {
                    PlayerScript.SetArrow(FinalBossScript.gameObject);
                    PlayerScript.Options_ShowTargetingArrow = true;
                    PlayerScript.Say("I need to reach the boss before he finishes the ritual!");
                    BattleStage = 1;
                }
                if(BattleStage == 2)
                {
                    PlayerScript.Say("Good! The laser is off, now quickly to the next area!");
                    BattleStage = 3;
                }  
            }                     
        }




        void Lose()
        {

        }
    }
}