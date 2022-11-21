using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;

namespace Assets.Scripts.FinalBossScene 
{
    public class RandomNPC_Sidequest : MonoBehaviour, IInteractable
    {
        #region Speaking
        [Header("Speaking Bubbles")]
        [SerializeField] private EasyExpandableTextBox Speaking_Textbox;
        [Space(10)]
        #endregion

        public bool Toggled;
        public AudioSource LeverPulledAudio;
        public GameObject questIcon;


        public void Toggle(bool toggleState)
        {
            Toggled = toggleState;
            InternalToggle();        
        }

        private bool _shownQuest;
        private bool _startedShowingQuest;
        public void ShowInteractibility()
        {
            if(!Toggled && !_startedShowingQuest)
            {
                _startedShowingQuest = true;
                Speaking_Textbox.Show(gameObject, 5f);
                StartCoroutine(DoNPCTalk());             
            }
        }

        IEnumerator DoNPCTalk()
        {     
                StartCoroutine(Speaking_Textbox.EasyMessage("Hey. I know you are saving the world... \n but can you help me deliver these cookies?", 0.1f, false, false, 2f));
            yield return new WaitForSeconds(8f);   
            _shownQuest = true;
            questIcon.SetActive(false);
        }
        
        public void Interact()
        {
            if(!Toggled)
            {
                Debug.Log("Interacting with " + GetObjectName());
                Toggle(true);
            }
        }
        public bool CanShowInteractionDialog()
        {
            return !Toggled && _shownQuest;
        }

        public bool CanInteract()
        {
            return !Toggled;
        }

        public string GetObjectName()
        {
            return "Random NPC with sidequest";
        }

        void Start()
        {
            Speaking_Textbox.Hide();
        }

        void InternalToggle()
        {
            if(Toggled)
            { 
                LeverPulledAudio.Play();                
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(15); //boss transformations
            }
        }
    }
}

