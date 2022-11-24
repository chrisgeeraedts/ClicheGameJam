using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;

public class MainMenuManager : MonoBehaviour
{
    public Button NewRunButton;
    public Button AchievementButton;
    public Button ExitButton;
    public GameObject NewAchievementCircle;

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
        else if (Input.GetKeyDown(KeyCode.C))
        {
            NavigationToCredits();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ExitGame();
        }

        NewAchievementCircle.SetActive(GlobalAchievementManager.GetInstance().WasNewAchievementFound());
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
        AchievementButton.interactable = false;
        if(!GlobalAchievementManager.GetInstance().IsAchievementCompleted(33))
        {
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(33); //achievements
            StartCoroutine(GoToAchievements());  
        }            
        else{
            AchievementButton.interactable = true;
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.AchievementsScene, LoadSceneMode.Additive);   
        }
    }

    IEnumerator GoToAchievements()
    {     
        yield return new WaitForSeconds(4f);  
        AchievementButton.interactable = true; 
        GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.AchievementsScene, LoadSceneMode.Additive);  
    }

    public void NavigationToCredits()
    {
        buttonClickAudioSource.Play();
        GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.CreditsScene);
    }

    public void ExitGame()
    {        
        buttonClickAudioSource.Play();
        Debug.Log(Application.platform);
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.WebGLExitScene);
        }
        else
        {
            Application.Quit();
        }
    }

    private int DEBUG_ACHIEVEMENTID;
    public void DEBUGACHIEVEMENT()
    {        
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(DEBUG_ACHIEVEMENTID); 
            DEBUG_ACHIEVEMENTID++;
    }
}
