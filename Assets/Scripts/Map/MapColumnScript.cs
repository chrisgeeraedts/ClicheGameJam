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
        Completed = completed;
        // enable light on number
        NumberLightElement.SetActive(completed);

        // change color of background
        if(completed)
        {
            BackgroundElement.GetComponent<Image>().color = completedColor;
        }
        else
        {
            BackgroundElement.GetComponent<Image>().color = IncompleteColor;
        }
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
