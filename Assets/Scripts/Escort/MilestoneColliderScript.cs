using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Escort
{
    public class MilestoneColliderScript : MonoBehaviour
    {
        public GameObject MinigameManager;
        public int MilestoneId;
        public string[] TriggersOn;

        void Start()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            for(int i = 0; i < TriggersOn.Length; i++)
            {
                if(col.gameObject.tag == TriggersOn[i])
                {
                    Debug.Log("milestone hit: " + MilestoneId);
                    Debug.Log(MinigameManager.GetComponent<MinigameManager>());
                    MinigameManager.GetComponent<MinigameManager>().MilestoneHit(MilestoneId, col);
                    Destroy(gameObject);
                }
            }
        }
    }
}