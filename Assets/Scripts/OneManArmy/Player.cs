using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.OneManArmy
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1;

        private Vector2 moveInput;
        private Rigidbody2D rb;
        public AudioSource gunAudio;
        public AudioSource damageTakenAudio;
        public AudioSource deathAudio;
        public GameObject MinigameManager;
        public GameObject ChatBubble;
        private Animator animator;

        private bool _immune = false;


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        public bool IsActive = true;
        private bool isDead = false;
        private bool isCompleted = false;
        private void Update()
        {
            if(!isDead && !isCompleted && IsActive)
            {
                RotateToMouse();
                HandlePlayerInput();
                HandlePlayerAnimations();
            }

            ChatBubble.transform.position = new Vector3(transform.position.x+0.7f, transform.position.y+0.7f, transform.position.z);
        }

        private void HandlePlayerAnimations()
        {
            if(rb.velocity.magnitude > 0)
            {
                animator.SetBool("IsMoving", true);
            }
            else{
                animator.SetBool("IsMoving", false);
            }
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
            if(!isDead && !isCompleted && IsActive)
            {
                //Debug.Log("shoot");
                gunAudio.Play();
                GameObject projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);
                projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            }
        }


        private int damageTaken = 0;
        private int damageMax = 6;

        public void Complete()
        {
            
        }

        public void TakeDamage()
        {          
            if(!_immune)
            {
                damageTaken++;
                if(damageTaken < damageMax)
                {
                    damageTakenAudio.Play();
                    animator.SetBool("IsTakingDamage", true);
                    Debug.Log("I GOT HIT!");
                    MinigameManager.GetComponent<MinigameManager>().PlayerTakenDamage(damageTaken);
                    _immune = true;
                    StartCoroutine(RecoveredFromDamage());
                }
                else
                {
                    deathAudio.Play();
                    Debug.Log("I DIED!");
                    isDead = true;
                    MinigameManager.GetComponent<MinigameManager>().PlayerDied();
                }    
            }
            else
            {
                Debug.Log("I GOT HIT BUT WAS IMMUNE");
            }
        }


        private IEnumerator RecoveredFromDamage()
        {
            yield return new WaitForSeconds(1f);   
            _immune = false;         
        }
    }
}
