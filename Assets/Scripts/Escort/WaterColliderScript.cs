using UnityEngine;

namespace Assets.Scripts.Escort
{
    public class WaterColliderScript : MonoBehaviour
    {
        public GameObject MinigameManager;

        void OnTriggerEnter2D(Collider2D col)
        {
            MinigameManager.GetComponent<MinigameManager>().WaterHit(col);
        }
    }
}