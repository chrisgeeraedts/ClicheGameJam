using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Map;
using Assets.Scripts.Shared;

namespace Assets.Scripts.Escort
{
    public class MinigameManager : MonoBehaviour
    {
        public GameObject Player;
        public GameObject NPC;
        private bool Completed = false;

        [SerializeField] AudioSource GameMusic;
        [SerializeField] AudioSource DeathMusic;
        [SerializeField] AudioSource WinMusic;
        [SerializeField] AudioSource BridgeDown;
        
        
        public GameObject MissionTexts;
        [SerializeField] GameObject FrustrationMeter;
        private float frustrationCount;
        public float frustrationIncrement;
        public float maxDistancePlayerAndNpc;

        public GameObject bridgeTilemap;
        public GameObject bridgeLever;

        // Start is called before the first frame update
        void Start()
        {
            MissionTexts.GetComponent<MissionTextScript>().ShowTitle();
            bridgeTilemap.SetActive(false);
            StartCoroutine(HideTitle());
            Player.GetComponent<Assets.Scripts.Shared.IPlayer>().SetPlayerActive(false);
            NPC.GetComponent<Assets.Scripts.Shared.INPC>().SetNPCActive(false);
        }

        int phase = -1;
        int targetPhase = -1;

        public TMP_Text Distance;
        public TMP_Text Number;
        // Update is called once per frame
        void Update()
        {
            if(frustrationCount >= 1f && !Completed)
            {
                Lose();
            }

            // calculate distance between player and npc
            float distance = Vector3.Distance (Player.transform.position, NPC.transform.position);

            // debug            
            Distance.text = distance.ToString();
            Number.text = frustrationCount.ToString();

            if(distance > maxDistancePlayerAndNpc)
            {
                // if too large, add to frustratie meter
                frustrationCount += frustrationIncrement;
                FrustrationMeter.GetComponent<ProgressBar>().SetFill(frustrationCount);                
                NPC.GetComponent<Assets.Scripts.Shared.INPC>().SetNPCPaused(true);
                if(!showingHurryPopup)
                {
                    showingHurryPopup = true;
                    ShowHurryPopup();
                }
            }
            else{ 
                if(NPC.GetComponent<Assets.Scripts.Shared.INPC>().IsNPCPaused())      
                {
                    NPC.GetComponent<Assets.Scripts.Shared.INPC>().SetNPCPaused(false);
                }                           
            }
            

            if(Completed)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {    
                    GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.MapScene);
                }
            }
            else
            {
                if(phase == 0)
                {
                    Player.GetComponent<PlayerScript>().Say("Lets go!", 3f);
                    NPC.GetComponent<NPCScript>().Say("Not too fast!", 3f);
                    phase = 1;                    
                }
                else if(phase == 1)
                {
                    // wait a bit before we move    
                    WaitThenGoToPhase(2f, 3);
                    phase = 2;  
                }
                else if(phase == 2)
                {
                    // do nothing, we are waiting
                }
                else if(phase == 3)
                {
                    Player.GetComponent<PlayerScript>().Say("I better make sure he doesn't fall!", 3f);
                    NPC.GetComponent<NPCScript>().Say("What's that?", 3f);
                    phase = 4;    
                }
                else if(phase == 4)
                {
                    // do nothing, we are waiting
                }
                else if(phase == 5)
                {
                    Player.GetComponent<PlayerScript>().Say("So... slow...", 3f);
                    NPC.GetComponent<NPCScript>().Say("Im scared!", 3f);
                    phase = 6;    
                }
                else if(phase == 6)
                {
                    // do nothing, we are waiting
                }
                if(phase == 7)
                {
                    Player.GetComponent<PlayerScript>().Say("...", 3f);
                    NPC.GetComponent<NPCScript>().Say("Where did u learn to walk so fast?", 3f);
                    phase = 8;    
                }
                else if(phase == 8)
                {
                    // do nothing, we are waiting
                }
                else if(phase == 9)
                {
                    Player.GetComponent<PlayerScript>().Say("The exit! Finally!", 3f);
                    NPC.GetComponent<NPCScript>().Say("I can see the light! ", 3f);
                    phase = 10;    
                }
                else if(phase == 10)
                {
                    // do nothing, we are waiting
                }
                else if(phase == 11)
                {
                    // do nothing, we are waiting
                    NPC.GetComponent<NPCScript>().Say("Thanks! ", 3f);
                    NPC.GetComponent<SpriteRenderer>().enabled = false;
                    Win();
                }
                
            }
        }

        bool showingHurryPopup;
        void ShowHurryPopup()
        {
            Player.GetComponent<PlayerScript>().Say("Hurry up!", 2f);
            NPC.GetComponent<NPCScript>().Say("Im coming!", 2f);
            HideHurryPopup(2f);
        }
        
        IEnumerator HideHurryPopup(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            showingHurryPopup = false;
        }

        private void ActivateBridge()
        {
            bridgeTilemap.SetActive(true);
        }

        public void WaterHit(Collider2D col)
        {
            Debug.Log("Water hit!");
            // check if NPC or Player hit            
            Player.GetComponent<PlayerScript>().Say("Why would you do that...?", 5f);

            if(col.gameObject.tag == "Player")
            {
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(12); // heroes cant swim
            }
            Lose();
        }

        public void MilestoneHit(int milestone, Collider2D col)
        {
            Debug.Log("Milestone hit: " + milestone);
            if(milestone == 0)
            {
                phase = 0;
                // Escort me, dont go to fast!
                
            }
            if(milestone == 1)
            {
                // Watch out, a jump!
                phase = 3;
            }
            if(milestone == 2)
            {
                // Its scary
                phase = 5;
            }
            if(milestone == 3)
            {
                // Its scary
                phase = 7;
            }
            if(milestone == 4)
            {
                // Its scary
                phase = 9;
            }
            if(milestone == 5)
            {
                // Is that it? (check if NPC hit it)
                // set finishing phases
                phase = 11;
            }
            if(milestone == 999)
            {
                //lever hit                
                bridgeTilemap.SetActive(true);

                // play audio
                BridgeDown.Play();

                // change sprite
                bridgeLever.GetComponent<LeverScript>().Toggle(true);

                Player.GetComponent<PlayerScript>().Say("Hope this helps...", 3f);
            }
        }



        private void Win()
        {            
            MapManager.GetInstance().FinishMinigame(true);
            GameMusic.Stop();
            WinMusic.Play();
            
            Completed = true;

            Player.GetComponent<Assets.Scripts.Shared.IPlayer>().SetPlayerActive(false);
            NPC.GetComponent<Assets.Scripts.Shared.INPC>().SetNPCActive(false);

            GlobalAchievementManager.GetInstance().SetAchievementCompleted(2); // escort quests
            MissionTexts.GetComponent<MissionTextScript>().DoWin(); 
        }

        private void Lose()
        {
            MapManager.GetInstance().FinishMinigame(false);
            GameMusic.Stop();
            DeathMusic.Play();

            Completed = true;       
            
            Player.GetComponent<Assets.Scripts.Shared.IPlayer>().SetPlayerActive(false);
            NPC.GetComponent<Assets.Scripts.Shared.INPC>().SetNPCActive(false);

            MissionTexts.GetComponent<MissionTextScript>().DoLoss(); 
        }

        IEnumerator HideTitle()
        {
            yield return new WaitForSeconds(5f);
            MissionTexts.GetComponent<MissionTextScript>().HideTitle(); 
            Player.GetComponent<Assets.Scripts.Shared.IPlayer>().SetPlayerActive(true);
            NPC.GetComponent<Assets.Scripts.Shared.INPC>().SetNPCActive(true);
        }

        IEnumerator WaitThenGoToPhase(float waitTime, int nextPhase)
        {
            targetPhase = nextPhase;
            yield return new WaitForSeconds(waitTime);
            phase = targetPhase;
        }
    }
}