using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;
using Assets.Scripts.Map;
using Assets.Scripts.Shared;

namespace Assets.Scripts.FinalBossScene 
{
    public class FinalBossManagerScript : MonoBehaviour
    {
        
        #region Base
        [Header("Base Configuration")]
        [SerializeField] private PlayerScript PlayerScript;
        [SerializeField] private FinalBossScript FinalBossScript;
        [SerializeField] private Image BossHealthBarElement;
        [SerializeField] private TMP_Text BossHealthTextElement;
        [Space(10)]
        #endregion

        #region Stage 1
        [Header("Stage 1")] // Getting to boss
        [SerializeField] private AudioSource Phase1Music;
        [SerializeField] private  Assets.Scripts.FinalBossScene.TimerProgressBar timeMeter;
        [SerializeField] private  int PlaytimeInSeconds;
        public FinalBossMinionScript[] Minions;
        [Space(10)]
        #endregion

        #region Stage 2
        [Header("Stage 2")] //Fighting boss stage 1
        [SerializeField] private AudioSource Phase2Music;
        [SerializeField] private LaserDamagingZoneScript LaserDamagingZoneScript_LeftDoor;
        [SerializeField] private LaserDamagingZoneScript LaserDamagingZoneScript_RightDoor;
        [SerializeField] RuntimeAnimatorController BossBattleAnimationController;
        [Space(10)]
        #endregion

        #region Stage 2
        [Header("Stage 2")] //Fighting boss stage 2
        [SerializeField] private AudioSource Phase3Music; 
        [Space(10)]
        #endregion


        [Header("DEBUG")] //Fighting boss stage 2
        [SerializeField] private TMP_Text DEBUG_TEXTFIELD;
        [SerializeField] private TMP_Text DEBUG_CURRENT_STAGE;
        [Space(10)]
        
        private int CurrentTimeInSeconds;
        private bool timerStarted = false;

        // Start is called before the first frame update
        void Start()
        {
            BattleStage = 0;
            PlayerScript.OnPlayerInteracted +=PlayerScript_OnPlayerInteracted;
            PlayerScript.OnPlayerMilestoneHit +=PlayerScript_OnPlayerMilestoneHit;

            CurrentTimeInSeconds = PlaytimeInSeconds;
            float progressValue = (float)(CurrentTimeInSeconds/60f);
            timeMeter.InitFill(progressValue, "05:00");

            LaserDamagingZoneScript_LeftDoor.TurnOff();
            LaserDamagingZoneScript_RightDoor.TurnOff();

            BossHealthBarElement.fillAmount = MapManager.GetInstance().GetBossHPForFill();
            BossHealthTextElement.text = string.Format("{0:0}", MapManager.GetInstance().BossHP) + "/" + MapManager.GetInstance().BossMaxHP;
        }

        private void PlayerScript_OnPlayerInteracted(object sender, PlayerInteractedEventArgs e)
        {
            if(e.InteractedWith.GetObjectName() == "Lever1_InteractableZone")
            {
                BattleStage = 2;
            }            
        }

        private void PlayerScript_OnPlayerMilestoneHit(object sender, PlayerMilestoneHitEventArgs e)
        {
            BattleStage = e.MilestoneCollider.StageId;          
        }

        private int BattleStage = -1;

        private int nextUpdate=1;
        // Update is called once per frame
        void Update()
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
                ChangeStage(1);
            }
            if(BattleStage == 2)
            {
                PlayerScript.Say("Good! The laser is off, now quickly to the next area!");
                ChangeStage(3);
            }  




            if(BattleStage == 10) // Boss stage reached.
            {
                // boss talking
                LaserDamagingZoneScript_LeftDoor.TurnOn();
                LaserDamagingZoneScript_RightDoor.TurnOn();
                ChangeStage(11);
            }

            if(BattleStage == 11) // Doors are close. Have boss talk and stop animating spellcast
            {                
                PlayerScript.SetArrow(null);
                PlayerScript.Options_ShowTargetingArrow = false;
                PlayerScript.StopMovement();
                PlayerScript.LockMovement();
                
                
                StartCoroutine(DoBossEvilTalking());
                ChangeStage(12);
            }

            if(BattleStage == 12) // Wait
            {
                
            }

            if(BattleStage == 13) // Set boss config
            {                
                FinalBossScript.ToggleSpellcastingVisuals(false);
                FinalBossScript.gameObject.GetComponent<Animator>().runtimeAnimatorController = BossBattleAnimationController;
                FinalBossScript.SetActive();
                PlayerScript.UnlockMovement();
                ChangeStage(14);
            }

            if(BattleStage == 14) // Boss Fight stage
            {
                // Do fighting. When boss health reaches 0, another battle stage will be started
            }

            if(BattleStage == 15) // Boss Fighting Wait Stage
            {
                ChangeStage(16);
            }                 
        }

        private void ChangeStage(int nextStage)
        {
            BattleStage = nextStage;
            DEBUG_CURRENT_STAGE.text = nextStage.ToString();
        }

        public void MilestoneHit(int stage, Collider2D col)
        {
            ChangeStage(stage);
        }

        IEnumerator WaitFor(float waitTime, int nextStage)
        {        
            yield return new WaitForSeconds(waitTime);    
            BattleStage = nextStage;
        }

        IEnumerator DoBossEvilTalking()
        {     
            yield return new WaitForSeconds(1f);   
            FinalBossScript.Say("You made it", 0.125f, false, false, 3f);
            yield return new WaitForSeconds(4f);   
            FinalBossScript.Say("You will not stop my cliche master plan!", 0.125f, false, false, 5f);
            yield return new WaitForSeconds(6f);   
            FinalBossScript.Say("The world will be destroyed!", 0.125f, false, false, 5f);
            yield return new WaitForSeconds(6f);   
            FinalBossScript.Say("Muahahaha!", 0.125f, false, false, 3f);
            yield return new WaitForSeconds(4f);   
            FinalBossScript.Say("Now... you die!", 0.125f, false, false, 3f);
            yield return new WaitForSeconds(4f);   
            PlayerScript.JumpOutOfWater();
            yield return new WaitForSeconds(1f);   
            ChangeStage(13);
        }

        void Lose()
        {

        }




        public void DEBUG_SET_STAGE()
        {
            Debug.Log(DEBUG_TEXTFIELD.text);
            string textNumber = Regex.Replace(DEBUG_TEXTFIELD.text, "[^\\w\\._]", ""); //"2";//DEBUG_TEXTFIELD.text;
            int stage = Convert.ToInt32(textNumber);
            
            Debug.Log("result: " + stage);
            BattleStage = stage;
        }
    }
}