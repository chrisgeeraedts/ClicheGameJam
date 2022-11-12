using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.OneManArmy
{
    public class Player : MonoBehaviour, Shared.IPlayer
    {
        public AudioSource gunAudio;
        public AudioSource damageTakenAudio;
        public AudioSource deathAudio;
        public GameObject MinigameManager;
        public GameObject ChatBubble;
        public GameObject bullet;
        public Transform firePoint;
        public float fireForce;

        [SerializeField] private float moveSpeed = 1;

        private Rigidbody2D rb;
        private Animator animator;
        private bool _immune = false;
        private bool isDead = false;
        private bool _isActive;
        private int damageTaken = 0;
        private int damageMax = 6;
        private bool gameIsFinished = false;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            animator.SetBool("IsMoving", false);
            SetPlayerActive(true);
        }

        public void SetPlayerActive(bool active)
        {
            _isActive = active;
        }
        public bool IsPlayerActive()
        {
            return _isActive;
        }

        public void SetGameFinished(bool finished)
        {
            gameIsFinished = finished;
        }

        private void Update()
        {
            if (!PlayerCanActivate()) return;

            HandleRotation();
            HandleMovement();
            HandleFire();
            HandlePlayerAnimations();
        }

        private void HandleFire()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
        }

        private bool PlayerCanActivate()
        {
            return !isDead && IsPlayerActive() && !gameIsFinished;
        }

        private void HandlePlayerAnimations()
        {
            if (rb.velocity.magnitude > 0)
            {
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
        }

        private void HandleRotation()
        {
            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition)
                - transform.position;
            dir.z = 0;

            Vector3 neutralDir = transform.up;
            float angle = Vector3.SignedAngle(neutralDir, dir, Vector3.forward);
            dir = Quaternion.AngleAxis(angle, Vector3.forward) * neutralDir;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        }

        private void Fire()
        {
            if (PlayerCanActivate())
            {
                gunAudio.Play();
                GameObject projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);
                projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            }
        }

        public void TakeDamage()
        {
            if (_immune) return;

            damageTaken++;
            if (damageTaken < damageMax)
            {
                damageTakenAudio.Play();
                MinigameManager.GetComponent<MinigameManager>().PlayerTakenDamage(damageTaken);
                _immune = true;
                StartCoroutine(RecoveredFromDamage());
            }
            else
            {
                deathAudio.Play();
                isDead = true;
                MinigameManager.GetComponent<MinigameManager>().PlayerDied();
            }
        }

        private IEnumerator RecoveredFromDamage()
        {
            yield return new WaitForSeconds(1f);
            _immune = false;
        }
    }
}
