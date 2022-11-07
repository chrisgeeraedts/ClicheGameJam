using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
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
        SceneManager.LoadScene("StoryScene");
    }

    public void NavigationToAchievements()
    {
        buttonClickAudioSource.Play();
        SceneManager.LoadScene("AchievementsScene", LoadSceneMode.Additive);
    }

    public void ExitGame()
    {        
        buttonClickAudioSource.Play();
        Application.Quit();
    }

    
}
