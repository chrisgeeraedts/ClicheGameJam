using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;
 using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public Button NewRunButton;
    public Button AchievementButton;
    public Button CreditsButton;
    public Button ExitButton;
    public GameObject NewAchievementCircle;

    private bool isNavigating = false;

    public GameObject MainMenuHero;
    void Update()
    {
        if (isNavigating) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject sel = EventSystem.current.currentSelectedGameObject;
            if(sel != null)
            {
                if(sel == NewRunButton)
                {                    
                    NavigateToMap();
                }
                else if(sel == AchievementButton)
                {                    
                    NavigationToAchievements();
                }
                else if(sel == CreditsButton)
                {                    
                    NavigationToCredits();
                }
                else if(sel == ExitButton)
                {                    
                    ExitGame();
                }
            }
            
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            isNavigating = true;
            NavigateToMap();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            isNavigating = true;
            NavigationToAchievements();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            isNavigating = true;
            NavigationToCredits();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            isNavigating = true;
            ExitGame();
        }

        NewAchievementCircle.SetActive(GlobalAchievementManager.GetInstance().WasNewAchievementFound());
    }

    void Start()
    {
        NewRunButton.Select();
        EventSystem.current.SetSelectedGameObject(NewRunButton.gameObject);
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
        if (!GlobalAchievementManager.GetInstance().IsAchievementCompleted(33))
        {
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(33); //achievements
            StartCoroutine(GoToAchievements());
        }
        else
        {
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
        if (Application.platform == RuntimePlatform.WebGLPlayer)
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


    public void DEBUGTUTORIAL()
    {
        GlobalAchievementManager.GetInstance().TutorialCompleted();
    }
}
