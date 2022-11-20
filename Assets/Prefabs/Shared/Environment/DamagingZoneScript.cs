using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared 
{
    public class DamagingZoneScript : MonoBehaviour, IDamagingZone
    {   
        [SerializeField] private float DamageOnHit;
        public string DamagingZoneKey;
        public GameObject[] ObjectsToToggle;

        private void Start()
        {
            DamagingZoneKey = System.Guid.NewGuid().ToString();
            Toggle(false);
        }

        public float GetDamageOnHit()
        {
            if(_toggled)
            {
                return DamageOnHit;
            }
            return 0;
        }

        public string GetZoneKey()
        {
            return DamagingZoneKey;   
        }
        
        private bool _toggled;
        public void Toggle(bool toggled)
        {
            _toggled = toggled;
            for (int i = 0; i < ObjectsToToggle.Length; i++)
            {
                ObjectsToToggle[i].SetActive(_toggled);
            }
        }
    }
}