using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Escort
{
    public class MilestoneColliderScript : MonoBehaviour
    {
        public GameObject MinigameManager;
        public int MilestoneId;

        void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("milestone hit: " + MilestoneId);
            Debug.Log(MinigameManager.GetComponent<MinigameManager>());
            MinigameManager.GetComponent<MinigameManager>().MilestoneHit(MilestoneId);
            Destroy(gameObject);
        }
    }
}