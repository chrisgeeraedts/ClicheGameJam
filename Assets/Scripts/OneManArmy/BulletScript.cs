using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float lifetime = 5f;
    private AudioSource gunAudioHit;

    void OnStart()
    {        
         StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(lifetime);   
        Destroy (gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Zombie"))
        {
            DamageEnemy(collision.gameObject); 
            gunAudioHit = GetComponent<AudioSource>();
            gunAudioHit.Play();
        }   

        transform.localScale = new Vector3(0,0,0);
    }

    private void DamageEnemy(GameObject collidedEnemy)
    {
        collidedEnemy.GetComponent<Assets.Scripts.OneManArmy.Enemy>().TakeDamage();
    }
}
