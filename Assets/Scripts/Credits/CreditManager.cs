using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Shared;
using Assets.Scripts.Map;

public class CreditManager : MonoBehaviour
{
    public GameObject CreditsText;
    public GameObject CreditsTextTarget;
    private Vector3 targetPosition;
    public float TimeToSpend;
    public CreditsLightScript[] CreditItemsToHighlight;

    void Start()
    {
        //float additionalMovement = (540 * 2 )+ (540/2); // screen height * 2 (double up) + screenheight / 2 to get the middle;
        
        //Debug.Log(height);
        //float height = ((RectTransform)CreditsText.transform).rect.height;
        Debug.Log(targetPosition);
        StartCoroutine (MoveOverSeconds (CreditsText, TimeToSpend));
        StartCoroutine(GoBackActual());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.MainMenuScene);
        }

        for(int i = 0; i < CreditItemsToHighlight.Length; i++)
        {
            if(!CreditItemsToHighlight[i].Shown && elapsedTime > CreditItemsToHighlight[i].timeStampToShow)
            {
                CreditItemsToHighlight[i].Show();
            }
        }

        //float step = speed * Time.deltaTime;
        //CreditsText.transform.position = Vector3.MoveTowards(CreditsText.transform.position, targetPosition, step);
    }

    IEnumerator GoBackActual()
    {  
        yield return new WaitForSeconds(91f);  // 1 second longer then move time to finish song and read 'Thanks for playing'
        GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.MainMenuScene);
    }

    float elapsedTime = 0;
    public IEnumerator MoveOverSeconds (GameObject objectToMove, float seconds)
    {
        Vector3 startingPos = objectToMove.transform.position;
        Vector3 end = new Vector3(0,0,0);
        while (elapsedTime < seconds)
        {            
            end = CreditsTextTarget.transform.position;
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (float)(elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }
}

    