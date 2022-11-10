using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Assets.Scripts.Shared;
using Assets.Scripts.Map;

namespace Assets.Scripts.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] string InitializeText;
        [SerializeField] string PressWText;
        [SerializeField] string PressAText;
        [SerializeField] string PressSText;
        [SerializeField] string PressDText;
        [SerializeField] string CompleteText;
        [SerializeField] string ReturnToMapText;
        [SerializeField] TextMeshProUGUI tutorialTextfield;
        [SerializeField] TextMeshProUGUI errorTextfield;
        [SerializeField] GameObject errorTextPanel;

        private Enums.TutorialState tutorialState = Enums.TutorialState.Initialize;
        private bool invisibleWallsAchievementGot = false;

        public Enums.TutorialState TutorialState
        {
            get { return tutorialState; }
            private set { tutorialState = value; }
        }

        public bool InvisibleWallsAchievementGot
        {
            get { return invisibleWallsAchievementGot; }
        }

        public void TriggerInvisibleWallsAchievement()
        {
            invisibleWallsAchievementGot = true;
            GlobalAchievementManager.GetInstance().SetAchievementCompleted(5);
        }

        public void AdvanceState()
        {
            switch (tutorialState)
            {
                case Enums.TutorialState.Initialize:
                    tutorialState = Enums.TutorialState.PressW;
                    break;
                case Enums.TutorialState.PressW:
                    tutorialState = Enums.TutorialState.PressA;
                    break;
                case Enums.TutorialState.PressA:
                    tutorialState = Enums.TutorialState.PressS;
                    break;
                case Enums.TutorialState.PressS:
                    tutorialState = Enums.TutorialState.PressD;
                    break;
                case Enums.TutorialState.PressD:
                    tutorialState = Enums.TutorialState.Complete;
                    break;
                case Enums.TutorialState.Complete:                   
                    GlobalAchievementManager.GetInstance().SetAchievementCompleted(23);
                    tutorialState = Enums.TutorialState.ReturnToMap;
                    break;
                case Enums.TutorialState.ReturnToMap:
                    MapManager.GetInstance().FinishMinigame(true);
                    SceneManager.LoadScene(Constants.SceneNames.MapScene);
                    break;
                default:
                    break;
            }

            SetCurrentStateText();
        }

        public void HandlePlayerInput(KeyCode key)
        {
            if (IsInputExpectedInput(key))
            {
                AdvanceState();
                errorTextfield.text = string.Empty;
                errorTextPanel.SetActive(false);
            }
            else if (!CanMoveWithKey(key))
            {
                errorTextfield.text = $"Wrong key! {key} was not expected";
                errorTextPanel.SetActive(true);
            }
        }

        public bool CanMoveWithKey(KeyCode key)
        {
            switch (TutorialState)
            {
                case Enums.TutorialState.Initialize:
                    return true;
                case Enums.TutorialState.PressW:
                    return key == KeyCode.W;
                case Enums.TutorialState.PressA:
                    return key == KeyCode.W || key == KeyCode.A;
                case Enums.TutorialState.PressS:
                    return key == KeyCode.W || key == KeyCode.A || key == KeyCode.S;
                case Enums.TutorialState.PressD:
                    return key == KeyCode.W || key == KeyCode.A || key == KeyCode.S || key == KeyCode.D;
                case Enums.TutorialState.Complete:
                    return true;
                case Enums.TutorialState.ReturnToMap:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsInputExpectedInput(KeyCode key)
        {
            switch (tutorialState)
            {
                case Enums.TutorialState.Initialize:
                    return true;
                case Enums.TutorialState.PressW:
                    return key == KeyCode.W;
                case Enums.TutorialState.PressA:
                    return key == KeyCode.A;
                case Enums.TutorialState.PressS:
                    return key == KeyCode.S;
                case Enums.TutorialState.PressD:
                    return key == KeyCode.D;
                case Enums.TutorialState.Complete:
                    return true;
                case Enums.TutorialState.ReturnToMap:
                    return key == KeyCode.Space;
                default:
                    return false;
            }
        }

        private void SetCurrentStateText()
        {
            tutorialTextfield.text = GetCurrentStateText();
        }

        private string GetCurrentStateText()
        {
            switch (tutorialState)
            {
                case Enums.TutorialState.Initialize:
                    return InitializeText;
                case Enums.TutorialState.PressW:
                    return PressWText;
                case Enums.TutorialState.PressA:
                    return PressAText;
                case Enums.TutorialState.PressS:
                    return PressSText;
                case Enums.TutorialState.PressD:
                    return PressDText;
                case Enums.TutorialState.Complete:
                    return CompleteText;
                case Enums.TutorialState.ReturnToMap:
                    return ReturnToMapText;
                default:
                    return "Unknown state";
            }
        }
    }
}