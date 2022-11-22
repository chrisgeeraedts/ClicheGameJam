using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.FinalBossScene 
{
    public class StationaryBoss : MonoBehaviour, IEnemy
    {
        [SerializeField] private FinalBossStage3Script FinalBossStage3Script;
        string _enemyKey;
        public string GetEnemyKey()
        {
            return _enemyKey;   
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void Damage(float amount) 
        {
            FinalBossStage3Script.AttemptToDamageBoss();
        }
    }
}