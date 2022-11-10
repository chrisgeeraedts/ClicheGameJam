using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;

public class MainMenuManager : MonoBehaviour
{
    public Button NewRunButton;

    public GameObject MainMenuHero;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NavigateToMap();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            NavigationToAchievements();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ExitGame();
        }
    }

    public AudioSource buttonClickAudioSource;
    // Start is called before the first frame update
    public void NavigateToMap()
    {
        buttonClickAudioSource.Play();
        MainMenuHero.GetComponent<MainMenuHeroScript>().StartGameAnimation();
        StartCoroutine(InitiateNewMap());
    }

    IEnumerator InitiateNewMap()
    {        
        NewRunButton.interactable = false;
        yield return new WaitForSeconds(3f);        
        NewRunButton.interactable = true;
        GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.StoryScene);
    }

    public void NavigationToAchievements()
    {
        buttonClickAudioSource.Play();
        SceneManager.LoadScene(Constants.SceneNames.AchievementsScene, LoadSceneMode.Additive);
    }

    public void ExitGame()
    {        
        buttonClickAudioSource.Play();
        Application.Quit();
    }
}
