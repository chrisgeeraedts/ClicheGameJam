using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Shared
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] AudioClip coinPickupClip;
        [SerializeField] bool isHidden = false;

        bool coinFound = false;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(!coinFound)
            {
                coinFound = true;
                Debug.Log("COIN"); 
                if (collision.gameObject.tag != Constants.TagNames.Player) return;

                if (isHidden) GlobalAchievementManager.GetInstance().SetAchievementCompleted(29);

                AudioSource.PlayClipAtPoint(coinPickupClip, transform.position);
                MapManager.GetInstance().GainCoins(1);
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
