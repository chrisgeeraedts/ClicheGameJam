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
    public class FinalBossStage3Script : MonoBehaviour
    {
        #region Base
        [Header("Base Configuration")]
        [SerializeField] private PlayerScript PlayerScript;
        [SerializeField] private Image BossHealthBarElement;
        [SerializeField] private TMP_Text BossHealthTextElement;
        [SerializeField] private Material greenMaterial;
        [SerializeField] private Material blueMaterial;
        [SerializeField] private EyeLaserTimer EyeBeamTimer;
        [SerializeField] private float EyeBeamInitialTimeInSeconds;
        [SerializeField] private GameObject LaserTargeter;
        [SerializeField] private GameObject LaserEyes;
        [SerializeField] CinemachineCameraShake CinemachineCameraShake;
        [SerializeField] BloomController BloomController;
        
        [Space(10)]
        #endregion

        #region Stage 3
        [Header("Stage 3")]
        [SerializeField] private AudioSource Phase3Music; 
        [SerializeField] private AudioSource LoseMusic;        
        [SerializeField] private UnityEngine.Rendering.Universal.Light2D CoreLight;    
        [Space(10)]
        #endregion

        #region ArrowTargetElements
        [Header("Targeting Objects")]
        [SerializeField] private GameObject Target1; 
        [SerializeField] private GameObject Target2; 
        [SerializeField] private GameObject Target3; 
        [SerializeField] private GameObject Target4; 
        [SerializeField] private GameObject Target5; 
        [SerializeField] private GameObject Target6; 
        [Space(10)]
        #endregion

        #region Damaging zones
        [Header("Damaging Zones")]
        [SerializeField] private GameObject BossDamagingZone; 
        [SerializeField] private GameObject DamagingZoneLeftTop; 
        [SerializeField] private GameObject DamagingZoneLeftMiddle; 
        [SerializeField] private GameObject DamagingZoneLeftBottom; 
        [SerializeField] private GameObject DamagingZoneRightTop; 
        [SerializeField] private GameObject DamagingZoneRightMiddle; 
        [SerializeField] private GameObject DamagingZoneRightBottom; 
        [Space(10)]
        #endregion

        #region Energy Beams
        [Header("Energy Beams")]
        [SerializeField] private LineRenderer LaserLeftTop; 
        [SerializeField] private LineRenderer LaserLeftMiddle; 
        [SerializeField] private LineRenderer LaserLeftBottom; 
        [SerializeField] private LineRenderer LaserRightTop; 
        [SerializeField] private LineRenderer LaserRightMiddle; 
        [SerializeField] private LineRenderer LaserRightBottom; 
        [Space(10)]
        #endregion

         #region LightElements
        [Header("LightElements")]
        [SerializeField] private UnityEngine.Rendering.Universal.Light2D[] LightElements; 
        [Space(10)]
        #endregion

        [Header("DEBUG")] //Fighting boss stage 2
        [SerializeField] private TMP_Text DEBUG_TEXTFIELD;
        [SerializeField] private TMP_Text DEBUG_CURRENT_STAGE;
        [Space(10)]

        private bool LaserLeftTopCompleted; 
        private bool LaserLeftMiddleCompleted; 
        private bool LaserLeftBottomCompleted; 
        private bool LaserRightTopCompleted; 
        private bool LaserRightMiddleCompleted; 
        private bool LaserRightBottomCompleted; 

        [SerializeField] private BossHealthBar BossHealthBar; 


        // Start is called before the first frame update
        void Start()
        {
            DamagingZoneLeftTop.GetComponent<DamagingZoneScript>().Toggle(false);
            DamagingZoneLeftMiddle.GetComponent<DamagingZoneScript>().Toggle(false);
            DamagingZoneLeftBottom.GetComponent<DamagingZoneScript>().Toggle(false);
            DamagingZoneRightTop.GetComponent<DamagingZoneScript>().Toggle(false);
            DamagingZoneRightMiddle.GetComponent<DamagingZoneScript>().Toggle(false);
            DamagingZoneRightBottom.GetComponent<DamagingZoneScript>().Toggle(false);
            BossDamagingZone.GetComponent<DamagingZoneScript>().Toggle(true);

            MapManager.GetInstance().BossHP = 66666f;
            MapManager.GetInstance().BossMaxHP = 66666f;

            PlayerScript.OnPlayerInteracted +=PlayerScript_OnPlayerInteracted;
            PlayerScript.OnPlayerMilestoneHit +=PlayerScript_OnPlayerMilestoneHit;
            PlayerScript.OnPlayerDeath+=PlayerScript_OnPlayerDeath;

            BossHealthBarElement.fillAmount = MapManager.GetInstance().GetBossHPForFill();
            BossHealthTextElement.text = string.Format("{0:0}", MapManager.GetInstance().BossHP) + "/" + MapManager.GetInstance().BossMaxHP;

            BattleStage = 0;
        }

        int BattleStage = -1;
        private float CurrentEyeBeamTimeInSeconds;

        // Update is called once per frame
        void Update()
        {




            if(BattleStage == 0)
            {                
                CurrentEyeBeamTimeInSeconds = EyeBeamInitialTimeInSeconds;
                float progressValue = (float)(CurrentEyeBeamTimeInSeconds/EyeBeamInitialTimeInSeconds);
                TimeSpan time = TimeSpan.FromSeconds(CurrentEyeBeamTimeInSeconds);
                EyeBeamTimer.InitFill(progressValue, "Next attack: " + time.ToString(@"mm\:ss"));
                
                LaserEyes.SetActive(false);

                PlayerScript.SetArrow(Target1);
                PlayerScript.Options_ShowTargetingArrow = true;
                StartCoroutine(DoHeroIntroTalking());
                ChangeStage(1);
            }
            if(BattleStage == 1) // hero is talking in coroutine
            {
                //wait
            }
            if(BattleStage == 2) // Combat phase with boss
            {
                //wait
            }
            if(BattleStage == 10) // First laser activated
            {
                // Player say 'its working!'
                // intensify attacks from boss
            }
            if(BattleStage == 20) // Second laser activated
            {
                // Player say 'Another one!'
                // intensify attacks from boss
            }
            if(BattleStage == 30) // Third laser activated
            {
                // intensify attacks from boss
            }
            if(BattleStage == 40) // Fourth laser activated
            {
                // intensify attacks from boss
            }
            if(BattleStage == 50) // Fifth laser activated
            {
                // intensify attacks from boss
            }
            if(BattleStage == 60) // Last laser activated
            {
                //Boss dies
                // Cool animation
                // Explosions etc
                // Sound
            }

            if(CurrentEyeBeamTimeInSeconds <= 0)
            {
                // define random platform
                int portalChoice = UnityEngine.Random.Range(0, 6);

                if(portalChoice == 0)
                {
                    AttackPlatform(DamagingZoneLeftTop);
                }
                else if(portalChoice == 1)
                {
                    AttackPlatform(DamagingZoneLeftMiddle);
                }
                else if(portalChoice == 2)
                {
                    AttackPlatform(DamagingZoneLeftBottom);
                }
                else if(portalChoice == 3)
                {
                    AttackPlatform(DamagingZoneRightTop);
                }
                else if(portalChoice == 4)
                {
                    AttackPlatform(DamagingZoneRightMiddle);
                }
                else if(portalChoice == 5)
                {
                    AttackPlatform(DamagingZoneRightBottom);
                }
                

                // Reset timer
                CurrentEyeBeamTimeInSeconds = EyeBeamInitialTimeInSeconds; 
                TimeSpan time = TimeSpan.FromSeconds(CurrentEyeBeamTimeInSeconds);
                float progressValue = 1f;
                EyeBeamTimer.InitFill(progressValue, "Next attack: " + time.ToString(@"mm\:ss"));
            }

            if(Time.time>=nextUpdate)  // If the next update is reached
            {                
                Debug.Log("CurrentEyeBeamTimeInSeconds " + CurrentEyeBeamTimeInSeconds);
                Debug.Log("EyeBeamInitialTimeInSeconds " + EyeBeamInitialTimeInSeconds);

                float progressValue = (float)(CurrentEyeBeamTimeInSeconds/EyeBeamInitialTimeInSeconds);
                Debug.Log("progressValue " + progressValue);

                nextUpdate=Mathf.FloorToInt(Time.time)+1;    
                TimeSpan time = TimeSpan.FromSeconds(CurrentEyeBeamTimeInSeconds);
                Debug.Log("time " + time);

                EyeBeamTimer.SetFill(progressValue, "Next attack: " + time.ToString(@"mm\:ss"));
                CurrentEyeBeamTimeInSeconds = CurrentEyeBeamTimeInSeconds - 1;                
                Debug.Log("CurrentEyeBeamTimeInSeconds " + CurrentEyeBeamTimeInSeconds);
            }
        }

        private int nextUpdate= 1;

        public void AttackPlatform(GameObject platform)
        {
            // Shake screen
            CinemachineCameraShake.ShakeCamera(5f, 1.5f);
            BloomController.StartBloom(60f);

            // Move laser target over 2 seconds from start to end
            GameObject startPoint = platform.GetComponent<PlatformLaserStartEnd>().StartPoint;
            GameObject endPoint = platform.GetComponent<PlatformLaserStartEnd>().EndPoint;
            LaserTargeter.transform.position = startPoint.transform.position;
            
            LaserEyes.SetActive(true);
            StartCoroutine(MoveLaserOverZone(LaserTargeter, endPoint.transform.position, 1.5f));

            // enable Damaging zone on the platform
            StartCoroutine(ActivateDamageZone(1.5f, platform));
            StartCoroutine(DeactivateDamageZone(11.5f, platform));
        }

        IEnumerator MoveLaserOverZone(GameObject objectToMove, Vector3 end, float seconds)
        {     
            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;
            while (elapsedTime < seconds)
            {
                objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.position = end;

            // disable eye beams untill next attack
            LaserEyes.SetActive(false);
        }

        IEnumerator ActivateDamageZone(float delay, GameObject damagingZone)
        {     
            yield return new WaitForSeconds(delay);   
            
            BloomController.StopBloom();
            Debug.Log("Activated platform");
            damagingZone.GetComponent<DamagingZoneScript>().Toggle(true);
        }

        IEnumerator DeactivateDamageZone(float delay, GameObject damagingZone)
        {     
            yield return new WaitForSeconds(delay);   
            Debug.Log("Deactivated platform");
            damagingZone.GetComponent<DamagingZoneScript>().Toggle(false);
        }


        IEnumerator DoHeroIntroTalking()
        {     
            yield return new WaitForSeconds(1f);   
                PlayerScript.Say("I can't stay near him! He is too strong!", 0.125f, false, false);  
            yield return new WaitForSeconds(8f);   
                PlayerScript.Say("Perhaps I can use his energy against him!", 0.125f, false, false);  
            yield return new WaitForSeconds(5f);   
            ChangeStage(2);
        }

        private void ChangeStage(int nextStage)
        {
            BattleStage = nextStage;
            DEBUG_CURRENT_STAGE.text = nextStage.ToString();
        }

        private void PlayerScript_OnPlayerDeath(object sender, PlayerDeathEventArgs e)
        {
            Lose();
        }

        private void FinalBossScript_OnBossDeath(object sender, BossDeathEventArgs e)
        {            

        }

        private void PlayerScript_OnPlayerInteracted(object sender, PlayerInteractedEventArgs e)
        {
       
        }

        private void PlayerScript_OnPlayerMilestoneHit(object sender, PlayerMilestoneHitEventArgs e)
        {
       
        }

        public void DamageBoss()
        {
            // Play Audio

            // Have boss say something

            MapManager.GetInstance().BossHP = MapManager.GetInstance().BossHP - DamageAmountPerTick;
            BossHealthBarElement.fillAmount = MapManager.GetInstance().GetBossHPForFill();
            BossHealthTextElement.text = string.Format("{0:0}", MapManager.GetInstance().BossHP) + "/" + MapManager.GetInstance().BossMaxHP;
        }

        public int DamageAmountPerTick = 11110;

        public void ActivateEnergyBeam(int energyId)
        {
            EyeBeamInitialTimeInSeconds -=3f;
            CurrentEyeBeamTimeInSeconds -=3f;
            if(energyId == 1)
            {
                LaserLeftBottom.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss();
                LaserLeftBottomCompleted = true;
            }
            else if(energyId == 2)
            {
                LaserLeftMiddle.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss();
                LaserLeftMiddleCompleted = true;
            }
            else if(energyId == 3)
            {
                LaserLeftTop.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss();
                LaserLeftTopCompleted = true;
            }
            else if(energyId == 4)
            {
                LaserRightBottom.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss();
                LaserRightBottomCompleted = true;
            }
            else if(energyId == 5)
            {
                LaserRightMiddle.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss();
                LaserRightMiddleCompleted = true;
            }
            else if(energyId == 6)
            {
                LaserRightTop.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss();
                LaserRightTopCompleted = true;
            }
        }

        void Lose()
        {
            Phase3Music.Stop();
            LoseMusic.Play();
            StartCoroutine(TransferToGameOverScreen());            
        }
        IEnumerator TransferToGameOverScreen()
        {        
            yield return new WaitForSeconds(4f);   
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.GameOverScene);
        }
    }
}