using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Shared;

public class AchievementListScript : MonoBehaviour
{
    public List<Achievement> Achievements;
    public GameObject AchievementListItemPrefab;
    public TMP_Text CounterTextElement;

    // Start is called before the first frame update
    void Start()
    {
        // Load what achievements we did and did not do (singleton list somewhere?)
        LoadAndShowAchievements();
    }

    void Update() 
    {
        if (GlobalAchievementManager.Instance.AchievementUpdated)
        {
            LoadAndShowAchievements();
            GlobalAchievementManager.Instance.AchievementUpdated = false;
        }
    }

    void LoadAndShowAchievements()
    {
        // Destroy all children objects
        foreach (Transform child in this.gameObject.transform) {
            GameObject.Destroy(child.gameObject);
        }

        // Reload new achievements from Global
        Achievements = GlobalAchievementManager.Instance.GetAllAchievements();    

        // Set UI elements
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
}