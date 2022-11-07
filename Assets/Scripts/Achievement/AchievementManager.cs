using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("AchievementsScene");
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
}