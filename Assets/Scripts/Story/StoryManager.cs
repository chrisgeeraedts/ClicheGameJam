using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;

public class StoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoToGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(GlobalAchievementManager.GetInstance().HasCompletedTutorial())
            {
                GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.MapScene);
            }
            else
            {
                GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.TutorialScene);
            }
        }
    }

    IEnumerator GoToGame()
    {
        yield return new WaitForSeconds(40);
        if(GlobalAchievementManager.GetInstance().HasCompletedTutorial())
        {
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.MapScene);
        }
        else
        {
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.TutorialScene);
        }
    }
}
