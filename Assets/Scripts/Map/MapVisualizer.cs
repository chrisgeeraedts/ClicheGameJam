using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Assets.Scripts.Shared.Enums;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;

namespace Assets.Scripts.Map
{
    public class MapVisualizer : MonoBehaviour
    {
        [SerializeField] GameObject HeroHealthBarElement;
        [SerializeField] GameObject BossHealthBarElement;
        [SerializeField] TMP_Text HeroHealthTextElement;
        [SerializeField] TMP_Text BossHealthTextElement;

        [SerializeField] int mapWidth, mapHeight;
        [SerializeField] GameObject mapParentObject;
        [SerializeField] GameObject mapNodePrefab;
        [SerializeField] AudioSource cursorMovedSound;
        [SerializeField] AudioSource mapSelectedSound;
        [SerializeField] AudioSource lockedMapSelectedSound;

        public GameObject LineHero_1;
        public GameObject Line1_2;
        public GameObject Line2_3;
        public GameObject Line3_4;
        public GameObject Line4_Boss;

        private int selectedX, selectedY;
        private int minigamePositionGridIndex;
        public GameObject[] minigamePositionGrid;
        private GameObject[,] minigameGrid;        

        private void Start()
        {
            if (MapManager.GetInstance() == null) return;
            Initialize();
        }

        public void DrawMap()
        {
            Initialize();
        }

        public int GetActiveStage()
        {
            return MapManager.GetInstance().MaxStageUnlocked;
        }


        private void Initialize()
        {
            // Disable lines initially
            //LineHero_1.GetComponent<LineLaserScript>().Hide();
            Line1_2.GetComponent<LineLaserScript>().Hide();
            Line2_3.GetComponent<LineLaserScript>().Hide();
            Line3_4.GetComponent<LineLaserScript>().Hide();
            Line4_Boss.GetComponent<LineLaserScript>().Hide();

            // Disable placeholder gameobjects
            for (int i = 0; i < minigamePositionGrid.Length; i++)
            {
                minigamePositionGrid[i].SetActive(false);
            }

            var minigames = MapManager.GetInstance().GetMinigames();
            GenerateMap(minigames);
            SelectLastActiveNode(); 
        }

        private void GenerateMap(MinigameInfo[,] minigames)
        {
            minigameGrid = new GameObject[minigames.GetLength(0), minigames.GetLength(1)];
            var maxStageUnlocked = MapManager.GetInstance().MaxStageUnlocked;

            for (int x = 0; x < minigames.GetLength(0); x++)
            {
                for (int y = 0; y < minigames.GetLength(1); y++)
                {
                    var mapNodeGameObject = Instantiate(mapNodePrefab, mapParentObject.transform, false);
                    mapNodeGameObject.transform.position =  minigamePositionGrid[minigamePositionGridIndex].transform.position;
                    Debug.Log("Create minigame at " + mapNodeGameObject.transform.localPosition.x + " x " + mapNodeGameObject.transform.localPosition.y);
                    var minigameInfo = minigames[x,y];
                    var mapNode = mapNodeGameObject.GetComponent<MapNode>();
                    mapNode.SetLocked(x > maxStageUnlocked);
                    mapNode.SetWon(minigameInfo.IsWon);
                    mapNode.SetInfo(minigameInfo);
                    
                    minigameGrid[x, y] = mapNodeGameObject;
                    minigamePositionGridIndex++;
                }
            }
        }

        private void SelectLastActiveNode()
        {
            selectedX= MapManager.GetInstance().MinigameStartedX;
            selectedY = MapManager.GetInstance().MinigameStartedY;
            minigameGrid[selectedX, selectedY].GetComponent<MapNode>().SetSelected(true);
        }

        private void Update()
        {
            HandlePlayerInput();
            SetHealthbars();
        }

        private void SetHealthbars()
        {
            HeroHealthBarElement.GetComponent<Image>().fillAmount = MapManager.GetInstance().GetHeroHPForFill();
            HeroHealthTextElement.text = string.Format("{0:0}", MapManager.GetInstance().HeroHP) + "/" + MapManager.GetInstance().HeroMaxHP;
            BossHealthBarElement.GetComponent<Image>().fillAmount = MapManager.GetInstance().GetBossHPForFill();
            BossHealthTextElement.text = string.Format("{0:0}", MapManager.GetInstance().BossHP) + "/" + MapManager.GetInstance().BossMaxHP;
        }

        private void HandlePlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.W)) MoveSelectedMapNode(Direction.Down);
            if (Input.GetKeyDown(KeyCode.A)) MoveSelectedMapNode(Direction.Left);
            if (Input.GetKeyDown(KeyCode.S)) MoveSelectedMapNode(Direction.Up);
            if (Input.GetKeyDown(KeyCode.D)) MoveSelectedMapNode(Direction.Right);
            if (Input.GetKeyDown(KeyCode.Return)) StartSelectedGame();
            if (Input.GetKeyDown(KeyCode.Space)) StartSelectedGame();
        }

        private void StartSelectedGame()
        {
            if (MapManager.GetInstance().CanStartGame(selectedX))
            {
                mapSelectedSound.Play();
                MapManager.GetInstance().StartMinigame(selectedX, selectedY);
            }
            else
            {
                lockedMapSelectedSound.Play();
            }
        }

        private void MoveSelectedMapNode(Direction direction)
        {
            if (MoveIsImpossible(direction)) return;

            SetCurrentMapNodeSelected(false);
            ApplyMove(direction);
            SetCurrentMapNodeSelected(true);
            cursorMovedSound.Play();
        }

        private void SetCurrentMapNodeSelected(bool isSelected)
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
            if(GetActiveStage() == 0)
            {
                LineHero_1.GetComponent<LineLaserScript>().EndGameObject = minigameGrid[selectedX, selectedY];
                LineHero_1.GetComponent<LineLaserScript>().Toggled = true;
                LineHero_1.GetComponent<LineLaserScript>().Show();
            }
            if(GetActiveStage() == 1)
            {
                //Line1_2.GetComponent<LineLaserScript>().StartGameObject = minigameGrid[selectedX, selectedY];
                //Line1_2.GetComponent<LineLaserScript>().EndGameObject = minigameGrid[selectedX, selectedY];
                //Line1_2.GetComponent<LineLaserScript>().Show();
            }
            
        }

        private bool MoveIsImpossible(Direction direction)
        {
            Debug.Log("Active: " + GetActiveStage() + " | " + selectedX + " = " + (selectedX >= GetActiveStage() - 1).ToString());
            switch (direction)
            {
                case Direction.Up:
                    return selectedY >= mapHeight - 1;
                case Direction.Down:
                    return selectedY <= 0;
                case Direction.Left:
                    return selectedX <= 0 || selectedX < GetActiveStage();
                case Direction.Right:
                    return selectedX >= mapWidth - 1 || selectedX >= GetActiveStage() - 1;
                default:
                    throw new Exception($"Unable to handle direction {direction}");
            }
        }

        public void OpenAchievements()
        {
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.AchievementsScene, LoadSceneMode.Additive);
        }
    }
}
