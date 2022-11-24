using Assets.Scripts.FinalBossScene;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class Vines : MonoBehaviour, IEnemy
    {
        public void Damage(float amount)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public string GetEnemyKey()
        {
            return "Vines";
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
