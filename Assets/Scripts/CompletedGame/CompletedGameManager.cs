using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Shared;
using Assets.Scripts.Map;
using System;

public class CompletedGameManager : MonoBehaviour
{
    public Button newRunButton;
    public AudioSource buttonClickAudioSource;
    public TuttyVictory TuttyVictory;
    public GameObject TuttyVictoryTarget1;
    public GameObject TuttyVictoryTarget2;
    public GameObject TitleImage;
    public GameObject TitleImageTarget;
    public AudioSource Music;
    public AudioSource BadMusic;
    public GameObject PartyParticles;

    public GameObject Princess;
    public GameObject CapturesPrincess;

    bool _buttonpressed = false;

    public void ExitGame()
    {
        if(!_buttonpressed)
        {
            _buttonpressed = true;
            buttonClickAudioSource.Play();
            newRunButton.interactable = false;
            StartCoroutine(MoveOverSeconds(TitleImage, TitleImageTarget.transform.position, 2));
            if(!GlobalAchievementManager.GetInstance().AreAllAchievementsCompleted())
            {
                PartyParticles.SetActive(false);
                StartCoroutine(TuttySpeechEvil());
                Music.Stop();
                BadMusic.Play();
                FlyCenter();
                TuttyVictory.TurnRed();
                TuttyVictory.Turn();
            }
            else
            {
                StartCoroutine(TuttySpeechGood());
                
                FlyCenter();
            }
        }
    }

    void Start()
    {
        Princess.SetActive(true);
        CapturesPrincess.SetActive(false);
    }

    IEnumerator TuttySpeechEvil()
    {
        yield return new WaitForSeconds(2f); 
        TuttyVictory.Say("Hahaha!", 0.125f, false, false, 2f);
        yield return new WaitForSeconds(4f); 

        Princess.SetActive(false);
        CapturesPrincess.SetActive(true);

        TuttyVictory.Say("You fell for my master plan!", 0.125f, false, false, 2f);
        yield return new WaitForSeconds(6f); 
        TuttyVictory.Say("You will never find all the cliches! Better try again!", 0.125f, false, false, 3f);
        yield return new WaitForSeconds(5f);
        TuttyVictory.Say("Hahaha!", 0.125f, false, false, 2f);
        GlobalAchievementManager.GetInstance().SetAchievementCompleted(17);
        yield return new WaitForSeconds(2f); 
        FlyAway();
        ActuallyExitGame();
    }

    IEnumerator TuttySpeechGood()
    {
        yield return new WaitForSeconds(2f); 
        TuttyVictory.Say($"Amazing, you actually found all the cliches !{Environment.NewLine}You truly are the ultimate CLICHE HUNTER !", 0.075f, false, false, 100f);        
        yield return new WaitForSeconds(5f); 
        FlyAway();
        ActuallyExitGame();
    }
    
    void FlyCenter()
    {
        StartCoroutine(MoveOverSeconds(TuttyVictory.gameObject, TuttyVictoryTarget1.transform.position, 2));
    }

    void FlyAway()
    {
        StartCoroutine(MoveOverSeconds(TuttyVictory.gameObject, TuttyVictoryTarget2.transform.position, 2));
    }

    void ActuallyExitGame()
    {
        
        MapManager.GetInstance().ResetMap();
        GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.CreditsScene);
    }

    IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ExitGame();
        }
    }
}
