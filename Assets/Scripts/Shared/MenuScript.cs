using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private bool isOpen;
    public GameObject MenuObject;
    public GameObject AchievementsElement;
    public GameObject MinigameCanvas;
    public GameObject PlayerElement;
    // Start is called before the first frame update
    void Start()
    {

    }

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
        //Time.timeScale = 1;
        MenuObject.SetActive(false);
        MinigameCanvas.SetActive(true);
        PlayerElement.GetComponent<Assets.Scripts.OneManArmy.Player>().IsActive = true;
    }

    public void PauseAndOpenMenu()
    {
        isOpen = true;
        //Time.timeScale = 0;
        MenuObject.SetActive(true);
        MinigameCanvas.SetActive(false);
        PlayerElement.GetComponent<Assets.Scripts.OneManArmy.Player>().IsActive = false;
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
