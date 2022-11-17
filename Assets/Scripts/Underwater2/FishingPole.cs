using Assets.Scripts.Shared;
using UnityEngine;

namespace Assets.Scripts.Underwater2
{
    public class FishingPole : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag != Constants.TagNames.Player) return;

            FindObjectOfType<Player>().SetHasFishingpole(true);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
