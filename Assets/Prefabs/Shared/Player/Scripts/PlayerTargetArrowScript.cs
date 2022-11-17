using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared
{
    public class PlayerTargetArrowScript : MonoBehaviour
    {
        [SerializeField] private GameObject ObjectToTarget;
        [SerializeField] private PlayerScript Player;
        [SerializeField] private float MaximumDistanceToShow = 4f;
        private SpriteRenderer m_SpriteRenderer;

        // Start is called before the first frame update
        void Start()
        {
            isReady = true;
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }
        private bool isReady = false;
        void FixedUpdate()
        {
            if(isReady)
            {   
                float distance = Vector3.Distance (ObjectToTarget.transform.position, Player.GetGameObject().transform.position);
                if(distance > MaximumDistanceToShow)
                {
                    // Show renderer
                    m_SpriteRenderer.enabled = true;

                    Vector3 direction = (ObjectToTarget.transform.position - Player.GetGameObject().transform.position).normalized;    
                    Vector3 neutralDir = transform.up;
                    float angle = Vector3.SignedAngle(neutralDir, direction, Vector3.forward) + 90f;
                    direction = Quaternion.AngleAxis(angle, Vector3.forward) * neutralDir;
                    transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                    transform.position = Player.GetGameObject().transform.position;
                }
                else{
                    m_SpriteRenderer.enabled  = false;
                }
            }
        }
    }
}