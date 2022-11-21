using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Fishing
{
    public class CaughtFish : MonoBehaviour
    {
        [SerializeField] List<Sprite> caughtFishSprites;
        [SerializeField] GameObject parentGameObject;
        [SerializeField] Image image;

        public bool IsShowing = false;

        public void ShowCatch(int fishLevelCaught)
        {
            image.sprite = caughtFishSprites[fishLevelCaught];
            parentGameObject.SetActive(true);
            IsShowing = true;
        }

        public void HideCatch()
        {
            parentGameObject.SetActive(false);
            IsShowing = false;
        }
    }
}
