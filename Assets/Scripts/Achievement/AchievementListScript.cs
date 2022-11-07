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

        // Placeholder list untill singleton structure is ready.
        Achievements = new List<Achievement>();
            Achievements.Add(new Achievement("Red Barrels", "Isn't it weird that all red barrels in games always explode? Who puts them there?", "Found in [<color=#fede34>Red Barrel Minigame</color>]", false, "Achievement/Barrel"));
            Achievements.Add(new Achievement("One Man Army", "You are always the hero, the big guy, the one man army that can take on legions of enemies.", "Found in [<color=#fede34>One Man Army Minigame</color>]", true, "Achievement/OneManArmy"));
            Achievements.Add(new Achievement("Escort Quests", "You have defeated gods and devils, and here you are escorting a way to slow NPC to a door...", "Found in [<color=#fede34>Escort Minigame</color>]", true, "Achievement/Escort"));
            Achievements.Add(new Achievement("Bikini armor", "So male heroes get awesome armor, and female heroes get... bikini's?", "Found in [<color=#fede34>Bikini Puzzle Minigame</color>]", false, "Achievement/Bikini"));
            Achievements.Add(new Achievement("Killing rats", "A new adventure! Your first assigment: Kill rats... ofcourse!", "Found in [<color=#fede34>RPG Battle Minigame</color>]", false, "Achievement/Rats"));
            Achievements.Add(new Achievement("Invisible walls", "You ever get that feeling of being railroaded... like... constantly?", "Found in [<color=#fede34>Tutorial Minigame</color>]", true, "Achievement/Walls"));
            Achievements.Add(new Achievement("Woodchopping Hands", "Hit a tree, get wood, hit more, get more wood", "Found in [<color=#fede34>Wood chopping Minigame</color>]", true, "Achievement/Wood"));
            Achievements.Add(new Achievement("Oblivious Guards", "Guard npc's never see anything important, let alone that massive explosion. Must have been the wind.", "Found in [<color=#fede34>Sneaking Minigame</color>]", true, "Achievement/Guard"));
            Achievements.Add(new Achievement("Eating Apples", "Oh no, i'm almost dead. Let me just eat 30 apples to heal up!", "Found in [<color=#fede34>???</color>]", true, "Achievement/Apple"));

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
    public Achievement(string name, string description, string location, bool achieved, string imageName)
    {
        Name = name;
        Description = description;
        Location = location;
        Achieved = achieved;
        ImageName= imageName;
    }
    public string Name;
    public string Description;
    public string Location;
    public bool Achieved;
    public string ImageName;
}
