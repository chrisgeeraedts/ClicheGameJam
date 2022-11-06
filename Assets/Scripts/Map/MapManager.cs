using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Shared.Enums;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] List<MinigameInfo> minigameInfos;
        [SerializeField] int mapWidth, mapHeight;
        [SerializeField] float xIncrement, yIncrement;
        [SerializeField] GameObject mapParentObject;
        [SerializeField] GameObject mapNodePrefab;
        [SerializeField] AudioSource cursorMoved;
        [SerializeField] AudioSource mapSelected;

        private bool isInitialized = false;
        private List<int> unusedMinigameInfoIndexes;
        private int selectedX, selectedY;
        private GameObject[,] minigameGrid;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (isInitialized) return;

            minigameGrid = new GameObject[mapWidth, mapHeight];
            GenerateMap();
            SelectFirstMapNode();

            isInitialized = true;
        }

        private void SelectFirstMapNode()
        {
            minigameGrid[0, 0].GetComponent<MapNode>().SetSelected(true);
        }

        private void Update()
        {
            HandlePlayerInput();
        }

        private void HandlePlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.W)) MoveSelectedMapNode(Direction.Up);
            if (Input.GetKeyDown(KeyCode.A)) MoveSelectedMapNode(Direction.Left);
            if (Input.GetKeyDown(KeyCode.S)) MoveSelectedMapNode(Direction.Down);
            if (Input.GetKeyDown(KeyCode.D)) MoveSelectedMapNode(Direction.Right);
            if (Input.GetKeyDown(KeyCode.Return)) StartSelectedGame();
            if (Input.GetKeyDown(KeyCode.Space)) StartSelectedGame();
        }

        private void StartSelectedGame()
        {
            mapSelected.Play();
            var selectedNode = minigameGrid[selectedX, selectedY].GetComponent<MapNode>();
            SceneManager.LoadScene(selectedNode.MinigameInfo.SceneName);
        }

        private void MoveSelectedMapNode(Direction direction)
        {
            if (MoveIsImpossible(direction)) return;

            SetCurrentMapModeSelected(false);
            ApplyMove(direction);
            SetCurrentMapModeSelected(true);
            cursorMoved.Play();
        }

        private void SetCurrentMapModeSelected(bool isSelected)
        {
            minigameGrid[selectedX, selectedY].GetComponent<MapNode>().SetSelected(isSelected);
        }

        private void ApplyMove(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    selectedY++;
                    break;
                case Direction.Down:
                    selectedY--;
                    break;
                case Direction.Left:
                    selectedX--;
                    break;
                case Direction.Right:
                    selectedX++;
                    break;
                default:
                    throw new Exception($"Unable to handle direction {direction}");
            }
        }

        private bool MoveIsImpossible(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return selectedY >= mapHeight - 1;
                case Direction.Down:
                    return selectedY <= 0;
                case Direction.Left:
                    return selectedX <= 0;
                case Direction.Right:
                    return selectedX >= mapWidth - 1;
                default:
                    throw new Exception($"Unable to handle direction {direction}");
            }
        }

        private void GenerateMap()
        {
            FillUnusedMinigameinfoIndexes();

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    var mapNode = Instantiate(mapNodePrefab, mapParentObject.transform, false);
                    var minigameInfo = GetRandomMinigameInfo();
                    mapNode.GetComponent<MapNode>().SetInfo(minigameInfo, x);
                    mapNode.transform.localPosition = new Vector2(x * xIncrement, y * yIncrement);
                    minigameGrid[x, y] = mapNode;
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
    }
}