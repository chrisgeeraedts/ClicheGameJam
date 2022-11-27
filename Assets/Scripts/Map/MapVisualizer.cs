using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Assets.Scripts.Shared.Enums;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;
using Cinemachine;
using System.Linq;

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

        public Canvas backgroundToHideOnFinalBoss;
        public Image titleToHideOnFinalBoss;
        
        [SerializeField] GameObject ExitButton;
        [SerializeField] GameObject Portal;
        [SerializeField] GameObject TeleportTarget;
        [SerializeField] AudioSource PortalShowingUp;
        
        [SerializeField] private EasyExpandableTextBox BossTextBox;
        [SerializeField] private EasyExpandableTextBox PlayerTextBox;
        [SerializeField] private GameObject BossTextBoxTarget;
        [SerializeField] private GameObject PlayerTextBoxTarget;
        [SerializeField] CinemachineVirtualCamera RegularCamera;
        [SerializeField] CinemachineVirtualCamera PortalCamera;
        [SerializeField] CinemachineCameraShake CinemachineCameraShake;
        [SerializeField] Camera MainCamera;
        [SerializeField] AudioSource GoingIntoTeleport;
        [SerializeField] GameObject CursorFollower;

        [SerializeField] MapColumnScript[] Stages;

        private int selectedX, selectedY;
        private int minigamePositionGridIndex;
        public GameObject[] minigamePositionGrid;
        private GameObject[,] minigameGrid;        

        
        [SerializeField] GameObject ProgressLaserHero;
        [SerializeField] GameObject ProgressLaserBos;
        [SerializeField] GameObject CenterPointProgressLaser;
        [SerializeField] ProgressCutter CenterPointProgressCutter;
        [SerializeField] GameObject[] CenterPointProgressLaserStages;
        [SerializeField] GameObject[] CenterPointProgressLaserStagesEnd;

        [SerializeField] GameObject[] ToHideOnBossPhase;
        [SerializeField] GameObject HeroStartLineObject;
        
        [SerializeField] LineRenderer ProgressCutter;

        private void Start()
        {
            BossTextBox.Hide();
            PlayerTextBox.Hide();
            
            RegularCamera.enabled = true;
            PortalCamera.enabled = false;

            if (MapManager.GetInstance() == null) return;
            Debug.Log(MapManager.GetInstance().MaxStageUnlocked);

            if(MapManager.GetInstance().MaxStageUnlocked < 4)
            {
                Initialize();
            }
        }


        private void SetLaserPoint(int stage)
        {
            GameObject target = CenterPointProgressLaserStages[stage];
            CenterPointProgressLaser.transform.position = target.transform.position;
            CenterPointProgressCutter.StartLocationGameObject = CenterPointProgressLaserStages[stage];
            CenterPointProgressCutter.EndLocationGameObject = CenterPointProgressLaserStagesEnd[stage];

        }

        public void DrawMap()
        {
            if(MapManager.GetInstance() != null && MapManager.GetInstance().MaxStageUnlocked < 4)
            {
                Initialize();
                CheckAlive();
                CheckBossReady();
            }
        }

        private void CheckAlive()
        {
            if(MapManager.GetInstance() != null && MapManager.GetInstance().HeroHP <= 0)
            {
                Debug.Log("WE ARE DEAD");
                // Game over!                
                SetHealthbars();
                DoGameOver();                
            }
        }

        private bool bossPrepared = false;
        private bool bossAnimationCanGo = false;
        private void CheckBossReady()
        {
            if(MapManager.GetInstance() != null && MapManager.GetInstance().MaxStageUnlocked >= 4 && !bossPrepared)
            {
                bossPrepared = true;
                Debug.Log("Boss is ready");
                // can go to final boss now
                for (int i = 0; i < minigamePositionGrid.Length; i++)
                {
                    minigamePositionGrid[i].SetActive(false);
                }

                for (int i = 0; i < ToHideOnBossPhase.Length; i++)
                {
                    ToHideOnBossPhase[i].SetActive(false);
                }

                backgroundToHideOnFinalBoss.enabled = false;
                titleToHideOnFinalBoss.enabled = false;    
                bossAnimationCanGo = true;        
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
            
            StartCoroutine(StartGameOverActual());
        }

        IEnumerator StartGameOverActual()
        {  
            yield return new WaitForSeconds(2f);  
            // HeroDeathAnimation
            HeroDeathAudio.Play();
            Hero.GetComponent<Animator>().runtimeAnimatorController = HeroDeathAnimation;
            Boss.GetComponent<Animator>().runtimeAnimatorController = BossSpellcastAnimation;
            yield return new WaitForSeconds(1f);  
            ShowGameoverScreen();
        }  
  
        private void ShowGameoverScreen()
        {  
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.GameOverScene);
        }  
        
        private void TaskOnClick(){
		    MapManager.GetInstance().Exit();
	    }

        private void Initialize()
        {
            Button btn = ExitButton.GetComponent<Button>();
		    btn.onClick.AddListener(TaskOnClick);

            // Disable lines initially
            for (int i = 0; i < Lines.Length; i++)
            {                
                Lines[i].Hide();
            }

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
                MapManager.GetInstance().BossHP = MapManager.GetInstance().BossHP - MapManager.GetInstance().BossDamageWhenMinigameWon;
            }
        }


        void TaskOnClick(int x, int y){
            if(MapManager.GetInstance().CanStartGame(x, y))
            {
                minigameGrid[selectedX, selectedY].GetComponent<MapNode>().DisableButton();     
                SetCurrentMapNodeSelected(false);       
                selectedX = x;
                selectedY = y;
                SetCurrentMapNodeSelected(true);
                StartSelectedGame();
            }
        }

        void TaskOnMouseEnter(object sender, MouseEnterEventArgs e){   

            if(MapManager.GetInstance() != null && MapManager.GetInstance().CanStartGame(e.X, e.Y))
            {
                SetCurrentMapNodeSelected(false);       
                selectedX = e.X;
                selectedY = e.Y;
                SetCurrentMapNodeSelected(true);
            }
        }

        public LineLaserScript[] Lines;
        private List<GameObject> WonMiniGameObjects;

        private void GenerateMap(MinigameInfo[,] minigames)
        {
            WonMiniGameObjects = new List<GameObject>();
            minigameGrid = new GameObject[minigames.GetLength(0), minigames.GetLength(1)];
            var maxStageUnlocked = MapManager.GetInstance().MaxStageUnlocked;

            // generate minigame tiles
            for (int x = 0; x < minigames.GetLength(0); x++)
            {
                GameObject minigameObjectWonThisX = null;
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
                    mapNode.X = x;
                    mapNode.ParentObject = mapNodeGameObject;
                    mapNode.SetLocked(x > maxStageUnlocked);
                    mapNode.SetWon(minigameInfo.IsWon);


                    FinishedMinigameInfoXY foo = MapManager.GetInstance().FirstFinishedMinigames.FirstOrDefault(a => a.X == x && a.Y == y);
                    if(foo != null)
                    {
                        minigameObjectWonThisX = mapNodeGameObject;
                    }

                    mapNode.SetInfo(minigameInfo);
                    
                    minigameGrid[x, y] = mapNodeGameObject;
                    minigamePositionGridIndex++;
                }

                if(minigameObjectWonThisX != null)
                {
                    WonMiniGameObjects.Add(minigameObjectWonThisX);
                }
            }

            // set highlighting of stages
            for (int i = 0; i < MapManager.GetInstance().MaxStageUnlocked; i++)
            {
                Stages[i].SetCompletedStage(true);
            }

            // draw lines
            int index = 0;
            GameObject prevWonGame = null;
            Debug.Log("FirstFinishedMinigames: " + MapManager.GetInstance().FirstFinishedMinigames.Count);
            foreach (var firstFinishedMinigame in MapManager.GetInstance().FirstFinishedMinigames)
            {
                GameObject wonGame = minigameGrid[firstFinishedMinigame.X, firstFinishedMinigame.Y];

                if(index == 0)
                {
                    Lines[0].Show();
                    Lines[0].StartGameObject = HeroStartLineObject;
                    Lines[0].EndGameObject = wonGame;  
                }
                else if(index > 0)
                {
                    Lines[index].Show();
                    Lines[index].StartGameObject = prevWonGame;
                    Lines[index].EndGameObject = wonGame;  
                }   
                prevWonGame = wonGame;
                index++;
            }


            SetLaserPoint(MapManager.GetInstance().MaxStageUnlocked);
        }

        private void SelectLastActiveNode()
        {
            selectedX= MapManager.GetInstance().MaxStageUnlocked;
            selectedY = 0;
            minigameGrid[selectedX, selectedY].GetComponent<MapNode>().SetSelected(true);
        }

        public void ExitGame()
        {
            MapManager.GetInstance().ResetMap();
            GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.MainMenuScene);
        }

        private int bossAnimationStage = 0;
        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                ExitGame();
            }

            if(!bossAnimationCanGo)
            {
                if(allowInput)
                {
                    HandlePlayerInput();
                    CheckAlive();
                    CheckBossReady();
                    CheckSpellcastBoss();
                    CheckSpellcastHero();
                }
            }
            else
            {
                if(bossAnimationStage == 0)
                {
                    PortalShowingUp.Play();
                    StartShaking();
                    StartCoroutine(MovePortalInView(Portal, TeleportTarget.transform.position, 5f));
                    MoveToStage(1);
                }
                if(bossAnimationStage == 1)
                {
                    // wait
                    StartCoroutine(WaitFor(5f, 3));
                    MoveToStage(2);
                }
                if(bossAnimationStage == 2)
                {
                    // waiting
                }
                if(bossAnimationStage == 3)
                {                    
                    PortalShowingUp.Stop();
                    MoveToStage(4);
                }
                if(bossAnimationStage == 4)
                {                    
                    BossSpeak();
                    MoveToStage(5);
                }
                if(bossAnimationStage == 5)
                {  
                    // waiting
                }
                if(bossAnimationStage == 6)
                {                            
                    PlayerSpeak(); 
                    MoveToStage(7);         
                }
                if(bossAnimationStage == 7)
                {  
                    // waiting
                }
                if(bossAnimationStage == 8)
                { 
                    CursorFollower.SetActive(false);
                    RegularCamera.enabled = false;
                    PortalCamera.enabled = true;
                    GoingIntoTeleport.Play();
                    StartCoroutine(WaitFor(1f, 10));
                    MoveToStage(9);  
                }
                if(bossAnimationStage == 9)
                {  
                    // waiting
                    
                }
                if(bossAnimationStage == 10)
                {  
                    // move scene
                    GameSceneChanger.Instance.ChangeScene(Constants.SceneNames.FinalBossFightScene);
                    MoveToStage(11);  
                    
                }
                if(bossAnimationStage == 11)
                {  
                    // waiting                    
                }
            }
        }

        void MoveToStage(int stage)
        {
            bossAnimationStage = stage;
        }


        void StartShaking()
        {
            CinemachineCameraShake.ShakeCamera(2f, 100f);
        }

        void BossSpeak()
        {        
            BossTextBox.Show(BossTextBoxTarget, 3f);
            StartCoroutine(BossTextBox.EasyMessage("The time has come!", 0.1f, false, false, 3f)); 
            StartCoroutine(WaitFor(4f, 6));
        }

        void PlayerSpeak()
        {        
            PlayerTextBox.Show(PlayerTextBoxTarget, 3f);
            StartCoroutine(PlayerTextBox.EasyMessage("I will stop you!", 0.1f, false, false, 3f)); 
            StartCoroutine(WaitFor(4f, 8));
        }

        IEnumerator WaitFor(float waitTime, int nextStage)
        {        
            yield return new WaitForSeconds(waitTime);  
            MoveToStage(nextStage);  
        }


         IEnumerator MovePortalInView(GameObject objectToMove, Vector3 end, float seconds)
        {     
            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;
            while (elapsedTime < seconds)
            {
                objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.position = end;
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
            if(MapManager.GetInstance() != null && MapManager.GetInstance().LastGameWasLost)
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
            if(MapManager.GetInstance() != null && MapManager.GetInstance().LastGameWasWon)
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
            if (MapManager.GetInstance().CanStartGame(selectedX, selectedY))
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

        public void DEBUG_GOTOFINALSTAGE()
        {
            bossPrepared = false;
            MapManager.GetInstance().SetMaxStage(4);

            bossPrepared = true;
            Debug.Log("Boss is ready");
            // can go to final boss now
            for (int i = 0; i < minigamePositionGrid.Length; i++)
            {
                minigamePositionGrid[i].SetActive(false);
            }

            for (int x = 0; x < minigameGrid.GetLength(0); x++)
            {
                for (int y = 0; y < minigameGrid.GetLength(1); y++)
                {
                    minigameGrid[x, y].SetActive(false);
                }
            }

            backgroundToHideOnFinalBoss.enabled = false;
            titleToHideOnFinalBoss.enabled = false;    
            bossAnimationCanGo = true;        


            bossAnimationCanGo = true;
        }

        

        private void SetCurrentMapNodeSelected(bool isSelected)
        {
            Debug.Log("SetCurrentMapNodeSelected - isSelected: " + isSelected);
            minigameGrid[selectedX, selectedY].GetComponent<MapNode>().SetSelected(isSelected);
            
            if(isSelected)
            {
                Debug.Log("MaxStageUnlocked " + MapManager.GetInstance().MaxStageUnlocked);
                Debug.Log("selectedX " + selectedX);
                Debug.Log("position: " + minigameGrid[selectedX, selectedY].transform.position);

                Lines[MapManager.GetInstance().MaxStageUnlocked].Show();
                Lines[MapManager.GetInstance().MaxStageUnlocked].EndGameObject = minigameGrid[selectedX, selectedY];
            }
            else 
            {
                if(MapManager.GetInstance().MaxStageUnlocked == 0)
                {
                    Lines[MapManager.GetInstance().MaxStageUnlocked].StartGameObject = HeroStartLineObject;
                }
                else
                {
                    Lines[MapManager.GetInstance().MaxStageUnlocked].StartGameObject = minigameGrid[selectedX, selectedY];
                }
                
            }
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
                    if(selectedX == 0)
                    {
                        return true;
                    }
                    return selectedY >= mapHeight - 1;
                case Direction.Down:
                    if(selectedX == 0)
                    {
                        return true;
                    }
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
