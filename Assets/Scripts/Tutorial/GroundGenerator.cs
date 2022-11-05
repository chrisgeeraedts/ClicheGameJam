using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Tutorial
{
    public class GroundGenerator : MonoBehaviour
    {
        [SerializeField] GameObject grassTile;
        [SerializeField] List<GameObject> treeTiles;
        [SerializeField] int gridWidth, gridHeight;
        [SerializeField] float xStart, yStart;

        private void Start()
        {
            GenerateTileGrid();
        }

        private void GenerateTileGrid()
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    var prefab = GetRandomPrefab();
                    var instance = Instantiate(prefab, transform, true);
                    instance.transform.position = new Vector2(xStart + x, yStart + y);
                }
            }
        }

        private GameObject GetRandomPrefab()
        {
            var randomInt = Random.Range(0, 100);

            if (randomInt > 30) return grassTile;
            var treeIndex = randomInt % treeTiles.Count;
            return treeTiles[treeIndex];
        }
    }
}
