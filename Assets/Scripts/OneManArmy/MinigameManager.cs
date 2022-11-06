using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Assets.Scripts.OneManArmy
{
    public class MinigameManager : MonoBehaviour
    {
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
        [SerializeField] TMP_Text TitleTextElement;
        [SerializeField] TMP_Text GameWinTextElement;
        [SerializeField] TMP_Text GameLossTextElement;

        [SerializeField] AudioSource GameMusic;
        [SerializeField] AudioSource DeathMusic;
        [SerializeField] AudioSource WinMusic;

        
        [SerializeField] int ZombieHealth = 2;

        // Start is called before the first frame update
        void Start()
        {
            activeZombies = new List<GameObject>();
            GameWinTextElement.enabled = false;
            GameLossTextElement.enabled = false;
            HealthImage0_6.SetActive(true);
            HealthImage1_6.SetActive(false);
            HealthImage2_6.SetActive(false);
            HealthImage3_6.SetActive(false);
            HealthImage4_6.SetActive(false);
            HealthImage5_6.SetActive(false);
            HealthImage6_6.SetActive(false);
            SpawnZombie();
            StartSpawningZombies();
            StartCoroutine(HideTitle());
        }

        IEnumerator HideTitle()
        {
            yield return new WaitForSeconds(10f);
            Destroy(TitleTextElement);
        }

        void StartSpawningZombies()
        {
            StartCoroutine(StartSpawningZombiesAsync());
        }

        float spawnModifier = 3; // go down to spawn faster
        IEnumerator StartSpawningZombiesAsync()
        {
            while(!Completed)
            {
                if(zombiesSpawned < zombieMax)
                {
                    yield return new WaitForSeconds(spawnModifier);
                    if(spawnModifier > 0.2f)
                    {
                        spawnModifier = spawnModifier * 0.9f;
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
            Debug.Log("Spawning zombie at " + spawnPointIndex + " Spawning speed is now: " + spawnModifier);

            GameObject zombieGameObject = Instantiate(zombie, spawnPoint.transform.position, spawnPoint.transform.rotation);
            zombieGameObject.GetComponent<Enemy>().InitEnemy(ZombieHealth, Player, gameObject);
            activeZombies.Add(zombieGameObject);
        }



        private int zombiesDestroyed = 0;
        private int zombiesSpawned = 0;
        private int zombieMax = 150;
        private int zombieKillGoal = 100;

        private bool Completed = false;

        public void KilledZombie()
        {
            zombiesDestroyed++;
            ScoreTextElement.text = zombiesDestroyed.ToString();

            if(zombiesDestroyed >= zombieKillGoal)
            {
                // WIN
                Win();
            }
        }

        void Update()
        {
            if(Completed)
            {
                // get escape key press
                // Go back to map
            }
        }

        private void Win()
        {
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
            // DEFEAT                
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
                GameMusic.Stop();
                DeathMusic.Play();
                HealthImage5_6.SetActive(false);
                HealthImage6_6.SetActive(true);
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
            }
            if(damageTakenTotal == 4)
            {
                HealthImage3_6.SetActive(false);
                HealthImage4_6.SetActive(true);
            }
            if(damageTakenTotal == 3)
            {
                HealthImage2_6.SetActive(false);
                HealthImage3_6.SetActive(true);
            }
            if(damageTakenTotal == 2)
            {
                HealthImage1_6.SetActive(false);
                HealthImage2_6.SetActive(true);
            }
            if(damageTakenTotal == 1)
            {
                HealthImage0_6.SetActive(false);
                HealthImage1_6.SetActive(true);
            }            
        }
    }
}