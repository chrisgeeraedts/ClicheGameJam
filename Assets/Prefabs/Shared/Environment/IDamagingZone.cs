using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared 
{
    public interface IDamagingZone
    {
        float GetDamageOnHit();
        string GetZoneKey();
        void Toggle(bool toggled);
    }
}
