using System;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;
using Assets.Scripts.Map;

namespace Assets.Scripts.BarrelFun
{
    public class MinigameManager : MonoBehaviour
    {
        public Player2Script Player;
        public MissionTextScript MissionTexts;
        public Assets.Scripts.BarrelFun.ProgressBar timeMeter;
        public int PlaytimeInSeconds;
        private int CurrentTimeInSeconds;
        private bool timerStarted = false;
        public GameObject FinishedGameDoor;
        private bool Completed = false;

        [SerializeField] AudioSource GameMusic;
        [SerializeField] AudioSource DeathMusic;
        [SerializeField] AudioSource WinMusic;

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
            if(!Completed)
            {
                if(CurrentTimeInSeconds <= 0)
                {
                    Lose();
                }

                if(Time.time>=nextUpdate)  // If the next update is reached
                {
                    Debug.Log(CurrentTimeInSeconds);
                    float progressValue = (float)(CurrentTimeInSeconds/60f);
                    Debug.Log(progressValue);
                    nextUpdate=Mathf.FloorToInt(Time.time)+1;    
                    TimeSpan time = TimeSpan.FromSeconds(CurrentTimeInSeconds);

                    timeMeter.SetFill(progressValue, time.ToString(@"mm\:ss"));
                    CurrentTimeInSeconds = CurrentTimeInSeconds - 1;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R))
                {    
                    GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.MapScene);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
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
            
            Completed = true;

            Player.SetPlayerActive(false);

            MissionTexts.GetComponent<MissionTextScript>().DoWin(); 
        }

        private void Lose()
        {
            MapManager.GetInstance().FinishMinigame(false);
            GameMusic.Stop();
            DeathMusic.Play();

            Completed = true;       
                        
            Player.SetPlayerActive(false);

            MissionTexts.GetComponent<MissionTextScript>().DoLoss(); 
        }

        public void DeathHit()
        {
            Lose();
        }
    }
}