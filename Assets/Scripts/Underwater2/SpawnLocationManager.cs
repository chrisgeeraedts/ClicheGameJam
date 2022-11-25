using Assets.Scripts.Map;
using Assets.Scripts.Shared;
using UnityEngine;

namespace Assets.Scripts.Underwater2
{
    public class SpawnLocationManager : MonoBehaviour
    {
        [SerializeField] Vector2 pierSpawnPosition;

        private void Start()
        {
            if (MapManager.GetInstance().SpawnPlayerAtPierInUnderwater)
            {
                FindObjectOfType<PlayerScript>().transform.position = pierSpawnPosition;
            }
        }
    }
}
