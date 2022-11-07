using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAchievementManager : Singleton<GlobalAchievementManager>
{
    public List<Achievement> _achievements;

    void Start()
    {
        // Placeholder list untill singleton structure is ready.
        _achievements = new List<Achievement>();
        _achievements.Add(new Achievement(0, "Red Barrels", "Isn't it weird that all red barrels in games always explode? Who puts them there?", "Found in [<color=#fede34>Red Barrel Minigame</color>]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(1, "One Man Army", "You are always the hero, the big guy, the one man army that can take on legions of enemies.", "Found in [<color=#fede34>One Man Army Minigame</color>]", false, "Achievement/OneManArmy"));
        _achievements.Add(new Achievement(2, "Escort Quests", "You have defeated gods and devils, and here you are escorting a way to slow NPC to a door...", "Found in [<color=#fede34>Escort Minigame</color>]", false, "Achievement/Escort"));
        _achievements.Add(new Achievement(3, "Bikini armor", "So male heroes get awesome armor, and female heroes get... bikini's?", "Found in [<color=#fede34>Bikini Puzzle Minigame</color>]", false, "Achievement/Bikini"));
        _achievements.Add(new Achievement(4, "Killing rats", "A new adventure! Your first assigment: Kill rats... ofcourse!", "Found in [<color=#fede34>RPG Battle Minigame</color>]", false, "Achievement/Rats"));
        _achievements.Add(new Achievement(5, "Invisible walls", "You ever get that feeling of being railroaded... like... constantly?", "Found in [<color=#fede34>Tutorial Minigame</color>]", false, "Achievement/Walls"));
        _achievements.Add(new Achievement(6, "Woodchopping Hands", "Hit a tree, get wood, hit more, get more wood", "Found in [<color=#fede34>Wood chopping Minigame</color>]", false, "Achievement/Wood"));
        _achievements.Add(new Achievement(7, "Oblivious Guards", "Guard npc's never see anything important, let alone that massive explosion. Must have been the wind.", "Found in [<color=#fede34>Sneaking Minigame</color>]", false, "Achievement/Guard"));
        _achievements.Add(new Achievement(8, "Eating Apples", "Oh no, i'm almost dead. Let me just eat 30 apples to heal up!", "Found in [<color=#fede34>???</color>]", false, "Achievement/Apple"));
        _achievements.Add(new Achievement(9, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(10, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(11, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(12, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(13, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(14, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(15, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(16, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(17, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(18, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(19, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(20, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(21, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(22, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(23, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(24, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(25, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(26, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(27, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(28, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(29, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(30, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(31, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(32, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(33, "[PLACEHOLDER]", "[PLACEHOLDER]", "[PLACEHOLDER]", false, "Achievement/Barrel"));
    }

    public List<Achievement> GetAllAchievements()
    {
        return _achievements;
    }

    public void SetAchievementCompleted(int achievementId)
    {
        _achievements[achievementId].Achieved = true;
    }
}



public abstract class Singleton<T> : Singleton where T : MonoBehaviour
{
    #region  Fields
    private static T _instance;
    // ReSharper disable once StaticMemberInGenericType
    private static readonly object Lock = new object();

    [SerializeField]
    private bool _persistent = true;
    #endregion

    #region  Properties
    public static T Instance
    {
        get
        {
            if (Quitting)
            {
                Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                // ReSharper disable once AssignNullToNotNullAttribute
                return null;
            }
            lock (Lock)
            {
                if (_instance != null)
                    return _instance;
                var instances = FindObjectsOfType<T>();
                var count = instances.Length;
                if (count > 0)
                {
                    if (count == 1)
                        return _instance = instances[0];
                    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] There should never be more than one {nameof(Singleton)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                    for (var i = 1; i < instances.Length; i++)
                        Destroy(instances[i]);
                    return _instance = instances[0];
                }

                Debug.Log($"[{nameof(Singleton)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                return _instance = new GameObject($"({nameof(Singleton)}){typeof(T)}")
                           .AddComponent<T>();
            }
        }
    }
    #endregion

    #region  Methods
    private void Awake()
    {
        if (_persistent)
            DontDestroyOnLoad(gameObject);
        OnAwake();
    }

    protected virtual void OnAwake() { }
    #endregion
}

public abstract class Singleton : MonoBehaviour
{
    #region  Properties
    public static bool Quitting { get; private set; }
    #endregion

    #region  Methods
    private void OnApplicationQuit()
    {
        Quitting = true;
    }
    #endregion
}