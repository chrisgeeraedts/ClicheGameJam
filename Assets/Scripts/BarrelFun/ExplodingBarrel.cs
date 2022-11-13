using Assets.Scripts.Shared;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BarrelFun
{
    public class ExplodingBarrel : MonoBehaviour
    {
        [SerializeField] AudioClip explosionClip;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag != Constants.TagNames.Bullet) return;

            Explode();
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }

        private void Explode()
        {
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(0);
            AudioSource.PlayClipAtPoint(explosionClip, transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
