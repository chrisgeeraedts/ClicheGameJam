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
        [SerializeField] private AudioSource EvilLaughAudio;
        [SerializeField] private LaserDamagingZoneScript LaserDamagingZoneScript_LeftDoor;
        [SerializeField] private LaserDamagingZoneScript LaserDamagingZoneScript_RightDoor;
        [SerializeField] RuntimeAnimatorController BossBattleAnimationController;
        [SerializeField] BloomCameraRaiserScript BloomCameraRaiserScript;
        [SerializeField] CinemachineCameraShake CinemachineCameraShake;
        [SerializeField] CinemachineVirtualCamera PlayerCam;
        [SerializeField] CinemachineVirtualCamera BossCam;
        
        [Space(10)]
        #endregion

        #region Stage 3
        [Header("Stage 3")] //Fighting boss stage 2
        [SerializeField] private AudioSource PhaseTransition; 
        [SerializeField] private AudioSource Phase3Music; 
        [Space(10)]
        #endregion

        #region Lose
        [Header("Lose")] //Fighting boss stage 2
        [SerializeField] private AudioSource LoseMusic; 
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
            PlayerCam.enabled = true;
            BossCam.enabled = false;
            BattleStage = 0;
            PlayerScript.OnPlayerInteracted +=PlayerScript_OnPlayerInteracted;
            PlayerScript.OnPlayerMilestoneHit +=PlayerScript_OnPlayerMilestoneHit;
            PlayerScript.OnPlayerDeath+=PlayerScript_OnPlayerDeath;
            FinalBossScript.OnBossDeath+=FinalBossScript_OnBossDeath;
            

            CurrentTimeInSeconds = PlaytimeInSeconds;
            float progressValue = (float)(CurrentTimeInSeconds/60f);
            timeMeter.InitFill(progressValue, "02:00");

            LaserDamagingZoneScript_LeftDoor.TurnOff();
            LaserDamagingZoneScript_RightDoor.TurnOff();

            BossHealthBarElement.fillAmount = MapManager.GetInstance().GetBossHPForFill();
            BossHealthTextElement.text = string.Format("{0:0}", MapManager.GetInstance().BossHP) + "/" + MapManager.GetInstance().BossMaxHP;
        }

        private void PlayerScript_OnPlayerDeath(object sender, PlayerDeathEventArgs e)
        {
            Lose();
        }

        private void FinalBossScript_OnBossDeath(object sender, BossDeathEventArgs e)
        {            
            PlayerScript.StopMovement();
            PlayerScript.LockMovement();
            ChangeStage(20);
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
            if(BattleStage < 12)
            {
                if(Time.time>=nextUpdate)  // If the next update is reached
                {
                    float progressValue = (float)(CurrentTimeInSeconds/60f);
                    nextUpdate=Mathf.FloorToInt(Time.time)+1;    
                    TimeSpan time = TimeSpan.FromSeconds(CurrentTimeInSeconds);

                    timeMeter.SetFill(progressValue, "Time until ritual completion: " + time.ToString(@"mm\:ss"));
                    CurrentTimeInSeconds = CurrentTimeInSeconds - 1;
                }
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
                PlayerScript.SetPlayerActive(false);
                PlayerScript.SetArrow(null);
                PlayerScript.ToggleTargetingArrow(false);
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
                PlayerScript.SetPlayerActive(true);
                FinalBossScript.ToggleSpellcastingVisuals(false);
                FinalBossScript.gameObject.GetComponent<Animator>().runtimeAnimatorController = BossBattleAnimationController;
                FinalBossScript.SetActive();
                PlayerScript.UnlockMovement();
                timeMeter.gameObject.SetActive(false);
                
                Phase1Music.Stop();
                Phase2Music.Play();
                ChangeStage(14);
            }

            if(BattleStage == 14) // Boss Fight stage
            {
                // Do fighting. When boss health reaches 0, another battle stage will be started
                if (Time.time > nextTalkActionTime ) {
                    nextTalkActionTime += talkPeriod;
                    // execute block of code here
                    StartCoroutine(DoBossRandomTalk()); 
                }
            }

            if(BattleStage == 20) // Boss stage 1 defeated
            {                 
                PlayerScript.Options_ShowTargetingArrow = false;
                PlayerCam.enabled = false;
                BossCam.enabled = true;    
                Phase2Music.Stop();
                Debug.Log("Boss dead!");
                PlayerScript.SetPlayerActive(false);
                Debug.Log("SetPlayerActive");
                PlayerScript.StopMovement();
                Debug.Log("StopMovement");
                PlayerScript.LockMovement();
                Debug.Log("LockMovement");
                StartCoroutine(DoBossDeathStage1Talking());
                ChangeStage(21);
            } 
            if(BattleStage == 21) // Wait for death speech
            {              
                // wait
            }             
            if(BattleStage == 22) // Transformation
            {              
                PhaseTransition.Play();
                CinemachineCameraShake.ShakeCamera(5f, 10f);
                BloomCameraRaiserScript.StartBloom();
                StartCoroutine(GoToBossStage2());
                BattleStage = 23;
            }                 
        }

        IEnumerator GoToBossStage2()
        {    
            yield return new WaitForSeconds(4f);  
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.FinalBossFightStage2Scene);
        }

        private float nextTalkActionTime = 10.0f;
        public float talkPeriod = 12f;

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

        IEnumerator DoBossDeathStage1Talking()
        {     
            yield return new WaitForSeconds(2f);   
                FinalBossScript.Say("No... It cannot end like this!", 0.075f, false, false, 3f);  
            yield return new WaitForSeconds(6f);   
                FinalBossScript.Say("I...WILL...NOT...FALL!", 0.075f, false, false, 8f);  
            yield return new WaitForSeconds(8f);   
            ChangeStage(22);
        }

        IEnumerator DoBossEvilTalking()
        {     
            yield return new WaitForSeconds(1f);   
            FinalBossScript.Say("You made it", 0.125f, false, false, 3f);
            yield return new WaitForSeconds(4f);   
            FinalBossScript.Say("You will not stop my cliche master plan!", 0.075f, false, false, 5f);
            yield return new WaitForSeconds(5f);   
            FinalBossScript.Say("The world will be destroyed!", 0.075f, false, false, 5f);
            yield return new WaitForSeconds(5f);   
            FinalBossScript.Say("Muahahaha!", 0.075f, false, false, 3f);
            EvilLaughAudio.Play();            
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(16); //boss transformations
            yield return new WaitForSeconds(4f);   
            FinalBossScript.Say("Now... you die!", 0.075f, false, false, 3f);
            yield return new WaitForSeconds(3f);   
            PlayerScript.KnockBack(transform.position.x > PlayerScript.gameObject.transform.position.x);
            yield return new WaitForSeconds(1f);               
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(10); //Boss speeches
            ChangeStage(13);
        }


        IEnumerator DoBossRandomTalk()
        {     
            yield return new WaitForSeconds(3f);  
            int talkChoice = UnityEngine.Random.Range(0, 4);
            if(talkChoice == 0)
            {
                FinalBossScript.Say("I will destroy you!", 0.125f, false, false, 3f);
            }
            if(talkChoice == 1)
            {
                FinalBossScript.Say("Muhahaha!", 0.125f, false, false, 3f);
            }
            if(talkChoice == 2)
            {
                FinalBossScript.Say("Your end is now!", 0.125f, false, false, 3f);
            }
            if(talkChoice == 3)
            {
                FinalBossScript.Say("I will be victorious!", 0.125f, false, false, 3f);
            }
            if(talkChoice == 4)
            {
                FinalBossScript.Say("Your world will die!", 0.125f, false, false, 3f);
            }
            yield return new WaitForSeconds(3f);  
        }

        void Lose()
        {
            Phase1Music.Stop();
            Phase2Music.Stop();
            Phase3Music.Stop();
            LoseMusic.Play();
            StartCoroutine(TransferToGameOverScreen());            
        }

        IEnumerator TransferToGameOverScreen()
        {        
            yield return new WaitForSeconds(4f);   
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.GameOverScene);
        }




        public void DEBUG_SET_STAGE()
        {
            string textNumber = Regex.Replace(DEBUG_TEXTFIELD.text, "[^\\w\\._]", ""); //"2";//DEBUG_TEXTFIELD.text;
            int stage = Convert.ToInt32(textNumber);
            BattleStage = stage;
        }
    }
}