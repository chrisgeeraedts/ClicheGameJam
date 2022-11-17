using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared 
{
    public class DamagingZoneScript : MonoBehaviour, IDamagingZone
    {   
        [SerializeField] private float DamageOnHit;
        public string DamagingZoneKey;

        private void Start()
        {
            DamagingZoneKey = System.Guid.NewGuid().ToString();
        }

        public float GetDamageOnHit()
        {
            return DamageOnHit;
        }

        public string GetZoneKey()
        {
            return DamagingZoneKey;   
        }
    }
}