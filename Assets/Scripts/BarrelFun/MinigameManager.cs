using System;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;
using Assets.Scripts.Map;

namespace Assets.Scripts.BarrelFun
{
    public class MinigameManager : MonoBehaviour
    {
        public Player2Script Player;
        public Animator PlayerAnimator;
        public MissionTextScript MissionTexts;
        public Assets.Scripts.BarrelFun.ProgressBar timeMeter;
        public int PlaytimeInSeconds;
        private int CurrentTimeInSeconds;
        private bool timerStarted = false;
        public GameObject FinishedGameDoor;
        private bool Completed = false;
        [SerializeField] RuntimeAnimatorController HeroTeleportingAnimation;

        [SerializeField] AudioSource GameMusic;
        [SerializeField] AudioSource DeathMusic;
        [SerializeField] AudioSource WinMusic;
        [SerializeField] AudioSource TeleportAudio;

        // Start is called before the first frame update
        void Start()
        {
            MissionTexts.ShowTitle();          
            CurrentTimeInSeconds = PlaytimeInSeconds;
            float progressValue = (float)(CurrentTimeInSeconds/60f);
            timeMeter.InitFill(progressValue, "01:05");
            StartCoroutine(HideTitle());
            Player.SetPlayerActive(false);
        }

        private int nextUpdate=1;
        // Update is called once per frame
        void Update()
        {

            if(Completed)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {    
                    if(GameSceneChanger.Instance != null)
                    {
                        GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.MapScene);
                    }
                    else
                    {
                        SceneManager.LoadScene(Constants.SceneNames.MapScene);
                    }
                    
                }
            }

            if(!Completed)
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

                    timeMeter.SetFill(progressValue, time.ToString(@"mm\:ss"));
                    CurrentTimeInSeconds = CurrentTimeInSeconds - 1;
                }
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {           
            if(!Completed && col.gameObject.tag == "Player")
            {
                Completed = true; 
                Player.SetPlayerActive(false);
                Player.StopMovement();
                Player.ToggleGravity(false);
                Player.Reposition(new Vector3(18.83f, 41f, 0f));
                TeleportAudio.time = 2f;
                TeleportAudio.Play();                
                StartCoroutine(DoWinAnimations());
            }
            
        }

        IEnumerator DoWinAnimations()
        {
            // player teleporting animation
            PlayerAnimator.runtimeAnimatorController = HeroTeleportingAnimation;
            yield return new WaitForSeconds(3f);            
            Win();
        }

        IEnumerator HideTitle()
        {
            yield return new WaitForSeconds(5f);
            MissionTexts.HideTitle();   
            Player.SetPlayerActive(true);
            timerStarted = true;
        }

        private void Win()
        {            
            MapManager.GetInstance().FinishMinigame(true);
            
            GameMusic.Stop();
            WinMusic.Play();
            

            Player.SetPlayerActive(false);

            MissionTexts.GetComponent<MissionTextScript>().DoWin(); 
        }

        private void Lose()
        {
            Debug.Log("Lose");
            MapManager.GetInstance().FinishMinigame(false);
            GameMusic.Stop();
            DeathMusic.Play();

            Completed = true;       
                        
            Player.SetPlayerActive(false);

            MissionTexts.GetComponent<MissionTextScript>().DoLoss(); 
        }
        private bool WasDeathHit = false;
        public void DeathHit(Collider2D col)
        {
            Debug.Log("DeathHit - 1");
            if (col.gameObject.tag == "Player")
            {
            Debug.Log("DeathHit - 2");
                if(!WasDeathHit)
                {
            Debug.Log("DeathHit - 3");
                    WasDeathHit = true;
                    Debug.Log("DeathHit - " + col.gameObject.name);
                    Lose();   
                }  
            }
        }
    }
}