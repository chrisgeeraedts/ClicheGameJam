using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Shared;
using Assets.Scripts.Map;

public class GameOverManager : MonoBehaviour
{
    public BloomCameraRaiserScript BloomCamera;
    public Button newRunButton;
    public Button ExitButton;
    public AudioSource buttonClickAudioSource;
    public AudioSource newRunAudioSource;
    public AudioSource[] audioToPause;

    public void StartNewGame()
    {
        buttonClickAudioSource.Play();
        // run bloom up to 99999
        BloomCamera.StartBloom();

        for(int i = 0; i < audioToPause.Length; i++)
        {
            audioToPause[i].Stop();
        }

        newRunAudioSource.Play();
        StartCoroutine(StartNewGameActual());
    }

    IEnumerator StartNewGameActual()
    {    
        newRunButton.interactable = false;
        yield return new WaitForSeconds(5f);        
        newRunButton.interactable = true;
        GlobalAchievementManager.GetInstance().SetAchievementCompleted(30);
        MapManager.GetInstance().ResetMap();
        GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.MainMenuScene);
    }

    public void ExitGame()
    {
        buttonClickAudioSource.Play();
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartNewGame();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ExitGame();
        }
    }
}
