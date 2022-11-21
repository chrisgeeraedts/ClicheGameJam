using UnityEngine;
using TMPro;
using System.Collections;
using Assets.Scripts.Map;

namespace Assets.Scripts.Fishing
{
    public class FishlineAim : MonoBehaviour
    {
        [SerializeField] Transform horizontalAim;
        [SerializeField] Transform verticalAim;
        [SerializeField] float minX, maxX, minY, maxY;
        [SerializeField] float horizontalSpeed, verticalSpeed;
        [SerializeField] TextMeshProUGUI fishCaughtLevelText;
        [SerializeField] HitLocationTester hitLocationTester;
        [SerializeField] float startScale;
        [SerializeField] float shrinkFactor;
        [SerializeField] Vector2 horizontalAimStartPosition;
        [SerializeField] Vector2 verticalAimStartPosition;
        [SerializeField] GameObject explanationParentGameObject;
        [SerializeField] int maximumFishLevelCaught = 9;

        public bool isActive = true;
        public bool HorizontalAimActive = false;
        public bool VerticalAimActive = false;

        private FishSpawner fishSpawner;
        private int horizontalSpeedModifier = 1;
        private int verticalSpeedModifier = 1;
        private int numberOfFishHits;
        private bool spawnerStarted = false;
        private CaughtFish caughtFish;
        private int numberOfCaughtFish;
        private GameObject currentSpawnedFish;

        public void FishHit()
        {
            numberOfFishHits++;
            if (numberOfFishHits >= maximumFishLevelCaught)
            {
                StopFishing();
                return;
            }

            var shrinkScale = Mathf.Pow(shrinkFactor, numberOfFishHits);
            currentSpawnedFish = fishSpawner.SpawnScaledFish(startScale * shrinkScale);
            isActive = true;
            ResetAimPositions();

            HorizontalAimActive = true;
        }

        private void Start()
        {
            fishSpawner = FindObjectOfType<FishSpawner>();
            caughtFish = FindObjectOfType<CaughtFish>();
            caughtFish.HideCatch();
        }

        private void Update()
        {
            //if (!isActive) return;

            HandlePlayerInput();

            MoveHorizontalAim();
            MoveVerticalAim();
        }

        private void ResetAimPositions()
        {
            horizontalAim.position = horizontalAimStartPosition;
            verticalAim.position = verticalAimStartPosition;
        }

        private void HandlePlayerInput()
        {
            if (caughtFish.IsShowing && Input.GetKeyDown(KeyCode.R))
            {
                //Exit, pass number of caught fish to MapManager
                Debug.Log("Exit");
                return;
            }

            if (!Input.GetKeyDown(KeyCode.Space)) return;

            if (HorizontalAimActive)
            {
                HorizontalAimActive = false;
                VerticalAimActive = true;
            }
            else if (VerticalAimActive)
            {
                VerticalAimActive = false;
                SpawnHitTester();
                isActive = false;
            }
            else
            {
                HorizontalAimActive = true;
                caughtFish.HideCatch();
                InitializeSpawner();
            }
        }

        private void InitializeSpawner()
        {
            if (!spawnerStarted)
            {
                explanationParentGameObject.SetActive(false);
                currentSpawnedFish = fishSpawner.SpawnScaledFish(startScale);
                spawnerStarted = true;
            }
        }

        private void SpawnHitTester()
        {
            var location = new Vector2(horizontalAim.position.x, verticalAim.position.y);
            var hitTest = Instantiate(hitLocationTester, location, Quaternion.identity);

            StartCoroutine(CheckHitTest(hitTest));
        }

        private IEnumerator CheckHitTest(HitLocationTester hitTest)
        {
            yield return new WaitForSeconds(1);

            if (hitTest != null && hitTest.isActiveAndEnabled)
            {
                StopFishing();
                hitTest.gameObject.SetActive(false);
                Destroy(hitTest.gameObject);
            }
        }

        private void StopFishing()
        {
            CheckAchievements();
            ResetAimPositions();
            DestroyCurrentFish();
            caughtFish.ShowCatch(numberOfFishHits);
            if (numberOfFishHits > 1 || numberOfFishHits == maximumFishLevelCaught) numberOfCaughtFish++; // 0 and 1 are not fish (Old Boot and Seaweed), Max is Treasure chest
            numberOfFishHits = 0;
        }

        private void CheckAchievements()
        {
            if (numberOfFishHits == 0)
            {
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(31);
            }

            if (numberOfFishHits == maximumFishLevelCaught)
            {
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(32);
                //TODO: Show gaining coins
                MapManager.GetInstance().GainCoins(1000);
            }
        }

        private void DestroyCurrentFish()
        {
            if (currentSpawnedFish == null) return;

            currentSpawnedFish.SetActive(false);
            Destroy(currentSpawnedFish);
            spawnerStarted = false;
        }

        private void MoveHorizontalAim()
        {
            if (!HorizontalAimActive) return;

            var newX = horizontalAim.position.x + horizontalSpeed * horizontalSpeedModifier * Time.deltaTime;
            horizontalAim.position = new Vector2(newX, horizontalAim.position.y);

            if (newX > maxX)
            {
                horizontalSpeedModifier = -1;
            }
            else if (newX < minX)
            {
                horizontalSpeedModifier = 1;
            }
        }

        private void MoveVerticalAim()
        {
            if (!VerticalAimActive) return;

            var newY = verticalAim.position.y + verticalSpeed * verticalSpeedModifier * Time.deltaTime;
            verticalAim.position = new Vector2(verticalAim.position.x, newY);

            if (newY > maxY)
            {
                verticalSpeedModifier = -1;
            }
            else if (newY < minY)
            {
                verticalSpeedModifier = 1;
            }
        }
    }
}
