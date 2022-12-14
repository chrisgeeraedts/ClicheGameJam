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
        public AudioSource audioSource;
        
        public bool Toggled;

        // Start is called before the first frame update
        void Start()
        {
            if(Toggled)
            {
                Particles.SetActive(false);
                TitleTextElement.enabled = false;
                TitleTextElement.GetComponent<Image>().sprite  = MissionTextSprite;
                 TitleTextElement.GetComponent<Image>().enabled = true;
                GameWinTextElement.enabled = false;
                GameLossTextElement.enabled = false;
            }
        }

        public void ShowTitle()
        {
            if(Toggled)
            {
                TitleTextElement.enabled = true;
                GetComponent<Animator>().SetBool("ShouldShow", true);
                if(audioSource != null)
                {
                    audioSource.time = 0.55f;
                    audioSource.Play();
                }
            }
        }
        public void HideTitle()
        {
            if(Toggled)
            {
                Debug.Log(TitleTextElement.enabled);
                TitleTextElement.enabled = false;
            }
        }
        public void DoWin()
        {
            if(Toggled)
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