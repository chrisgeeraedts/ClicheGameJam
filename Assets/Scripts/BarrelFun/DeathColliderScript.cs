using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BarrelFun
{
    public class DeathColliderScript : MonoBehaviour
    {
        public GameObject MinigameManager;

        void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log(MinigameManager.GetComponent<MinigameManager>());
            MinigameManager.GetComponent<MinigameManager>().DeathHit(col);
        }
    }
}