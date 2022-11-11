using UnityEngine;

namespace Assets.Scripts.BarrelFun
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float leftXBound, rightXBound;

        private Rigidbody2D rigidBody;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            DestroyIfOutOfBounds();
        }

        private void DestroyIfOutOfBounds()
        {
            if (rigidBody.position.x < leftXBound ||
                rigidBody.position.x > rightXBound)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
