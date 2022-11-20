using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared
{
    public interface IPlayer
    {
        void SetPlayerActive(bool active);
        bool IsPlayerActive();
        GameObject GetGameObject();
    }

    public interface ISpeaker
    {
        void Say(string message, float timeBetweenCharacters = 0.125f, bool canSkipText = true, bool waitForButtonClick = true, float timeToWaitAfterTextIsDisplayed = 1f);
    }
}
