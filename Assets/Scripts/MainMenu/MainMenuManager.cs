using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void NavigateToMap()
    {
        SceneManager.LoadScene("StoryScene");
    }

    public void NavigationToAchievements()
    {
        SceneManager.LoadScene("AchievementsScene");
    }

    public void ExitGame()
    {        
        Application.Quit();
    }

    
}
