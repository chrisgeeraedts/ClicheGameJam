using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Escort
{
    public class WaterColliderScript : MonoBehaviour
    {
        public GameObject MinigameManager;

        void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log(MinigameManager.GetComponent<MinigameManager>());
            MinigameManager.GetComponent<MinigameManager>().WaterHit();
        }
    }
}