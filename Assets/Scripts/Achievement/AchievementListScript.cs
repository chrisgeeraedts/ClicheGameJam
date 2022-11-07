using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementListScript : MonoBehaviour
{
    public List<Achievement> Achievements;
    public GameObject AchievementListItemPrefab;
    public TMP_Text CounterTextElement;



    // Start is called before the first frame update
    void Start()
    {
        // Load what achievements we did and did not do (singleton list somewhere?)
        Achievements = GlobalAchievementManager.Instance.GetAllAchievements();    

        int totalAchievements = Achievements.Count;
        int completedAchievements = 0;
        foreach (Achievement achievement in Achievements)
        {
            GameObject newObj = Instantiate(AchievementListItemPrefab);
            newObj.transform.SetParent(this.gameObject.transform, false);
            newObj.GetComponent<AchievementItemScript>().Initialize(achievement);
            if(achievement.Achieved)
            {
                completedAchievements++;
            }
        }

        CounterTextElement.text = "<color=#fede34>" + completedAchievements + "</color> out of <color=#fede34>" + totalAchievements + "</color> completed";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Achievement
{
    public Achievement(int id, string name, string description, string location, bool achieved, string imageName)
    {
        Id = id;
        Name = name;
        Description = description;
        Location = location;
        Achieved = achieved;
        ImageName= imageName;
    }
    public int Id;
    public string Name;
    public string Description;
    public string Location;
    public bool Achieved;
    public string ImageName;
}
