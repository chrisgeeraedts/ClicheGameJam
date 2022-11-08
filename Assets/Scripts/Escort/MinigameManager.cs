using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Escort
{
    public class MinigameManager : MonoBehaviour
    {
        public GameObject Player;
        public GameObject NPC;
        private bool Completed = false;

        [SerializeField] AudioSource GameMusic;
        [SerializeField] AudioSource DeathMusic;
        [SerializeField] AudioSource WinMusic;
        
        [SerializeField] Image TitleTextElement;
        [SerializeField] TMP_Text GameWinTextElement;
        [SerializeField] TMP_Text GameLossTextElement;
        [SerializeField] GameObject FrustrationMeter;


        // Start is called before the first frame update
        void Start()
        {
            GameWinTextElement.enabled = false;
            GameLossTextElement.enabled = false;
            StartCoroutine(HideTitle());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void WaterHit()
        {
            Debug.Log("Water hit!");
            GlobalAchievementManager.Instance.SetAchievementCompleted(12); // heroes cant swim
            Lose();
        }

        public void StartEscortQuest()
        {
        }

        public void MilestoneHit(int milestone)
        {
            Debug.Log("Milestone hit: " + milestone);
            if(milestone == 0)
            {
                // Escort me, dont go to fast!

                FrustrationMeter.GetComponent<ProgressBar>().SetFill(0.1f);
            }
            if(milestone == 1)
            {
                // Watch out, a jump!
                FrustrationMeter.GetComponent<ProgressBar>().SetFill(0.2f);
            }
            if(milestone == 2)
            {
                // Its scary
                FrustrationMeter.GetComponent<ProgressBar>().SetFill(0.4f);
            }
            if(milestone == 3)
            {
                // You're going too fast
                FrustrationMeter.GetComponent<ProgressBar>().SetFill(0.6f);
            }
            if(milestone == 4)
            {
                // Are we there yet?
                FrustrationMeter.GetComponent<ProgressBar>().SetFill(0.9f);
            }
            if(milestone == 5)
            {
                // Is that it?
                FrustrationMeter.GetComponent<ProgressBar>().SetFill(1f);
            }
        }



        private void Win()
        {            
            GameMusic.Stop();
            WinMusic.Play();
            
            GameWinTextElement.enabled = true;
            Completed = true;

            GlobalAchievementManager.Instance.SetAchievementCompleted(2); // escort quests
        }

        private void Lose()
        {
            GameMusic.Stop();
            DeathMusic.Play();

            GameLossTextElement.enabled = true;
            Completed = true;            
        }

        IEnumerator HideTitle()
        {
            yield return new WaitForSeconds(5f);
            Destroy(TitleTextElement);
        }
    }
}