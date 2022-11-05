using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OneManArmy
{
    public class Enemy : MonoBehaviour
    {
        public void InitEnemy(int maxHealth)
        {
            MaxHealth = maxHealth;
        }

        public int MaxHealth;
        public int CurrentHealth;

        [SerializeField] GameObject playerToBeTargeted;
        [SerializeField] float movementSpeed;

        Rigidbody2D rg;

        // Start is called before the first frame update
        void Awake()
        {
            rg = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
                Vector3 direction = (playerToBeTargeted.transform.position - transform.position).normalized;
                rg.velocity = direction * movementSpeed;

            
                direction.z = 0;
        
                Vector3 neutralDir = transform.up;
                float angle = Vector3.SignedAngle(neutralDir, direction, Vector3.forward) + 90f;
                direction = Quaternion.AngleAxis(angle, Vector3.forward) * neutralDir;
                transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(true)
            {
                DamagePlayer();
            }
        }

        private void DamagePlayer()
        {
            playerToBeTargeted.GetComponent<Assets.Scripts.OneManArmy.Player>().TakeDamage();
        }

        public void TakeDamage()
        {            
            Debug.Log("ZOMBIE GOT HIT!");
        }
    }
}