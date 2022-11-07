using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementItemScript : MonoBehaviour
{
    public TMP_Text TitleTextElement;
    public TMP_Text DescriptionTextElement;
    public TMP_Text LocationTextElement;
    public UnityEngine.UI.Image ImageElement;
    public UnityEngine.UI.Image AchievedNoElement;
    public UnityEngine.UI.Image AchievedYesElement;
    public UnityEngine.UI.Image AchievedBackgroundNoElement;
    public UnityEngine.UI.Image AchievedBackgroundYesElement;

    public void Initialize(Achievement achievement)
    {
        TitleTextElement.text = achievement.Name;
        DescriptionTextElement.text = achievement.Description;
        LocationTextElement.text = achievement.Location;

        if(achievement.Achieved)
        {
            Destroy(AchievedNoElement);
            Destroy(AchievedBackgroundNoElement);
            ImageElement.sprite = (Sprite)Resources.Load<Sprite>(achievement.ImageName + "_YES");
        }
        else
        {            
            Destroy(AchievedYesElement);
            Destroy(AchievedBackgroundYesElement);
            ImageElement.sprite = (Sprite)Resources.Load<Sprite>(achievement.ImageName + "_NO");
        }
    }
}
