using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.OneManArmy
{
    public class MinigameManager : MonoBehaviour
    {
        public GameObject Player;
        public GameObject HealthImage0_6;
        public GameObject HealthImage1_6;
        public GameObject HealthImage2_6;
        public GameObject HealthImage3_6;
        public GameObject HealthImage4_6;
        public GameObject HealthImage5_6;
        public GameObject HealthImage6_6;

        // Start is called before the first frame update
        void Start()
        {
            HealthImage0_6.SetActive(true);
            HealthImage1_6.SetActive(false);
            HealthImage2_6.SetActive(false);
            HealthImage3_6.SetActive(false);
            HealthImage4_6.SetActive(false);
            HealthImage5_6.SetActive(false);
            HealthImage6_6.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}