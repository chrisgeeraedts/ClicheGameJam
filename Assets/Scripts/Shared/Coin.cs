using UnityEngine;

namespace Assets.Scripts.Shared
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] AudioClip coinPickupClip;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag != Constants.TagNames.Player) return;

            Debug.Log("Coin picked up");
            AudioSource.PlayClipAtPoint(coinPickupClip, transform.position);
            //TODO: Keep track of coins
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
