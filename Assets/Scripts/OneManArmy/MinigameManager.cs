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

        
        [SerializeField] TMP_Text ScoreTextElement;
        [SerializeField] TMP_Text TitleTextElement;

        // Start is called before the first frame update
        void Start()
        {
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

        float spawnModifier = 10; // go down to spawn faster
        IEnumerator StartSpawningZombiesAsync()
        {
            while(zombiesSpawned < zombieMax)
            {
                yield return new WaitForSeconds(spawnModifier);
                spawnModifier = spawnModifier * 0.9f;
                SpawnZombie();
            }
        }

        void SpawnZombie()
        {       
            zombiesSpawned++;
            // select spawnpoint
            int spawnPointIndex = Random.Range(0, 6);
            GameObject spawnPoint = SpawnPoints[spawnPointIndex];
            Debug.Log("Spawning zombie at " + spawnPointIndex);

            GameObject zombieGameObject = Instantiate(zombie, spawnPoint.transform.position, spawnPoint.transform.rotation);
            zombieGameObject.GetComponent<Enemy>().InitEnemy(3, Player, gameObject);

        }



        private int zombiesDestroyed = 0;
        private int zombiesSpawned = 0;
        private int zombieMax = 150;

        public void KilledZombie()
        {
            zombiesDestroyed++;
            ScoreTextElement.text = zombiesDestroyed.ToString();
        }

        public void PlayerDied()
        {
                HealthImage5_6.SetActive(false);
                HealthImage6_6.SetActive(true);
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