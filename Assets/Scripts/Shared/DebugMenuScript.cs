using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;

public class DebugMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DebugMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("debug menu key pressed");
            ShowDebugMenu();
        }
    }

    public GameObject DebugMenu;
    public void ShowDebugMenu()
    {
        DebugMenu.SetActive(true);
    }

    public void NavigateTo_OneManArmy()
    {
        SceneManager.LoadScene(Constants.SceneNames.OneManArmyScene);
    }
    public void NavigateTo_Escort()
    {
        SceneManager.LoadScene(Constants.SceneNames.EscortScene);
    }
    public void NavigateTo_Underwater()
    {
        SceneManager.LoadScene(Constants.SceneNames.UnderwaterScene);
    }
    public void NavigateTo_Tutorial()
    {
        SceneManager.LoadScene(Constants.SceneNames.TutorialScene);
    }
    public void NavigateTo_Story()
    {
        SceneManager.LoadScene(Constants.SceneNames.StoryScene);
    }
}
