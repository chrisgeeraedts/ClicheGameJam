using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared
{
    public class PlayerTargetArrowScript : MonoBehaviour
    {
        private GameObject ObjectToTarget;
        private PlayerScript Player;
        private float MaximumDistanceToShow = 4f;
        private SpriteRenderer m_SpriteRenderer;        
        private bool Toggled;

        public void Setup(PlayerScript player, GameObject objectToTarget, float maximumDistanceToShow)
        {
            Player = player;
            ObjectToTarget = objectToTarget;
            MaximumDistanceToShow = maximumDistanceToShow;
            isReady = true;
        }

        public void UpdateTarget(GameObject objectToTarget)
        {
            ObjectToTarget = objectToTarget;
            isReady = ObjectToTarget != null;
        }

        public void Toggle(bool toggled)
        {
            Toggled = toggled;
        }

        public bool IsToggled()
        {
            return Toggled;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_SpriteRenderer.enabled  = false;
        }
        private bool isReady = false;
        void FixedUpdate()
        {
            if(isReady && Toggled)
            {   
                if(ObjectToTarget != null)
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
}