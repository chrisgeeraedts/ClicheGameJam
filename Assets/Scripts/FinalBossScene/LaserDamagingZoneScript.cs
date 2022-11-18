using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Shared;

namespace Assets.Scripts.FinalBossScene 
{
    public class LaserDamagingZoneScript : MonoBehaviour, IDamagingZone
    {   
        [SerializeField] private float DamageOnHit;
        public string DamagingZoneKey;
        public GameObject VisualObject;
        private bool isTurnedOff = false;

        private void Start()
        {
            DamagingZoneKey = System.Guid.NewGuid().ToString();
        }

        public void TurnOff()
        {
            isTurnedOff = true;
            VisualObject.SetActive(false);   
        }

        public void TurnOn()
        {
            isTurnedOff = false;
            VisualObject.SetActive(true);   
        }

        public float GetDamageOnHit()
        {
            if(isTurnedOff)
            {
                return 0;
            }
            else
            {
                return DamageOnHit;
            }
        }

        public string GetZoneKey()
        {
            return DamagingZoneKey;   
        }
    }
}