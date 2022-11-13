using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Shared;

public class GlobalAchievementManager : MonoBehaviour
{
    public TMP_Text TitleTextElement;
    public TMP_Text DescriptionTextElement;
    public UnityEngine.UI.Image ImageElement;
    public Canvas PopupCanvas;

    public List<Achievement> _achievements;
    private bool isActive = false;
    public bool AchievementUpdated;

    private static GlobalAchievementManager instance;
    private bool isInitialized = false;
    private void Start()
    {
        SetupSingleton();
        if (!gameObject.activeSelf) return;

        Initialize();
    }

    public static GlobalAchievementManager GetInstance()
    {
        return instance;
    }

    private void SetupSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }



    void LoadData()
    {
        _achievements = new List<Achievement>();
        _achievements.Add(new Achievement(0, "Red Barrels", "Isn't it weird that all red barrels in games always explode? Who puts them there?", "Found in [<color=#fede34>Red Barrel Minigame</color>]", false, "Achievement/Barrel"));
        _achievements.Add(new Achievement(1, "One Man Army", "You are always the hero, the big guy, the one man army that can take on legions of enemies.", "Found in [<color=#fede34>One Man Army Minigame</color>]", false, "Achievement/OneManArmy"));
        _achievements.Add(new Achievement(2, "Escort Quests", "You have defeated gods and devils, and here you are escorting a way to slow NPC to a door...", "Found in [<color=#fede34>Escort Minigame</color>]", false, "Achievement/Escort"));
        _achievements.Add(new Achievement(3, "Bikini armor", "So male heroes get awesome armor, and female heroes get... bikini's?", "Found in [<color=#fede34>Bikini Puzzle Minigame</color>]", false, "Achievement/Bikini"));
        _achievements.Add(new Achievement(4, "Killing rats", "A new adventure! Your first assigment: Kill rats... ofcourse!", "Found in [<color=#fede34>RPG Battle Minigame</color>]", false, "Achievement/Rats"));
        _achievements.Add(new Achievement(5, "Illusion of Choice", "You ever get that feeling of being railroaded... like... constantly?", "Found in [<color=#fede34>Tutorial Minigame</color>]", false, "Achievement/Walls"));
        _achievements.Add(new Achievement(6, "Woodchopping Hands", "Hit a tree, get wood, hit more, get more wood", "Found in [<color=#fede34>Wood chopping Minigame</color>]", false, "Achievement/Wood"));
        _achievements.Add(new Achievement(7, "Oblivious Guards", "Guard npc's never see anything important, let alone that massive explosion. Must have been the wind.", "Found in [<color=#fede34>Sneaking Minigame</color>]", false, "Achievement/Guard"));
        _achievements.Add(new Achievement(8, "Eating Apples", "Oh no, i'm almost dead. Let me just eat 30 apples to heal up!", "Found in [<color=#fede34>???</color>]", false, "Achievement/Apple"));
        _achievements.Add(new Achievement(9, "Health Does not matter", "So, I took massive amounts of damage, yet I can still run, shoot and jump like nothing happend?", "Found in [<color=#fede34>One Man Army Minigame</color>]", false, "Achievement/Escort"));
        _achievements.Add(new Achievement(10, "The Evil Monoloque", "The endless jammering of a boss, explaining his plan so you can stop it just in time.", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(11, "Boss Transformations", "You killed the final boss, but oh wait, he transforms into something bigger and stronger", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(12, "Heroes can't swim", "You can beat demons, seduce angels, but water stays your deadliest enemy", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(13, "Infinite Magazins", "How are you not wasting bullets, reloading mid-magazine?", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(14, "Shiny Treasure", "How come these treasure chests with loot always shine brightly? Is there a lamp in there?", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(15, "Side-Quests", "Well, the world is going to be destroyed in a few minutes, so let's so find this girl's missing cat", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(16, "The Evil Laughter", "Is there a training class for evil laughing?", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(17, "Behind the Boss", "Why is there always a boss behind a boss?", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(18, "The water level", "Every game must have a water level, its just not a real game without one!", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(19, "The fishing game", "Dont forget about the mandatory fishing skill in any crafting game!", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(20, "Incredible vendors", "Hi shop owner, what can I get for my [Blade of the God-killing]?", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(21, "Breakable Vazes", "Lets just go around the world and break random pottery.", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(22, "Block Puzzles", "The most cliche puzzle in any game", "Found in [<color=#fede34>???</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(23, "Tutorials", "Why do games still have to teach players how to move? WASD has not changed for decades", "Found in [<color=#fede34>Tutorial</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(24, "KONAMI Code", "[PLACEHOLDER]]", "Found in [<color=#fede34>Tutorial</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(25, "Falling platforms", "[PLACEHOLDER]", "Found in [<color=#fede34>Tutorial</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(26, "Pre-boss savepoint", "[PLACEHOLDER]", "Found in [<color=#fede34>Tutorial</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(27, "Mobile game adds", "[PLACEHOLDER]", "Found in [<color=#fede34>Tutorial</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(28, "Lootboxes", "[PLACEHOLDER]", "Found in [<color=#fede34>Tutorial</color>]", false, "Achievement/PLACEHOLDER"));
        _achievements.Add(new Achievement(29, "Hidden collectibles", "[PLACEHOLDER]", "Found in [<color=#fede34>Barrel Fun</color>]", false, "Achievement/PLACEHOLDER"));
    }

    void Initialize()
    {
        Debug.Log("Initializing achievements");
        if (isInitialized) return;

        isInitialized = true;

        AchievementUpdated = false;
        if(gameObject.GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().enabled = false;
        }
        
        LoadData();

        AchievementsToShow = new List<Achievement>();
        StartCoroutine(ShowAchievementPopups());
        isActive = true;
        Debug.Log("Completed initializing achievements");
    }

    public List<Achievement> GetAllAchievements()
    {
        if(_achievements == null)
        {            
            LoadData();
        }
        return _achievements;
    }

    public void SetAchievementCompleted(int achievementId)
    {
        if(!isActive)
        {
            Debug.LogWarning("GlobalAchievementManager was not initialized - load this scene from the correct starting scene!");
            return;
        }
        else
        {
            // Check if we actually have this achievement
            if (_achievements.Count > achievementId)
            {
                // check if we havent already completed it!
                if(_achievements[achievementId].Achieved == false)
                {
                    AchievementUpdated = true;

                    // Find camera in active Scene to set
                    Camera mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
                    PopupCanvas.worldCamera = mainCamera;

                    // Store achievement data
                    _achievements[achievementId].Achieved = true;

                    // Add to stack to show
                    AchievementsToShow.Add(_achievements[achievementId]);
                }
            }
            else
            {
                Debug.LogWarning("Setting unknown achievement!");
                return;
            }
        }
    }

    IEnumerator FinishAnimation()
    {
        yield return new WaitForSeconds(4f);
        GetComponent<Animator>().SetBool("ShowPopup", false);
    }

    private List<Achievement> AchievementsToShow;

    IEnumerator ShowAchievementPopups()
    {
        while(true)
        {
            if(AchievementsToShow.Count > 0)
            {
                Achievement achievementToShow = AchievementsToShow[0];

                // Prepare popup
                TitleTextElement.text = achievementToShow.Name;
                DescriptionTextElement.text = achievementToShow.Description;
                ImageElement.sprite = (Sprite)Resources.Load<Sprite>(achievementToShow.ImageName + "_YES");

                // Show achievement popup
                GetComponent<Animator>().enabled = true;
                GetComponent<Animator>().SetBool("ShowPopup", true);
                
                GetComponent<AudioSource>().Play();
                StartCoroutine(FinishAnimation());

                AchievementsToShow.Remove(achievementToShow);
                yield return new WaitForSeconds(4f);
            }
            yield return new WaitForSeconds(1f);
        }
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