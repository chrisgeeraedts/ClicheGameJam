using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Assets.Scripts.Shared;

namespace Assets.Scripts.Map
{
    public class FinishedMinigameInfoXY
    {
        public int X;
        public int Y;
    }

    public class MapManager : MonoBehaviour
    {
        [SerializeField]public int BossDamageWhenMinigameWon;
        [SerializeField]public int HeroDamageWhenMinigameLost;

        public float HeroMaxHP;
        public float HeroHP;

        public float BossMaxHP;
        public float BossHP;


        public bool LastGameWasLost = false;
        public bool LastGameWasWon = false;
        public bool SpawnPlayerAtPierInUnderwater = false;
        public int NumberOfFishInInventory = 0;

        // LOOT
        public bool HasFishingPole = false;
        public bool HasGoldenGun = false;
        public bool HasSillyHat = false;
        // /LOOT

        public bool IsWearingSillyHat = false;

        public float GetHeroHPForFill()
        {
            return (1/HeroMaxHP)*HeroHP;
        }
        public float GetBossHPForFill()
        {
            return (1/BossMaxHP)*BossHP;
        }

        [SerializeField] List<MinigameInfo> minigameInfos;
        [SerializeField] int mapWidth = 4;
        [SerializeField] int mapHeight = 2;

        private List<int> unusedMinigameInfoIndexes;
        private MinigameInfo[,] minigames;
        
        private List<FinishedMinigameInfoXY> FirstFinishedMinigames;

        public void SetMaxStage(int stage)
        {
            maxStageUnlocked = stage;
        }

        private int maxStageUnlocked = 0;
        private static MapManager instance;
        private int minigameStartedX, minigameStartedY;
        private int coins;
        private bool isInitialized = false;

        public void ResetMap()
        {
            Debug.Log("Resetting map");
            instance = null;
            Destroy(gameObject);
        }


        public static MapManager GetInstance()
        {
            return instance;
        }

        public int MaxStageUnlocked => maxStageUnlocked;
        public int MinigameStartedX => minigameStartedX;
        public int MinigameStartedY => minigameStartedY;
        public int Coins => coins;

        public MinigameInfo[,] GetMinigames()
        {
            if (minigames == null)
            {
                minigames = new MinigameInfo[mapWidth, mapHeight];
                GenerateMinigames();
            }

            return minigames;
        }

        public void StartMinigame(int x, int y)
        {
            if (!CanStartGame(x, y)) return;

            minigameStartedX = x;
            minigameStartedY = y;
            if(!isInitialized)
            {
                GetMinigames();
            }
            var currentMinigame = minigames[minigameStartedX, minigameStartedY];
            GameSceneChanger.Instance.ChangeScene(currentMinigame.SceneName);
        }

        public bool CanStartGame(int x, int y)
        {
            return x == maxStageUnlocked; // && !minigames[x, y].IsWon; Allow finished games to be played again
        }

        public void FinishMinigame(bool isWon)
        {
            if(!isInitialized)
            {
                GetMinigames();
            }
            var currentMinigame = minigames[minigameStartedX, minigameStartedY];
            currentMinigame.FinishGame(isWon);

            if (isWon && minigameStartedX >= maxStageUnlocked) 
            {
                maxStageUnlocked = minigameStartedX + 1;
            }

            LastGameWasLost = !isWon;
            LastGameWasWon = isWon;
            if(!isWon)
            {
                HeroHP = HeroHP - HeroDamageWhenMinigameLost;
                Debug.Log("HERO TOOK " + HeroDamageWhenMinigameLost + " damage");
            }
            else
            
            {
                BossHP = BossHP - BossDamageWhenMinigameWon;
                Debug.Log("BOSS TOOK " + BossDamageWhenMinigameWon + " damage");
            }
        }

        public void GainCoins(int numberOfCoins)
        {
            coins += numberOfCoins;
        }

        public bool SpendCoins(int numberOfCoins)
        {
            if (coins < numberOfCoins) return false;

            coins -= numberOfCoins;

            return true;
        }


        public bool HasBininiArmor()
        {
            return _basBikiniArmor;
        }
        
        private bool _basBikiniArmor = false;
        public void BikiniArmorBought()
        {
            _basBikiniArmor = true;
        }

        public void Heal(float healAmount)
        {
            HeroHP = HeroHP + healAmount;
            if(HeroHP > HeroMaxHP)
            {
                HeroHP = HeroMaxHP;
            }
        }
        
        private void GenerateMinigames()
        {
            HeroHP = HeroMaxHP; //Lazy fix for health being 0 after GameOver -> Restart ?
            FillUnusedMinigameinfoIndexes();
            Debug.Log("Generating minigames: " + "width:" + mapWidth + " height:" +  mapHeight);
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    var minigameInfo = GetRandomMinigameInfo();
                Debug.Log("minigame: " + "x:" + x + " y:" +  y);
                    minigames[x, y] = minigameInfo;
                }
            }

            isInitialized = true;
        }

        private MinigameInfo GetRandomMinigameInfo()
        {
            var index = unusedMinigameInfoIndexes[Random.Range(0, unusedMinigameInfoIndexes.Count)];
            var minigameInfo = minigameInfos[index];
            unusedMinigameInfoIndexes.Remove(index);

            if (unusedMinigameInfoIndexes.Count == 0)
            {
                FillUnusedMinigameinfoIndexes();
            }

            return minigameInfo;
        }

        private void FillUnusedMinigameinfoIndexes()
        {
            unusedMinigameInfoIndexes = new List<int>();

            for (int i = 0; i < minigameInfos.Count; i++)
            {
                unusedMinigameInfoIndexes.Add(i);
            }
        }

        private void Awake()
        {
            SetupSingleton();
        }

        public void Exit()
        {
            ResetMap();
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.MainMenuScene);
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
    }
}