﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Map
{
    class HatManager : MonoBehaviour
    {
        [SerializeField] Sprite avatarWithSillyHatImage;
        [SerializeField] Sprite avatarWithCrown;
        [SerializeField] Image avatarRenderer;

        private void Start()
        {
            ApplyHat();
        }

        private void ApplyHat()
        {
            Debug.Log($"Applying hat. Player is wearing hat: {MapManager.GetInstance().IsWearingSillyHat}");
            if (MapManager.GetInstance().IsWearingSillyHat)
            {
                avatarRenderer.sprite = avatarWithSillyHatImage;
            }
            else
            {
                avatarRenderer.sprite = avatarWithCrown;
            }
        }

        public void SwapHats()
        {
            if (!MapManager.GetInstance().HasSillyHat) return;
            Debug.Log("Swapping hats"); 
            MapManager.GetInstance().IsWearingSillyHat = !MapManager.GetInstance().IsWearingSillyHat;
            ApplyHat();
        }
    }
}
