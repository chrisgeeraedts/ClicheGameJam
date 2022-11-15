using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] int mapHeight = 3;

        private List<int> unusedMinigameInfoIndexes;
        private MinigameInfo[,] minigames;
        
        private List<FinishedMinigameInfoXY> FirstFinishedMinigames;

        private int maxStageUnlocked = 0;
        private static MapManager instance;
        private int minigameStartedX, minigameStartedY;
        private int coins;
        private bool isInitialized = false;

        public void ResetMap()
        {
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
            if (!CanStartGame(x)) return;

            minigameStartedX = x;
            minigameStartedY = y;
            if(!isInitialized)
            {
                GetMinigames();
            }
            var currentMinigame = minigames[minigameStartedX, minigameStartedY];
            GameSceneChanger.Instance.ChangeScene(currentMinigame.SceneName);
        }

        public bool CanStartGame(int x)
        {
            return x <= maxStageUnlocked;
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

            Debug.Log($"Player now has {coins} coins");
        }

        public bool SpendCoins(int numberOfCoins)
        {
            if (coins < numberOfCoins) return false;

            coins -= numberOfCoins;
            return true;
        }

        private void GenerateMinigames()
        {
            FillUnusedMinigameinfoIndexes();

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    var minigameInfo = GetRandomMinigameInfo();
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