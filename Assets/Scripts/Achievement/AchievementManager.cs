using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;

public class AchievementManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoBack()
    {
        SceneManager.UnloadSceneAsync(Constants.SceneNames.AchievementsScene);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GoBack();
        }
    }

    void Start()   
    {        
        GlobalAchievementManager.GetInstance().SetAchievementCompleted(33); //boss transformations
    }

    
     //DEBUG - REMOVE
    private int achievement;
    public void GetAchievement()
    {
        
        GlobalAchievementManager.GetInstance().SetAchievementCompleted(achievement);
        achievement++;
    }
}
