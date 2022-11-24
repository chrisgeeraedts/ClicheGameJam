using Assets.Scripts.FinalBossScene;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class Vines : MonoBehaviour, IEnemy
    {
        public AudioSource destroyedAudio;
        public void Damage(float amount)
        {
            gameObject.SetActive(false);
            destroyedAudio.Play();
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
