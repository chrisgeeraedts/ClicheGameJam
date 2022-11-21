using UnityEngine;

namespace Assets.Scripts.Fishing
{
    public class FishSpawner : MonoBehaviour 
    {
        [SerializeField] GameObject fishSpottedPrefab;
        [SerializeField] float fishSpawnMinX, fishSpawnMaxX, fishSpawnMinY, fishSpawnMaxY;

        public Vector2 FishLocation;

        public GameObject SpawnScaledFish(float scale)
        {
            var instance = Instantiate(fishSpottedPrefab);
            instance.transform.position = GetFishLocation();
            instance.transform.localScale = new Vector2(scale, scale);

            return instance;
        }

        private Vector2 GetFishLocation()
        {
            FishLocation = new Vector2(Random.Range(fishSpawnMinX, fishSpawnMaxX), Random.Range(fishSpawnMinY, fishSpawnMaxY));
            return FishLocation;
        }
    }
}
