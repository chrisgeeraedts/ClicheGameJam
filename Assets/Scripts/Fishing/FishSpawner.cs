using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Fishing
{
    public class FishSpawner : MonoBehaviour 
    {
        [SerializeField] GameObject fishSpottedPrefab;

        public Vector2 FishLocation;

        public GameObject SpawnScaledFish(float scale)
        {
            //StartCoroutine(SpawnFish(scale));
            return SpawnFish(scale);
        }

        private GameObject SpawnFish(float scale = 1f)
        {
            var instance = Instantiate(fishSpottedPrefab);
            instance.transform.position = GetFishLocation();
            instance.transform.localScale = new Vector2(scale, scale);

            return instance;
        }

        //private IEnumerator SpawnFish(float scale = 1f)
        //{
        //    yield return new WaitForSeconds(1);

        //    var instance = Instantiate(fishSpottedPrefab);
        //    instance.transform.position = GetFishLocation();
        //    instance.transform.localScale = new Vector2(scale, scale);
        //}

        private Vector2 GetFishLocation()
        {
            //TODO: Implement
            FishLocation = new Vector2(1, 0);
            return FishLocation;
        }
    }
}
