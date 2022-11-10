using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts.Shared
{
    public class MissionTextScript : MonoBehaviour
    {
        public Sprite MissionTextSprite;
        public Image TitleTextElement;
        public Image GameWinTextElement;
        public Image GameLossTextElement;
        public GameObject Particles;
        private AudioSource audioSource;
        
        public bool Toggled;

        // Start is called before the first frame update
        void Start()
        {
            if(Toggled)
            {
                Particles.SetActive(false);
                TitleTextElement.enabled = false;
                TitleTextElement.sprite  = MissionTextSprite;
                GameWinTextElement.enabled = false;
                GameLossTextElement.enabled = false;

                audioSource = GetComponent<AudioSource>();
            }
        }

        public void ShowTitle()
        {
            if(Toggled)
            {
                GetComponent<Animator>().SetBool("ShouldShow", true);
                audioSource.time = 0.55f;
                audioSource.Play();
                TitleTextElement.enabled = true;
            }
        }
        public void HideTitle()
        {
            if(Toggled)
            {
                TitleTextElement.enabled = false;
            }
        }
        public void DoWin()
        {
        if(
            Toggled)
            {
                Particles.SetActive(true);
                GameWinTextElement.enabled = true;
            }
            
        }
        public void DoLoss()
        {
            if(Toggled)
            {
                GameLossTextElement.enabled = true;    
            }    
        }
    }
}