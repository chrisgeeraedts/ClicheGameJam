using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private bool isOpen;
    public GameObject MenuObject;
    public GameObject MinigameCanvas;
    public GameObject PlayerElement;
    public AudioSource AudioToPause;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isOpen)
            {
                CloseMenuAndContinue();
            }            
            else
            {
                PauseAndOpenMenu();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            if(isOpen)
            {
                CloseMenuAndContinue();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if(isOpen)
            {
                OpenAchievements();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if(isOpen)
            {
                CloseMenuAndExitRun();
            }
        }
    }

    public void CloseMenuAndContinue()
    {
        isOpen = false; 
        Time.timeScale = 1;
        MenuObject.SetActive(false);
        MinigameCanvas.SetActive(true);
        if (PlayerElement != null)
        {            
            Assets.Scripts.Shared.IPlayer playerScript =             
                PlayerElement.GetComponent(typeof(Assets.Scripts.Shared.IPlayer)) as Assets.Scripts.Shared.IPlayer;
            playerScript.SetPlayerActive(true);
        }
        if(AudioToPause != null)
        {
            AudioToPause.Play();
        }
    }

    public void PauseAndOpenMenu()
    {
        isOpen = true;
        Time.timeScale = 0;
        MenuObject.SetActive(true);
        MinigameCanvas.SetActive(false); 
        if (PlayerElement != null)
        {   
            Assets.Scripts.Shared.IPlayer playerScript =             
                PlayerElement.GetComponent(typeof(Assets.Scripts.Shared.IPlayer)) as Assets.Scripts.Shared.IPlayer;
            playerScript.SetPlayerActive(false);
        } 
        if(AudioToPause != null)
        {
            AudioToPause.Stop();
        }
    }

    public void CloseMenuAndExitRun()
    {
        isOpen = false;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OpenAchievements()
    {       
        SceneManager.LoadScene("AchievementsScene", LoadSceneMode.Additive);
    }
}
