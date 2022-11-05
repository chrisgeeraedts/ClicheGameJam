using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.OneManArmy
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1;

        private Vector2 moveInput;
        private Rigidbody2D rb;
        public AudioSource gunAudio;


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            RotateToMouse();
            HandlePlayerInput();
        }

        private void RotateToMouse()
        {
            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition)
                - transform.position;
            dir.z = 0;
    
            Vector3 neutralDir = transform.up;
            float angle = Vector3.SignedAngle(neutralDir, dir, Vector3.forward);
            dir = Quaternion.AngleAxis(angle, Vector3.forward) * neutralDir;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        }

        private void HandlePlayerInput()
        {
            rb.MovePosition(rb.position+moveInput * moveSpeed * Time.fixedDeltaTime);
        }

        private void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        public GameObject bullet;
        public Transform firePoint;
        public float fireForce;
        private void OnFire()
        {
            Debug.Log("shoot");
            gunAudio.Play();
            GameObject projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);
            projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        }


        public void TakeDamage()
        {            
            Debug.Log("I GOT HIT!");
        }
    }
}
