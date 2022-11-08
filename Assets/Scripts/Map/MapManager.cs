using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] List<MinigameInfo> minigameInfos;
        [SerializeField] int mapWidth, mapHeight;

        private bool isInitialized = false;
        private List<int> unusedMinigameInfoIndexes;
        private MinigameInfo[,] minigames;
        private static MapManager instance;

        public static MapManager GetInstance()
        {
            return instance;
        }

        public MinigameInfo[,] GetMinigames()
        {
            if (minigames == null)
            {
                minigames = new MinigameInfo[mapWidth, mapHeight];
                GenerateMinigames();
            }

            return minigames;
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

        private void Start()
        {
            SetupSingleton();
            if (!gameObject.activeSelf) return;

            Initialize();
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

        private void Initialize()
        {
            Debug.Log("Initializing");
            if (isInitialized) return;

            isInitialized = true;
            var mapVisualizer = FindObjectOfType<MapVisualizer>();
            if (mapVisualizer != null)
            {
                //TODO: Move MapManager to Menu Scene.
                //MapManager is then instantiated before MapVisualizer and can safely be accessed in MapVisualizer's Start() without need for this hacky workaround
                mapVisualizer.DrawMap();
            }
            
            Debug.Log("initialized");
        }
    }
}