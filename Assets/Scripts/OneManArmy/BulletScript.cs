using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float lifetime = 2f;
    private AudioSource gunAudioHit;

    void Start()
    {        
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
            gunAudioHit = GetComponent<AudioSource>();
            gunAudioHit.Play();
            Destroy(gameObject);
        }   
        if(collision.gameObject.CompareTag("BulletWall"))
        { 
            gunAudioHit = GetComponent<AudioSource>();
            gunAudioHit.Play();
            Destroy(gameObject);
        }   
    }

    private void DamageEnemy(GameObject collidedEnemy)
    {
        collidedEnemy.GetComponent<Assets.Scripts.OneManArmy.Enemy>().TakeDamage();
    }
}
