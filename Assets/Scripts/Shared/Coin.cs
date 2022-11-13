using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Shared
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] AudioClip coinPickupClip;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag != Constants.TagNames.Player) return;

            AudioSource.PlayClipAtPoint(coinPickupClip, transform.position);
            MapManager.GetInstance().GainCoins(1);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
