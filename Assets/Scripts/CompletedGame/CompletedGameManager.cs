using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Shared;
using Assets.Scripts.Map;

public class CompletedGameManager : MonoBehaviour
{
    public Button newRunButton;
    public Button ExitButton;
    public AudioSource buttonClickAudioSource;

    public void ExitGame()
    {
        buttonClickAudioSource.Play();
        MapManager.GetInstance().ResetMap();
        GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.MainMenuScene);
    }

    public void Credits()
    {
        buttonClickAudioSource.Play();
        MapManager.GetInstance().ResetMap();
        GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.CreditsScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Credits();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ExitGame();
        }
    }
}
