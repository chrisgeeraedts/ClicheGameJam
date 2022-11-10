using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OneManArmy
{
    public class Enemy : MonoBehaviour
    {
        private bool isReady = false;
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

        // Start is called before the first frame update
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            enemyAudioHit = GetComponent<AudioSource>();
        }

        // Update is called once per frame
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
                Killed();
            }
        }

        public void Killed()
        {
            // play audio            
            _miniGameManager.GetComponent<MinigameManager>().KilledZombie();
            
            Destroy(gameObject);
        }
    }
}