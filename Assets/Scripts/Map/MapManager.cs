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
        [SerializeField] float xIncrement, yIncrement;
        [SerializeField] GameObject mapParentObject;
        [SerializeField] GameObject mapNodePrefab;

        private bool mapIsGenerated = false;
        private List<int> unusedMinigameInfoIndexes;

        private void Start()
        {
            GenerateMap();
        }

        private void GenerateMap()
        {
            if (mapIsGenerated) return;
            FillUnusedMinigameinfoIndexes();

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    var mapNode = Instantiate(mapNodePrefab, mapParentObject.transform, false);
                    var minigameInfo = GetRandomMinigameInfo();
                    mapNode.GetComponent<MapNode>().SetInfo(minigameInfo, x);
                    mapNode.transform.localPosition = new Vector2(x * xIncrement, y * yIncrement);
                }
            }

            mapIsGenerated = true;
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
    }
}