using Assets.Scripts.Shared;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.BarrelFun
{
    public class FallingPlatform : MonoBehaviour
    {
        [SerializeField] float maxFallingSpeed = -3f;
        [SerializeField] float fallingSpeedIncrease = -0.1f;
        [SerializeField] float fallingDelay = 0.3f;
        [SerializeField] float belowLevelYBound = -10f;
        private bool isFalling = false;
        private bool maxFallingSpeedReached = false;
        private Rigidbody2D rigidBody;
        private CapsuleCollider2D topCollider;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            topCollider = GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            Fall();
            DestroyIfOutOfBounds();
        }

        private void DestroyIfOutOfBounds()
        {
            if (rigidBody.position.y <= belowLevelYBound)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }

        private void Fall()
        {
            if (!isFalling) return;
            if (maxFallingSpeedReached) return;

            GetComponent<Hovering>().enabled = false;
            var newFallingSpeed = rigidBody.velocity.y + (fallingSpeedIncrease * Time.deltaTime);
            rigidBody.velocity = new Vector2(0, newFallingSpeed);

            if (newFallingSpeed <= maxFallingSpeed)
            {
                maxFallingSpeedReached = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag != Constants.TagNames.Player) return;
            if (!topCollider.IsTouchingLayers(LayerMask.GetMask(Constants.LayerNames.Player))) return;
            if (isFalling) return;

            StartCoroutine(TriggerFalling());
        }

        private IEnumerator TriggerFalling()
        {
            yield return new WaitForSeconds(fallingDelay);

            isFalling = true;
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(25);
        }
    }
}
