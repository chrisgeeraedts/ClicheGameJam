using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuttyVictory : MonoBehaviour
{


    [SerializeField] private EasyExpandableTextBox Speaking_Textbox;

    // Start is called before the first frame update
    void Start()
    {        
        Speaking_Textbox.Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Turn()
    {
        GetComponent<Animator>().SetTrigger("TurnEvil");
    }

    public void TurnRed()
    {        
        transform.localScale = new Vector3(4,4,4);
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    private bool isShowingSayPopup = false;
    public void Say(string message, float timeBetweenCharacters = 0.125f, bool canSkipText = true, bool waitForButtonClick = true, float timeToWaitAfterTextIsDisplayed = 1f)
    {
        if(!isShowingSayPopup)
        {                
            Debug.Log("Blocking more popups");
            isShowingSayPopup = true;
            Speaking_Textbox.Show(gameObject, 3f);
            StartCoroutine(Speaking_Textbox.EasyMessage(message, timeBetweenCharacters, canSkipText, waitForButtonClick, timeToWaitAfterTextIsDisplayed));
            StartCoroutine(HideSay(message, timeBetweenCharacters, timeToWaitAfterTextIsDisplayed));
        }
    }

    IEnumerator HideSay(string message, float duration, float timeToWaitAfterTextIsDisplayed )
    {
        yield return new WaitForSeconds((duration*message.Length)+timeToWaitAfterTextIsDisplayed); 
        Debug.Log("Can show popups again");
        isShowingSayPopup = false;
        Speaking_Textbox.Hide();
    }
}
