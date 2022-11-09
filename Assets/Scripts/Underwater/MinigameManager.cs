using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Shared;

public class MinigameManager : MonoBehaviour
{

    public GameObject Player;
    private bool Completed = false;

    [SerializeField] Image TitleTextElement;
    [SerializeField] Image GameWinTextElement;
    [SerializeField] Image GameLossTextElement;
    [SerializeField] GameObject AirMeter;
    
    [SerializeField] AudioSource GameMusic;
    [SerializeField] AudioSource DeathMusic;
    [SerializeField] AudioSource WinMusic;

    // Start is called before the first frame update    
    void Start()
    {
        GameWinTextElement.enabled = false;
        GameLossTextElement.enabled = false;            
        StartCoroutine(HideTitle());
        Player.GetComponent<Assets.Scripts.Shared.IPlayer>().SetPlayerActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Completed)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {                    
                Time.timeScale = 1;
                SceneManager.LoadScene(Constants.SceneNames.MapScene);
            }
        }
    }

    private void Win()
    {            
        GameMusic.Stop();
        WinMusic.Play();
        
        GameWinTextElement.enabled = true;
        Completed = true;

        GlobalAchievementManager.GetInstance().SetAchievementCompleted(18); // escort quests
    }

    private void Lose()
    {
        GameMusic.Stop();
        DeathMusic.Play();

        GameLossTextElement.enabled = true;
        Completed = true;            
    }

    IEnumerator HideTitle()
    {
        yield return new WaitForSeconds(5f);
        Destroy(TitleTextElement); 
        Player.GetComponent<Assets.Scripts.Shared.IPlayer>().SetPlayerActive(true);
    }
}
