using Assets.Scripts.Shared;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] AudioClip crateDestroyedClip;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag !=Constants.TagNames.Bullet) return;

            AudioSource.PlayClipAtPoint(crateDestroyedClip, transform.position);

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
