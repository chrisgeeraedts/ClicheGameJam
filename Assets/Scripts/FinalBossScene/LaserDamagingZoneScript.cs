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
        public GameObject Blocker;
        private bool isTurnedOff = false;

        private void Start()
        {
            DamagingZoneKey = System.Guid.NewGuid().ToString();
            GetComponent<SpriteRenderer>().enabled = false;
            if(Blocker != null)
            {
                Blocker.GetComponent<SpriteRenderer>().enabled = false;
            }
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

        
        private bool _toggled;
        public void Toggle(bool toggled)
        {
            _toggled = toggled;
            if(_toggled)
            {
                TurnOn();
            }
            if(!_toggled)
            {
                TurnOff();
            }
        }
    }
}