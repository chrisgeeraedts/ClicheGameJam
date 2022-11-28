using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OneManArmy
{
    public class Enemy : MonoBehaviour
    {
        private bool isReady = false;
        public AudioSource[] ZombieDeathAudio;
        public AudioSource[] ZombieGrowlAudio;
        public UnityEngine.Rendering.Universal.Light2D Light2D;
        public void InitEnemy(int maxHealth, GameObject playerToBeTargeted, GameObject miniGameManager)
        {
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            _playerToBeTargeted = playerToBeTargeted;
            _miniGameManager = miniGameManager;
            isReady = true;
        }

        public void StopEnemy()
        {
            isReady = false;
        }

        public int MaxHealth;
        public int CurrentHealth;

        GameObject _playerToBeTargeted;
        GameObject _miniGameManager;
        [SerializeField] float movementSpeed;
        AudioSource enemyAudioHit;

        Rigidbody2D rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            enemyAudioHit = GetComponent<AudioSource>();
        }

        void FixedUpdate()
        {
            if(isReady)
            {    
                Vector3 direction = (_playerToBeTargeted.transform.position - transform.position).normalized;
                rb.velocity = direction * movementSpeed;
                        direction.z = 0;
        
                Vector3 neutralDir = transform.up;
                float angle = Vector3.SignedAngle(neutralDir, direction, Vector3.forward) + 90f;
                direction = Quaternion.AngleAxis(angle, Vector3.forward) * neutralDir;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);


                int makeSound = UnityEngine.Random.Range(0, 4000);
                if(makeSound == 0)
                {
                    int growlSound = UnityEngine.Random.Range(0, 4);
                    ZombieGrowlAudio[growlSound].Play();
                }
                

            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(isReady)
            { 
                if(collision.gameObject.CompareTag("Player"))
                {
                    DamagePlayer();
                }   
            }
        }

        private void DamagePlayer()
        {
            _playerToBeTargeted.GetComponent<Assets.Scripts.OneManArmy.Player>().TakeDamage();
        }

        public void TakeDamage()
        {            
            CurrentHealth+=-1;
            if (CurrentHealth == 0)
            {
                if (0 == Random.Range(0, 4))
                {
                    int deathSound = UnityEngine.Random.Range(0, 4);
                    ZombieDeathAudio[deathSound].Play();
                }

                Killed();
            }
        }

        public void Killed()
        {
            // play audio            
            _miniGameManager.GetComponent<MinigameManager>().KilledZombie();
            isReady = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<CircleCollider2D>().enabled = false;
            Destroy(Light2D);
            StartCoroutine(ActuallyDestroy());
        }

        IEnumerator ActuallyDestroy()
        {
            yield return new WaitForSeconds(2f);
            try
            {
                Destroy(gameObject);
            }   
            catch{}         
        }
    }
}