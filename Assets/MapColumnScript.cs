using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapColumnScript : MonoBehaviour
{
    [SerializeField] int Stage = 1;
    [SerializeField] bool Completed = true;

    public GameObject BackgroundElement;
    public GameObject NumberElement;
    public GameObject NumberLightElement;
    public Sprite[] NumberImages;

    private Color completedColor = new Color(0, 255, 0, 255); 
    private Color IncompleteColor = new Color(255, 0, 0, 255); 
    private bool isInitialized;

    public void SetCompletedStage(bool completed)
    {
        // enable light on number
        NumberLightElement.SetActive(completed);

        Debug.Log("Stage "+ Stage +" Current Color: " + BackgroundElement.GetComponent<Image>().color.ToString());
        // change color of background
        if(completed)
        {
            Debug.Log("Stage "+ Stage +" Target Color: " + completedColor.ToString());
            BackgroundElement.GetComponent<Image>().color = completedColor;
        }
        else
        {
            Debug.Log("Stage "+ Stage +" Target Color: " + IncompleteColor.ToString());
            BackgroundElement.GetComponent<Image>().color = IncompleteColor;
        }
        Debug.Log("Stage "+ Stage +" Current Color: " + BackgroundElement.GetComponent<Image>().color.ToString());
    }

    private void SetCorrectStageNumber(int stageNumber)
    {
        NumberElement.GetComponent<Image>().sprite  = NumberImages[stageNumber - 1];
        NumberElement.GetComponent<Image>().enabled = true;

    }

    // Start is called before the first frame update
    void Start()
    {
            SetCorrectStageNumber(Stage);        
            SetCompletedStage(Completed);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInitialized)
        {        
            SetCorrectStageNumber(Stage);        
            SetCompletedStage(Completed);
            isInitialized = true;
        }
    }
    private Color GetColorFromString(string hexColor)
    {
        Color newCol;
        if (ColorUtility.TryParseHtmlString(hexColor, out newCol))
        return newCol;

        return new Color(0,0,0,0);
    }
}
