using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Map
{
    public class FinishedMinigameInfoXY
    {
        public int X;
        public int Y;
    }

    public class MapManager : MonoBehaviour
    {
        [SerializeField] List<MinigameInfo> minigameInfos;
        [SerializeField] int mapWidth, mapHeight;

        private List<int> unusedMinigameInfoIndexes;
        private MinigameInfo[,] minigames;
        
        private List<FinishedMinigameInfoXY> FirstFinishedMinigames;

        private int maxStageUnlocked = 0;
        private static MapManager instance;
        private int minigameStartedX, minigameStartedY;

        public static MapManager GetInstance()
        {
            return instance;
        }

        public int MaxStageUnlocked => maxStageUnlocked;
        public int MinigameStartedX => minigameStartedX;
        public int MinigameStartedY => minigameStartedY;

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
            if(minigames != null)
            {
                var currentMinigame = minigames[minigameStartedX, minigameStartedY];
                SceneManager.LoadScene(currentMinigame.SceneName); //TODO: Unload current scene?
            }
        }

        public bool CanStartGame(int x)
        {
            return x <= maxStageUnlocked;
        }

        public void FinishMinigame(bool isWon)
        {
            var currentMinigame = minigames[minigameStartedX, minigameStartedY];
            currentMinigame.FinishGame(isWon);

            if (isWon && minigameStartedX >= maxStageUnlocked) 
            {
                maxStageUnlocked = minigameStartedX + 1;

                // SAVE THE COMPLETED GAME FOR THE ROUTE SOMEHOW
            }
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