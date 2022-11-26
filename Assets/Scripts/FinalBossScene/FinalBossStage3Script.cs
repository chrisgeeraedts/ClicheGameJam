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

namespace Assets.Scripts.FinalBossScene 
{
    public class FinalBossStage3Script : MonoBehaviour, IGetHealthSystem
    {
        #region Base
        [Header("Base Configuration")]
        [SerializeField] private PlayerScript PlayerScript;
        [SerializeField] private StationaryBoss StationaryBoss;
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
        [SerializeField] CinemachineVirtualCamera PlayerCamera;
        [SerializeField] CinemachineCameraShake BossDeadCinemachineCameraShake;
        [SerializeField] BloomController BossDeadBloomController;
        [SerializeField] CinemachineVirtualCamera BossCamera;
        [SerializeField] EasyExpandableTextBox BossTextbox;
        [SerializeField] GameObject bossTextboxObject;
        [SerializeField] BloomCameraRaiserScript BloomCameraRaiserScript;
        [SerializeField] Volume GlobalVolume;
        [SerializeField] GameObject EnergyOrb;
        [SerializeField] GameObject AbsorbedEnergyOrb;
        

        [Space(10)]
        #endregion

        #region Stage 3
        [Header("Stage 3")]
        [SerializeField] private AudioSource Phase3Music; 
        [SerializeField] private AudioSource EyeBeamAudio; 
        [SerializeField] private AudioSource LoseMusic;     
        [SerializeField] private AudioSource WinMusic;     
        [SerializeField] private AudioSource BossDead;  
        [SerializeField] private AudioSource BossHurt; 
        [SerializeField] private AudioSource BossStart;       
        [SerializeField] private AudioSource FinalAudio;       
        [SerializeField] private UnityEngine.Rendering.Universal.Light2D CoreLight;    
        [SerializeField] private SpriteRenderer BossRenderer;    
        [SerializeField] private AudioSource EnergyOrbAbsorbing; 
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
        [SerializeField] private int DamageAmountPerLaser = 11111;
        [Space(10)]
        #endregion

        #region LightElements
        [Header("LightElements")]
        [SerializeField] private UnityEngine.Rendering.Universal.Light2D[] LightElements; 
        [SerializeField] private UnityEngine.Rendering.Universal.Light2D[] PlatformLightElements; 
        [Space(10)]
        #endregion

        #region Explosions
        [Header("Explosions")]
        [SerializeField] private RandomAnimationActivator[] RandomAnimationActivators; 
        [Space(10)]
        #endregion

        #region Canvasses
        [Header("Canvasses")]
        [SerializeField] private Canvas[] CanvasToDisableAfterWin; 
        [Space(10)]
        #endregion

        #region EnergyControls
        [Header("EnergyControls")]
        [SerializeField] private EnergyControlBox[] EnergyControls; 
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

        private HealthSystem _healthSystem;
        [SerializeField] private HealthBarUI Base_HealthBarUI; 

        public HealthSystem GetHealthSystem()
        {
            return _healthSystem;
        }

        // Start is called before the first frame update
        void Start()
        {            
            BossCamera.enabled = false;
            PlayerCamera.enabled = true;
            BossTextbox.Hide();
            EnergyOrb.SetActive(false);
            AbsorbedEnergyOrb.SetActive(false);
            DamagingZoneLeftTop.GetComponent<DamagingZoneScript>().Toggle(false);
            DamagingZoneLeftMiddle.GetComponent<DamagingZoneScript>().Toggle(false);
            DamagingZoneLeftBottom.GetComponent<DamagingZoneScript>().Toggle(false);
            DamagingZoneRightTop.GetComponent<DamagingZoneScript>().Toggle(false);
            DamagingZoneRightMiddle.GetComponent<DamagingZoneScript>().Toggle(false);
            DamagingZoneRightBottom.GetComponent<DamagingZoneScript>().Toggle(false);
            BossDamagingZone.GetComponent<DamagingZoneScript>().Toggle(true);

            MapManager.GetInstance().BossHP = 66666f;
            MapManager.GetInstance().BossMaxHP = 66666f;

            
             _healthSystem = new HealthSystem(MapManager.GetInstance().BossMaxHP);
            Base_HealthBarUI.SetHealthSystem(_healthSystem);

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
            if (!_bossIsDead)
            {
                if(BattleStage == 0)
                {                
                    CurrentEyeBeamTimeInSeconds = EyeBeamInitialTimeInSeconds;
                    float progressValue = (float)(CurrentEyeBeamTimeInSeconds/EyeBeamInitialTimeInSeconds);
                    TimeSpan time = TimeSpan.FromSeconds(CurrentEyeBeamTimeInSeconds);
                    EyeBeamTimer.InitFill(progressValue, "Next attack: " + time.ToString(@"mm\:ss"));
                    
                    LaserEyes.SetActive(false);

                   

                    StartCoroutine(DoHeroIntroTalking());
                    GlobalAchievementManager.GetInstance().SetAchievementCompleted(11); //boss transformations
                    ChangeStage(1);
                }
                if(BattleStage == 1) 
                {
                    //wait
                }                
                if(BattleStage == 2) // Combat phase with boss
                {
                    // keep arrow on closest control
                    PlayerScript.SetArrow(GetClosestEnergyControl(EnergyControls).gameObject);
                    PlayerScript.Options_ShowTargetingArrow = true;
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
                    Debug.Log("ATTACK TIMER " + CurrentEyeBeamTimeInSeconds);
                    float progressValue = (float)(CurrentEyeBeamTimeInSeconds/EyeBeamInitialTimeInSeconds);

                    nextUpdate=Mathf.FloorToInt(Time.time)+1;    
                    TimeSpan time = TimeSpan.FromSeconds(CurrentEyeBeamTimeInSeconds);

                    EyeBeamTimer.SetFill(progressValue, "Next attack: " + time.ToString(@"mm\:ss")); 

                    CurrentEyeBeamTimeInSeconds = CurrentEyeBeamTimeInSeconds -1;       
                }



                
                if(BattleStage == 3) //all 6 energy
                {
                    for (int i = 0; i < PlatformLightElements.Length; i++)
                    {
                        PlatformLightElements[i].color = Color.blue;
                    }


                    // Say
                    PlayerScript.Say("I've sapped his energy. Now to collect it!", 0.075f, false, false, 1f);  
                    
                    //orb spawns 
                    EnergyOrb.SetActive(true);
                    
                    //set arrow to orb
                    PlayerScript.SetArrow(EnergyOrb);
                    PlayerScript.Options_ShowTargetingArrow = true;

                    ChangeStage(4);
                }
                if(BattleStage == 4)
                {
                    // wait for collision
                }
                if(BattleStage == 5) // received orb
                {
                    //Change player blue
                    //Change player larger
                    //Increase Jump range player
                    PlayerScript.Empower(10000);
                    EnergyOrbAbsorbing.Play();
                    EnergyOrb.SetActive(false);
                    AbsorbedEnergyOrb.SetActive(true);
                    
                    PlayerScript.Say("I feel so strong! Lets slay this beast!", 0.075f, false, false); 
                    // Disable bad aura
                    BossDamagingZone.SetActive(false);
                    ChangeStage(6);
                }
                if(BattleStage == 6) // ready to finish boss
                {
                    //Player attack boss
                    //Collide boss on attack
                    //Boss takes 5000 damage
                }
                if(BattleStage == 7) // Boss finally hit
                {
                    DamageBoss(10000);
                    ChangeStage(7);
                }
                if(BattleStage == 8) // Boss finally hit
                {
                }
            }
        }

        public void AttemptToDamageBoss()
        {
            if(BattleStage == 6) // check if we are on this phase of the fight
            {                
                ChangeStage(7);
            }
            else
            {
                DamageBoss(1);
            }
        }

        private int nextUpdate= 1;

        public void AttackPlatform(GameObject platform)
        {
            if(!_bossIsDead)
            {
                // Shake screen
                CinemachineCameraShake.ShakeCamera(5f, 1.5f);
                BloomController.StartBloom(60f);

                // Play Audio
                EyeBeamAudio.Play();

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
            damagingZone.GetComponent<DamagingZoneScript>().Toggle(true);
        }

        IEnumerator DeactivateDamageZone(float delay, GameObject damagingZone)
        {     
            yield return new WaitForSeconds(delay);   
            damagingZone.GetComponent<DamagingZoneScript>().Toggle(false);
        }


        IEnumerator DoHeroIntroTalking()
        {     
            yield return new WaitForSeconds(1f);   
                BossStart.Play();
                PlayerScript.Say("I can't stay near him! He is too strong!", 0.075f, false, false);  
            yield return new WaitForSeconds(6f);   
                PlayerScript.Say("Perhaps I can use his energy against him!", 0.075f, false, false);  
            yield return new WaitForSeconds(6f);   
            ChangeStage(2);
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

        public void DamageBoss(int damageAmount)
        {
            if(!_bossIsDead)
            {
                // Play Audio
                BossHurt.Play();

                // Have boss say something
                MapManager.GetInstance().BossHP = MapManager.GetInstance().BossHP - damageAmount;
                BossHealthBarElement.fillAmount = MapManager.GetInstance().GetBossHPForFill();
                BossHealthTextElement.text = string.Format("{0:0}", MapManager.GetInstance().BossHP) + "/" + MapManager.GetInstance().BossMaxHP;

                // Check death
                if(MapManager.GetInstance().BossHP <= 0)
                {
                    _bossIsDead = true;
                    Base_HealthBarUI.gameObject.SetActive(false);
                    BossHealthBarElement.fillAmount = 0;
                    BossHealthTextElement.text = "0/66600";
                    Win();
                }
                else
                {                    
                    _healthSystem.Damage(damageAmount);
                }
            }
            
        }

        private int LasersActivated = 0;
        public void ActivateEnergyBeam(int energyId)
        {
            LasersActivated++;

            if(LasersActivated == 1)
            {                
                PlayerScript.Say("It seems to work!", 0.075f, false, false);  
            }
            if(LasersActivated == 3)
            {                
                PlayerScript.Say("He is weakening!", 0.075f, false, false);  
            }            
            if(LasersActivated == 5)
            {                
                PlayerScript.Say("Just one more!", 0.075f, false, false);  
            }     

            

            if(energyId == 1)
            {
                LaserLeftBottom.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss(DamageAmountPerLaser);
                LaserLeftBottomCompleted = true;
                AttackPlatform(DamagingZoneLeftBottom);
            }
            else if(energyId == 2)
            {
                LaserLeftMiddle.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss(DamageAmountPerLaser);
                LaserLeftMiddleCompleted = true;
                AttackPlatform(DamagingZoneLeftMiddle);
            }
            else if(energyId == 3)
            {
                LaserLeftTop.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss(DamageAmountPerLaser);
                LaserLeftTopCompleted = true;
                AttackPlatform(DamagingZoneLeftTop);
            }
            else if(energyId == 4)
            {
                LaserRightBottom.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss(DamageAmountPerLaser);
                LaserRightBottomCompleted = true;
                AttackPlatform(DamagingZoneRightBottom);
            }
            else if(energyId == 5)
            {
                LaserRightMiddle.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss(DamageAmountPerLaser);
                LaserRightMiddleCompleted = true;
                AttackPlatform(DamagingZoneRightMiddle);
            }
            else if(energyId == 6)
            {
                LaserRightTop.material = blueMaterial;
                LightElements[energyId-1].color = Color.blue;
                DamageBoss(DamageAmountPerLaser);
                LaserRightTopCompleted = true;
                AttackPlatform(DamagingZoneRightTop);
            }

            if(LasersActivated == 6)
            {                
                EyeBeamInitialTimeInSeconds = 4f;
                CurrentEyeBeamTimeInSeconds = 4f;
                BattleStage = 3;  
            }            
            else if(energyId < 7){                
                CurrentEyeBeamTimeInSeconds -=3f;
                EyeBeamInitialTimeInSeconds = CurrentEyeBeamTimeInSeconds;
            }
        }

        EnergyControlBox GetClosestEnergyControl(EnergyControlBox[] energyControls)
        {
            EnergyControlBox tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = PlayerScript.gameObject.transform.position;
            foreach (EnergyControlBox t in energyControls.Where(x => !x.Toggled))
            {
                float dist = Vector3.Distance(t.gameObject.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin;
        }

        bool _bossIsDead = false;
        void Win()
        {
            Bloom _bloom;
            BossCamera.enabled = true;
            PlayerCamera.enabled = false;
            PlayerScript.SetPlayerActive(false);
            PlayerScript.LockMovement();
            PlayerScript.StopMovement();
            Phase3Music.Stop(); 
            GlobalVolume.profile.TryGet(out _bloom);
            _bloom.tint.value = Color.blue;

            

            // Shake screen
            Debug.Log(BossDeadCinemachineCameraShake);
            BossDeadCinemachineCameraShake.ShakeCamera(5f, 100f);
            
            BossTextbox.Show(bossTextboxObject, 3f);
            BossDead.Play();  
            StartCoroutine(BossTextbox.EasyMessage("Noooooooo!", 0.125f, false, false, 3f)); 
            StartCoroutine(BossExplosions());
            StartCoroutine(BossDestruction());
        }

        IEnumerator BossExplosions()
        {            
            yield return new WaitForSeconds(3f); 
            if(RandomAnimationActivators != null)
            {
                for (int i = 0; i < RandomAnimationActivators.Length; i++)
                {
                    RandomAnimationActivators[i].StartAnimationRandomly(500, 0.65f);
                }
            }
        }

        IEnumerator BossDestruction(bool wait = true)
        {            
            if(wait)
            {
                yield return new WaitForSeconds(10f);  
            }

            for (int i = 0; i < CanvasToDisableAfterWin.Length; i++)
            {
                CanvasToDisableAfterWin[i].enabled = false;
            }

            FinalAudio.Play();
            float tick = 0f;
            Color endColor = Color.blue;
                Debug.Log("Coloring");

            StationaryBoss.Death();

            BloomCameraRaiserScript.StartBloom();
            yield return new WaitForSeconds(3f); 
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.CompletedGameScene); 
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

        public void ActivateEnergySphere()
        {
            BattleStage = 5;
        }

        private void ChangeStage(int nextStage)
        {
            BattleStage = nextStage;
            Debug.Log("STAGE: " + nextStage);
        }


        int DEBUG = 1;
        public void GOTOFINALBOSSSTAGE()
        {
            ActivateEnergyBeam(DEBUG);
            DEBUG++;
        }

        public void DEBUG_DESTROY_BOSS()
        {            
            ActivateEnergyBeam(1);
            ActivateEnergyBeam(2);
            ActivateEnergyBeam(3);
            ActivateEnergyBeam(4);
            ActivateEnergyBeam(5);
            ActivateEnergyBeam(6);
            DamageBoss(10000);
        }
    }
}