using Assets.Scripts.Shared;
using UnityEngine;

namespace Assets.Scripts.Fishing
{
    public class HitLocationTester : MonoBehaviour
    {
        public bool FishHit = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag != Constants.TagNames.Fish) return;

            Debug.Log("Fish hit !");
            FishHit = true;

            FindObjectOfType<FishlineAim>().FishHit();

            var fish = collision.gameObject;
            fish.SetActive(false);
            Destroy(fish);

            gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }
}
