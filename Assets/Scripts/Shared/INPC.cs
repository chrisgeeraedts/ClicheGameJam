using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared
{
    public interface INPC
    {
        void SetNPCActive(bool active);
        bool IsNPCActive();
        
        void SetNPCPaused(bool paused);
        bool IsNPCPaused();
    }
}
