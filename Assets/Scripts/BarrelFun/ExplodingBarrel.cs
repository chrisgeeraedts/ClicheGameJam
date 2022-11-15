using Assets.Scripts.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BarrelFun
{
    public class ExplodingBarrel : MonoBehaviour
    {
        [SerializeField] AudioClip explosionClip;
        [SerializeField] GameObject explosionAnimation;

        void Start()
        {
            explosionAnimation.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag != Constants.TagNames.Bullet) return;

            Explode();
            
            //disable bullet
            collision.gameObject.SetActive(false); 
            Destroy(collision.gameObject);
        }



        private void Explode()
        {
            StartCoroutine(CompleteExplosion());

            GlobalAchievementManager.GetInstance().SetAchievementCompleted(0);
            AudioSource.PlayClipAtPoint(explosionClip, transform.position, 0.3f);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            explosionAnimation.SetActive(true);
        }

        
        IEnumerator CompleteExplosion()
        {
            yield return new WaitForSeconds(0.8f);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }
}
