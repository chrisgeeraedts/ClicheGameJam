using UnityEngine;
using static Assets.Scripts.Tutorial.TutorialProgress;

namespace Assets.Scripts.Tutorial
{
    public class TutorialTrigger : MonoBehaviour
    {
        [SerializeField] TutorialTriggerType triggerType = TutorialTriggerType.None;

        private void Awake()
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            FindObjectOfType<TutorialProgress>().TriggerTutorial(triggerType);
        }
    }
}
