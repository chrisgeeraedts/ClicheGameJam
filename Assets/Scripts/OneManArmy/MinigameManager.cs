using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;

namespace Assets.Scripts.OneManArmy
{
    public class MinigameManager : MonoBehaviour
    {
        [SerializeField] GameObject PlayerChatBubble;
        [SerializeField] TMP_Text PlayerChatTextElement;

        [SerializeField] GameObject Player;
        [SerializeField] GameObject HealthImage0_6;
        [SerializeField] GameObject HealthImage1_6;
        [SerializeField] GameObject HealthImage2_6;
        [SerializeField] GameObject HealthImage3_6;
        [SerializeField] GameObject HealthImage4_6;
        [SerializeField] GameObject HealthImage5_6;
        [SerializeField] GameObject HealthImage6_6;

        [SerializeField] GameObject[] SpawnPoints;
        public GameObject zombie;
        private List<GameObject> activeZombies;

        
        [SerializeField] TMP_Text ScoreTextElement;
        [SerializeField] Image TitleTextElement;
        [SerializeField] TMP_Text GameWinTextElement;
        [SerializeField] TMP_Text GameLossTextElement;

        [SerializeField] AudioSource GameMusic;
        [SerializeField] AudioSource DeathMusic;
        [SerializeField] AudioSource WinMusic;

        
        [SerializeField] int ZombieHealth = 2;
        [SerializeField] int ZombieKillGoal = 100;
        [SerializeField] int zombieMax = 25;

        // Start is called before the first frame update
        void Start()
        {
            activeZombies = new List<GameObject>();
            GameWinTextElement.enabled = false;
            GameLossTextElement.enabled = false;
            PlayerChatBubble.SetActive(false);
            HealthImage0_6.SetActive(true);
            HealthImage1_6.SetActive(false);
            HealthImage2_6.SetActive(false);
            HealthImage3_6.SetActive(false);
            HealthImage4_6.SetActive(false);
            HealthImage5_6.SetActive(false);
            HealthImage6_6.SetActive(false);
            ScoreTextElement.text = "0/" + ZombieKillGoal.ToString();
            SpawnZombie();
            StartSpawningZombies();
            StartCoroutine(HideTitle());
        }

        IEnumerator HideTitle()
        {
            yield return new WaitForSeconds(5f);
            Destroy(TitleTextElement);
        }

        void StartSpawningZombies()
        {
            StartCoroutine(StartSpawningZombiesAsync());
        }

        float spawnModifier = 4; // go down to spawn faster
        IEnumerator StartSpawningZombiesAsync()
        {
            while(!Completed)
            {
                if(zombiesSpawned < zombieMax)
                {
                    yield return new WaitForSeconds(spawnModifier);
                    if(spawnModifier > 0.2f)
                    {
                        spawnModifier = spawnModifier * 0.92f;
                    }
                    SpawnZombie();
                }
                
            }
        }

        void SpawnZombie()
        {       
            zombiesSpawned++;
            // select spawnpoint
            int spawnPointIndex = Random.Range(0, 6);
            GameObject spawnPoint = SpawnPoints[spawnPointIndex];
            //Debug.Log("Spawning zombie at " + spawnPointIndex + " Spawning speed is now: " + spawnModifier);

            GameObject zombieGameObject = Instantiate(zombie, spawnPoint.transform.position, spawnPoint.transform.rotation);
            zombieGameObject.GetComponent<Enemy>().InitEnemy(ZombieHealth, Player, gameObject);
            activeZombies.Add(zombieGameObject);
        }



        private int zombiesDestroyed = 0;
        private int zombiesSpawned = 0;

        private bool Completed = false;

        public void KilledZombie()
        {
            zombiesDestroyed++;
            ScoreTextElement.text = zombiesDestroyed.ToString() + "/" + ZombieKillGoal.ToString();

            if(zombiesDestroyed >= ZombieKillGoal)
            {
                // WIN
                GlobalAchievementManager.Instance.SetAchievementCompleted(1);
                Win();
            }
        }

        void Update()
        {
            if(Completed)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Debug.Log("Escape key was pressed"); // Go to other scene
                    SceneManager.LoadScene(Constants.SceneNames.MapScene);
                }
            }
        }

        private void Win()
        {            
            GameMusic.Stop();
            WinMusic.Play();

            GameWinTextElement.enabled = true;
            Completed = true;
            foreach (GameObject zombie in activeZombies)
            {
                if(zombie != null)
                {
                    Destroy(zombie);
                }
            }
            Player.GetComponent<Player>().Complete();
        }

        private void Lose()
        {
            GameMusic.Stop();
            DeathMusic.Play();
            HealthImage5_6.SetActive(false);
            HealthImage6_6.SetActive(true);

            GameLossTextElement.enabled = true;
            Completed = true;
            foreach (GameObject zombie in activeZombies)
            {
                if(zombie != null)
                {
                    zombie.GetComponent<Enemy>().StopEnemy();
                }
            }
            Player.GetComponent<Player>().Complete();
        }

        public void PlayerDied()
        {
            Lose();
        }

        public void PlayerTakenDamage(int damageTakenTotal)
        {
            if(damageTakenTotal == 6)
            {
                HealthImage5_6.SetActive(false);
                HealthImage6_6.SetActive(true);
            }
            if(damageTakenTotal == 5)
            {
                HealthImage4_6.SetActive(false);
                HealthImage5_6.SetActive(true);

                PlayerChatBubble.SetActive(true);
                PlayerChatTextElement.text = "And my torso? Incredible how I am still doing all of this!";

                //TODO: ARCHIEVEMENT
                GlobalAchievementManager.Instance.SetAchievementCompleted(9);

                if(!PopupIsOpen)    
                {
                    HideChatBubble();
                }  
                else
                {
                    NewPopupHasOpened = true;
                }
                PopupIsOpen = true;   
            }
            if(damageTakenTotal == 4)
            {
                HealthImage3_6.SetActive(false);
                HealthImage4_6.SetActive(true);

                PlayerChatBubble.SetActive(true);
                PlayerChatTextElement.text = "Both of my arms as well! How am I holding a weapon?!?";
                if(!PopupIsOpen)    
                {
                    HideChatBubble();
                } 
                else
                {
                    NewPopupHasOpened = true;
                } 
                PopupIsOpen = true;   
            }
            if(damageTakenTotal == 3)
            {
                HealthImage2_6.SetActive(false);
                HealthImage3_6.SetActive(true);

                PlayerChatBubble.SetActive(true);
                PlayerChatTextElement.text = "My arm! Aaah!";
                if(!PopupIsOpen)    
                {
                    HideChatBubble();
                }  
                else
                {
                    NewPopupHasOpened = true;
                }
                PopupIsOpen = true;   
            }
            if(damageTakenTotal == 2)
            {
                HealthImage1_6.SetActive(false);
                HealthImage2_6.SetActive(true);

                PlayerChatBubble.SetActive(true);
                PlayerChatTextElement.text = "Both my legs! How am I still walking?!?";
                if(!PopupIsOpen)    
                {
                    HideChatBubble();
                }  
                else
                {
                    NewPopupHasOpened = true;
                }
                PopupIsOpen = true;   
            }
            if(damageTakenTotal == 1)
            {
                HealthImage0_6.SetActive(false);
                HealthImage1_6.SetActive(true);

                PlayerChatBubble.SetActive(true);
                PlayerChatTextElement.text = "Aaaah, my leg!";       
                if(!PopupIsOpen)    
                {
                    HideChatBubble();
                }  
                else
                {
                    NewPopupHasOpened = true;
                }
                PopupIsOpen = true;   
            }            
        }


        void HideChatBubble()
        {
            StartCoroutine(HideChatBubbleAsync());
        }

        private bool PopupIsOpen = false;
        private bool NewPopupHasOpened = false;


        IEnumerator HideChatBubbleAsync()
        {
            yield return new WaitForSeconds(3f);
            if(!NewPopupHasOpened)
            {
                PlayerChatBubble.SetActive(false); 
                PopupIsOpen = false;      
            }
            else{
                NewPopupHasOpened = false;
                HideChatBubble();
            }
        }
    }
}