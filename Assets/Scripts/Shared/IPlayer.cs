using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared
{
    public interface IPlayer
    {
        void SetPlayerActive(bool active);
        bool IsPlayerActive();
    }
}
