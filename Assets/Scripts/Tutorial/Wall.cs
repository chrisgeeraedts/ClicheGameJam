using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class Wall : MonoBehaviour
    {
        private TutorialManager tutorialManager;
        private void Start()
        {
            tutorialManager = FindObjectOfType<TutorialManager>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (tutorialManager.InvisibleWallsAchievementGot) return;

            tutorialManager.TriggerInvisibleWallsAchievement();
        }
    }
}
