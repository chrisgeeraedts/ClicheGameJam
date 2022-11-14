using Assets.Scripts.Map;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    public class Vase : MonoBehaviour
    {
        [SerializeField] AudioClip brokenVaseSound;
        [SerializeField] Sprite brokenVaseSprite;
        [SerializeField] int coinsGained = 100;

        public void OnClick()
        {
            MapManager.GetInstance().GainCoins(coinsGained);
            AudioSource.PlayClipAtPoint(brokenVaseSound, transform.position);
            FindObjectOfType<Shopkeeper>()?.SetBrokenVaseText();

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
