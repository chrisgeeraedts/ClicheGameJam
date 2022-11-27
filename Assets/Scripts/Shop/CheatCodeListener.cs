using Assets.Scripts.Map;
using System;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    class CheatCodeListener: MonoBehaviour
    {
        private int numberOfCorrectInputs;
        private int numberOfRequiredInputs = 10;
        private bool cheatAchieved = false;
        //Correct code: UP UP DOWN DOWN LEFT RIGHT LEFT RIGHT B A 

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (cheatAchieved) return;

            if (UpInputCorrect() ||
                DownInputCorrect() ||
                LeftInputCorrect() ||
                RightInputCorrect() ||
                AInputCorrect() ||
                BInputCorrect())
            {
                numberOfCorrectInputs++;
                Debug.Log($"Cheat code: {numberOfCorrectInputs}");
            }
            else if(Input.anyKeyDown)
            {
                numberOfCorrectInputs = 0;
            }

            if (numberOfCorrectInputs == numberOfRequiredInputs)
            {
                cheatAchieved = true;
                MapManager.GetInstance().GainCoins(500);
                GlobalAchievementManager.GetInstance().SetAchievementCompleted(24);
            }
        }

        private bool UpInputCorrect()
        {
            if (!Input.GetKeyDown(KeyCode.UpArrow)) return false;
            var correct = numberOfCorrectInputs == 0 || numberOfCorrectInputs == 1;

            return correct;
        }

        private bool DownInputCorrect()
        {
            if (!Input.GetKeyDown(KeyCode.DownArrow) ) return false;
            var correct = numberOfCorrectInputs == 2 || numberOfCorrectInputs == 3;

            return correct;
        }

        private bool LeftInputCorrect()
        {
            if (!Input.GetKeyDown(KeyCode.LeftArrow) ) return false;
            var correct = numberOfCorrectInputs == 4 || numberOfCorrectInputs == 6;

            return correct;
        }

        private bool RightInputCorrect()
        {
            if (!Input.GetKeyDown(KeyCode.RightArrow)) return false;
            var correct = numberOfCorrectInputs == 5 || numberOfCorrectInputs == 7;

            return correct;
        }

        private bool AInputCorrect()
        {
            if (!Input.GetKeyDown(KeyCode.A) ) return false;
            var correct = numberOfCorrectInputs == 9;

            return correct;
        }

        private bool BInputCorrect()
        {
            if (!Input.GetKeyDown(KeyCode.B)) return false;
            var correct = numberOfCorrectInputs == 8;

            return correct;
        }
    }
}
