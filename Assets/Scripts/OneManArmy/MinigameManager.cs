using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;
using Assets.Scripts.Map;

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
        public GameObject MissionTexts;

        [SerializeField] AudioSource GameMusic;
        [SerializeField] AudioSource DeathMusic;
        [SerializeField] AudioSource WinMusic;


        [SerializeField] int ZombieHealth = 2;
        [SerializeField] int ZombieKillGoal = 100;
        [SerializeField] int zombieMax = 25;

        void Start()
        {
            MissionTexts.GetComponent<MissionTextScript>().ShowTitle();
            activeZombies = new List<GameObject>();
            PlayerChatBubble.SetActive(false);
            HealthImage0_6.SetActive(true);
            HealthImage1_6.SetActive(false);
            HealthImage2_6.SetActive(false);
            HealthImage3_6.SetActive(false);
            HealthImage4_6.SetActive(false);
            HealthImage5_6.SetActive(false);
            HealthImage6_6.SetActive(false);
            ScoreTextElement.text = "<color=#fede34>" + 0 + "</color>/" + ZombieKillGoal.ToString();
            Player.GetComponent<Assets.Scripts.Shared.IPlayer>().SetPlayerActive(false);
            StartCoroutine(HideTitle());
        }

        IEnumerator HideTitle()
        {
            yield return new WaitForSeconds(5f);
            MissionTexts.GetComponent<MissionTextScript>().HideTitle();
            Player.GetComponent<Assets.Scripts.Shared.IPlayer>().SetPlayerActive(true);
            SpawnZombie();
            StartSpawningZombies();
        }

        void StartSpawningZombies()
        {
            StartCoroutine(StartSpawningZombiesAsync());
        }

        float spawnModifier = 4; // go down to spawn faster
        IEnumerator StartSpawningZombiesAsync()
        {
            while (!Completed && !zombieLimitReached)
            {
                if (zombiesSpawned < zombieMax)
                {
                    yield return new WaitForSeconds(spawnModifier);
                    if (spawnModifier > 0.2f)
                    {
                        spawnModifier = Mathf.Max(0.5f, spawnModifier * 0.92f);
                    }
                    SpawnZombie();
                }

                zombieLimitReached = zombiesSpawned + 1 >= zombieMax;
            }
        }

        void SpawnZombie()
        {
            zombiesSpawned++;
            int spawnPointIndex = Random.Range(0, 6);
            GameObject spawnPoint = SpawnPoints[spawnPointIndex];

            GameObject zombieGameObject = Instantiate(zombie, spawnPoint.transform.position, spawnPoint.transform.rotation);
            zombieGameObject.GetComponent<Enemy>().InitEnemy(ZombieHealth, Player, gameObject);
            activeZombies.Add(zombieGameObject);
        }



        private int zombiesDestroyed = 0;
        private int zombiesSpawned = 0;

        private bool Completed = false;
        private bool zombieLimitReached = false;

        public void KilledZombie()
        {
            zombiesDestroyed++;
            ScoreTextElement.text = "<color=#fede34>" + zombiesDestroyed.ToString() + "</color>/" + ZombieKillGoal.ToString();

            if (zombiesDestroyed >= ZombieKillGoal)
            {
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(1);
                Win();
            }
        }

        void Update()
        {
            if (Completed)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene(Constants.SceneNames.MapScene);
                }
            }
        }

        private void Win()
        {
            //MapManager.GetInstance().FinishMinigame(true);
            GameMusic.Stop();
            WinMusic.Play();

            MissionTexts.GetComponent<MissionTextScript>().DoWin();
            Completed = true;
            foreach (GameObject zombie in activeZombies)
            {
                if (zombie != null)
                {
                    zombie.SetActive(false);
                    Destroy(zombie);
                }
            }
            Player.GetComponent<Player>().Complete();
            Time.timeScale = 0;
        }

        private void Lose()
        {
            //MapManager.GetInstance().FinishMinigame(false);
            GameMusic.Stop();
            DeathMusic.Play();
            HealthImage5_6.SetActive(false);
            HealthImage6_6.SetActive(true);

            MissionTexts.GetComponent<MissionTextScript>().DoLoss();
            Completed = true;
            foreach (GameObject zombie in activeZombies)
            {
                if (zombie != null)
                {
                    zombie.GetComponent<Enemy>().StopEnemy();
                }
            }
            Player.GetComponent<Player>().Complete();
            Time.timeScale = 0;
        }

        public void PlayerDied()
        {
            Lose();
        }

        public void PlayerTakenDamage(int damageTakenTotal)
        {
            if (damageTakenTotal == 6)
            {
                HealthImage5_6.SetActive(false);
                HealthImage6_6.SetActive(true);
            }
            if (damageTakenTotal == 5)
            {
                HealthImage4_6.SetActive(false);
                HealthImage5_6.SetActive(true);

                PlayerChatBubble.SetActive(true);
                PlayerChatTextElement.text = "And my torso? Incredible how I am still doing all of this!";

                GlobalAchievementManager.GetInstance().SetAchievementCompleted(9);

                if (!PopupIsOpen)
                {
                    HideChatBubble();
                }
                else
                {
                    NewPopupHasOpened = true;
                }
                PopupIsOpen = true;
            }
            if (damageTakenTotal == 4)
            {
                HealthImage3_6.SetActive(false);
                HealthImage4_6.SetActive(true);

                PlayerChatBubble.SetActive(true);
                PlayerChatTextElement.text = "Both of my arms as well! How am I holding a weapon?!?";
                if (!PopupIsOpen)
                {
                    HideChatBubble();
                }
                else
                {
                    NewPopupHasOpened = true;
                }
                PopupIsOpen = true;
            }
            if (damageTakenTotal == 3)
            {
                HealthImage2_6.SetActive(false);
                HealthImage3_6.SetActive(true);

                PlayerChatBubble.SetActive(true);
                PlayerChatTextElement.text = "My arm! Aaah!";
                if (!PopupIsOpen)
                {
                    HideChatBubble();
                }
                else
                {
                    NewPopupHasOpened = true;
                }
                PopupIsOpen = true;
            }
            if (damageTakenTotal == 2)
            {
                HealthImage1_6.SetActive(false);
                HealthImage2_6.SetActive(true);

                PlayerChatBubble.SetActive(true);
                PlayerChatTextElement.text = "Both my legs! How am I still walking?!?";
                if (!PopupIsOpen)
                {
                    HideChatBubble();
                }
                else
                {
                    NewPopupHasOpened = true;
                }
                PopupIsOpen = true;
            }
            if (damageTakenTotal == 1)
            {
                HealthImage0_6.SetActive(false);
                HealthImage1_6.SetActive(true);

                PlayerChatBubble.SetActive(true);
                PlayerChatTextElement.text = "Aaaah, my leg!";
                if (!PopupIsOpen)
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
            if (!NewPopupHasOpened)
            {
                PlayerChatBubble.SetActive(false);
                PopupIsOpen = false;
            }
            else
            {
                NewPopupHasOpened = false;
                HideChatBubble();
            }
        }
    }
}