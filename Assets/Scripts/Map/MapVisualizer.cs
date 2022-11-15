using System;
using System.Collections;
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
        [SerializeField] GameObject DamageTextPrefab;
        [SerializeField] GameObject HeroDamageTextSpawnPoint;
        [SerializeField] GameObject BossDamageTextSpawnPoint;
        [SerializeField] GameObject Hero;
        [SerializeField] GameObject Boss;
        [SerializeField] RuntimeAnimatorController HeroDeathAnimation;
        [SerializeField] AudioSource HeroDeathAudio;
        [SerializeField] AudioSource HeroDamageAudio;
        [SerializeField] AudioSource BossDamageAudio;
        [SerializeField] RuntimeAnimatorController BossIdleAnimation;
        [SerializeField] RuntimeAnimatorController BossSpellcastAnimation;
        [SerializeField] RuntimeAnimatorController BossTakeDamageAnimation;
        [SerializeField] RuntimeAnimatorController HeroIdleAnimation;
        [SerializeField] RuntimeAnimatorController HeroSpellcastAnimation;

        [SerializeField] Image HeroHealthBarElement;
        [SerializeField] Image BossHealthBarElement;
        [SerializeField] TMP_Text HeroHealthTextElement;
        [SerializeField] TMP_Text BossHealthTextElement;

        [SerializeField] int mapWidth, mapHeight;
        [SerializeField] GameObject mapParentObject;
        [SerializeField] GameObject mapNodePrefab;
        [SerializeField] AudioSource cursorMovedSound;
        [SerializeField] AudioSource mapSelectedSound;
        [SerializeField] AudioSource lockedMapSelectedSound;
        [SerializeField] AudioSource BossSpellCast;
        [SerializeField] AudioSource HeroSpellCast;
        [SerializeField] GameObject BossSpellBeam;
        [SerializeField] GameObject HeroSpellBeam;
        
        [SerializeField] AudioSource MusicAudio;
        [SerializeField] AudioSource GameOverAudio;

        public GameObject LineHero_1;
        public GameObject Line1_2;
        public GameObject Line2_3;
        public GameObject Line3_4;
        public GameObject Line4_Boss;

        [SerializeField] MapColumnScript[] Stages;

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
            CheckAlive();
        }

        private void CheckAlive()
        {
            if(MapManager.GetInstance().HeroHP <= 0)
            {
                Debug.Log("WE ARE DEAD");
                // Game over!                
                SetHealthbars();
                DoGameOver();                
            }
        }
        
        private bool allowInput = true;
        public void DoGameOver()
        {
            //Block other input
            allowInput = false;

            // give game over music
            MusicAudio.Stop();
            GameOverAudio.Play();
            HeroDeathAudio.Play();

            // HeroDeathAnimation
            Hero.GetComponent<Animator>().runtimeAnimatorController = HeroDeathAnimation;
            Boss.GetComponent<Animator>().runtimeAnimatorController = BossSpellcastAnimation;
            
            StartCoroutine(StartGameOverActual());
        }

        IEnumerator StartGameOverActual()
        {  
            yield return new WaitForSeconds(2f); 
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.GameOverScene);
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


            // ugly fix to show proper health on re-opening the map
            if(MapManager.GetInstance().LastGameWasLost)
            {         
                // set HP to 1 damage round higher
                MapManager.GetInstance().HeroHP = MapManager.GetInstance().HeroHP + MapManager.GetInstance().HeroDamageWhenMinigameLost;
                HeroHealthBarElement.fillAmount = MapManager.GetInstance().GetHeroHPForFill();
                HeroHealthTextElement.text = string.Format("{0:0}", MapManager.GetInstance().HeroHP) + "/" + MapManager.GetInstance().HeroMaxHP;

                // set HP back to proper
                MapManager.GetInstance().HeroHP = MapManager.GetInstance().HeroHP - MapManager.GetInstance().HeroDamageWhenMinigameLost;
            }
            
            if(MapManager.GetInstance().LastGameWasWon)
            {         
                
                // set HP to 1 damage round higher
                MapManager.GetInstance().BossHP = MapManager.GetInstance().BossHP + MapManager.GetInstance().BossDamageWhenMinigameWon;
                BossHealthBarElement.fillAmount = MapManager.GetInstance().GetBossHPForFill();
                BossHealthTextElement.text = string.Format("{0:0}", MapManager.GetInstance().BossHP) + "/" + MapManager.GetInstance().BossMaxHP;

                // set HP back to proper
                MapManager.GetInstance().HeroHP = MapManager.GetInstance().BossHP - MapManager.GetInstance().BossDamageWhenMinigameWon;
            }
        }

        void TaskOnClick(int x, int y){
                minigameGrid[selectedX, selectedY].GetComponent<MapNode>().DisableButton();
                SetCurrentMapNodeSelected(false);       
                selectedX = x;
                selectedY = y;
                SetCurrentMapNodeSelected(true);
                StartSelectedGame();
        }

        void TaskOnMouseEnter(object sender, MouseEnterEventArgs e){   
                SetCurrentMapNodeSelected(false);       
                selectedX = e.X;
                selectedY = e.Y;
                SetCurrentMapNodeSelected(true);
        }

        private void GenerateMap(MinigameInfo[,] minigames)
        {
            minigameGrid = new GameObject[minigames.GetLength(0), minigames.GetLength(1)];
            var maxStageUnlocked = MapManager.GetInstance().MaxStageUnlocked;

            // generate minigame tiles
            for (int x = 0; x < minigames.GetLength(0); x++)
            {
                for (int y = 0; y < minigames.GetLength(1); y++)
                {
                    var mapNodeGameObject = Instantiate(mapNodePrefab, mapParentObject.transform, false);
                    Button btn = mapNodeGameObject.GetComponent<MapNode>().MinigameButton;

                    int button_x = x;
                    int button_y = y;
		            btn.onClick.AddListener(delegate{TaskOnClick(button_x, button_y);});
		            mapNodeGameObject.GetComponent<MapNode>().OnMouseEntered += TaskOnMouseEnter;
		            //btn.OnPointerEnter.AddListener(delegate{TaskOnMouseEnter(button_x, button_y);});

                    mapNodeGameObject.transform.position =  minigamePositionGrid[minigamePositionGridIndex].transform.position;
                    var minigameInfo = minigames[x,y];
                    var mapNode = mapNodeGameObject.GetComponent<MapNode>();
                    mapNode.X = x;
                    mapNode.Y = y;
                    mapNode.SetLocked(x > maxStageUnlocked);
                    mapNode.SetWon(minigameInfo.IsWon);
                    mapNode.SetInfo(minigameInfo);
                    
                    minigameGrid[x, y] = mapNodeGameObject;
                    minigamePositionGridIndex++;
                }
            }

            // set stage correct
            for (int i = 0; i <= maxStageUnlocked; i++)
            {
                Stages[i].SetCompletedStage(true);
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
            if(allowInput)
            {
                HandlePlayerInput();
                CheckAlive();
                CheckSpellcastBoss();
                CheckSpellcastHero();
            }
        }

        public void DoBossSpelLCast()
        {
            // DEBUG PURPOSES
            MapManager.GetInstance().HeroHP = MapManager.GetInstance().HeroHP - MapManager.GetInstance().HeroDamageWhenMinigameLost;
            MapManager.GetInstance().LastGameWasLost = true;
        }

        public void DoHeroSpelLCast()
        {
            // DEBUG PURPOSES
            MapManager.GetInstance().BossHP = MapManager.GetInstance().BossHP - MapManager.GetInstance().BossDamageWhenMinigameWon;
            MapManager.GetInstance().LastGameWasWon = true;
        }

        public void CheckSpellcastBoss()
        {
            //Debug.Log(MapManager.GetInstance().LastGameWasLost);
            if(MapManager.GetInstance().LastGameWasLost)
            {                    
                MapManager.GetInstance().LastGameWasLost = false;
                StartCoroutine(DoBossAttack());
            }
        }

        IEnumerator DoBossAttack()
        {  
            yield return new WaitForSeconds(1.0f); // wait some time before boss attacks             
            Boss.GetComponent<Animator>().runtimeAnimatorController = BossSpellcastAnimation;      
                   
            StartCoroutine(BossSpellAudio());
            StartCoroutine(ReturnBossIdle());
        }   

        public void CheckSpellcastHero()
        {
            //Debug.Log(MapManager.GetInstance().LastGameWasWon);
            if(MapManager.GetInstance().LastGameWasWon)
            {                    
                MapManager.GetInstance().LastGameWasWon = false;
                StartCoroutine(DoHeroAttack());
            }
        }

        IEnumerator DoHeroAttack()
        {  
            yield return new WaitForSeconds(1.0f); // wait some time before boss attacks 
            Hero.GetComponent<Animator>().runtimeAnimatorController = HeroSpellcastAnimation;      
                   
            StartCoroutine(HeroSpellAudio());
            StartCoroutine(BossTakeDamage());
            StartCoroutine(ReturnBossIdleAfterDamage());
            StartCoroutine(ReturnHeroIdle());
        }  

        IEnumerator BossSpellAudio()
        {  
            yield return new WaitForSeconds(0.5f);    
            BossSpellCast.Play(); 
            SetHealthbars();
            HeroDamageAudio.Play();        
            var damageText = Instantiate(DamageTextPrefab, HeroDamageTextSpawnPoint.transform, false);    
            damageText.GetComponent<DamageNumberScript>().ShowText(MapManager.GetInstance().HeroDamageWhenMinigameLost);
        }      


        IEnumerator ReturnBossIdle()
        {  
            yield return new WaitForSeconds(1.5f);    
            Boss.GetComponent<Animator>().runtimeAnimatorController = BossIdleAnimation;
        }

        IEnumerator HeroSpellAudio()
        {     
            HeroSpellCast.Play(); 
            yield return new WaitForSeconds(0.5f); 
            SetHealthbars();
        }      

        IEnumerator BossTakeDamage()
        {   
            yield return new WaitForSeconds(0.5f); 
            Boss.GetComponent<Animator>().runtimeAnimatorController = BossTakeDamageAnimation;
            BossDamageAudio.Play();
            var damageText = Instantiate(DamageTextPrefab, BossDamageTextSpawnPoint.transform, false);
            damageText.GetComponent<DamageNumberScript>().ShowText(MapManager.GetInstance().BossDamageWhenMinigameWon);
        }
        IEnumerator ReturnBossIdleAfterDamage()
        {  
            yield return new WaitForSeconds(1f);    
            Boss.GetComponent<Animator>().runtimeAnimatorController = BossIdleAnimation;
        }

        IEnumerator ReturnHeroIdle()
        {  
            yield return new WaitForSeconds(0.5f);    
            Hero.GetComponent<Animator>().runtimeAnimatorController = HeroIdleAnimation;
        }

        private void SetHealthbars()
        {
            HeroHealthBarElement.fillAmount = MapManager.GetInstance().GetHeroHPForFill();
            HeroHealthTextElement.text = string.Format("{0:0}", MapManager.GetInstance().HeroHP) + "/" + MapManager.GetInstance().HeroMaxHP;
            BossHealthBarElement.fillAmount = MapManager.GetInstance().GetBossHPForFill();
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
                    return selectedX >= mapWidth - 1 || selectedX >= MapManager.GetInstance().MaxStageUnlocked;
                default:
                    throw new Exception($"Unable to handle direction {direction}");
            }
        }
    }
}
