using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.OneManArmy
{
    public class BulletScript : MonoBehaviour
    {
        private float lifetime = 2f;
        private AudioSource gunAudioHit;
    
        void Start()
        {       
             gunAudioHit = GetComponent<AudioSource>(); 
             StartCoroutine(Destroy());
        }
    
        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(lifetime);   
            Destroy(gameObject);
        }
    
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Zombie"))
            {
                DamageEnemy(collision.gameObject); 
                gunAudioHit.Play();
                 transform.localScale = new Vector3(0,0,0);
                lifetime = 0.1f;
                StartCoroutine(Destroy());
            }   
            if(collision.gameObject.CompareTag("BulletWall"))
            { 
                gunAudioHit.Play();
                 transform.localScale = new Vector3(0,0,0);
                lifetime = 0.1f;
                StartCoroutine(Destroy());
            }   
        }
    
        private void DamageEnemy(GameObject collidedEnemy)
        {
            collidedEnemy.GetComponent<Assets.Scripts.OneManArmy.Enemy>().TakeDamage();
        }
    }
}