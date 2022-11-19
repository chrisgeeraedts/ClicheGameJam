using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.FinalBossScene
{
    public class MilestoneColliderScript : MonoBehaviour
    {
        public int StageId;

        void Start()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}