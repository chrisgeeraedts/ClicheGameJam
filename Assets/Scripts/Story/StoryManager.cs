using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Map");
        }
    }

    IEnumerator GoToGame()
    {
        yield return new WaitForSeconds(35f);
        SceneManager.LoadScene("Map");
    }
}
