using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Shared
{
    public class EnemyDamageNumberScript : MonoBehaviour
    {
        public void ShowText(float damageNumber, bool damage = true)
        { 
            if(damage)
            {                
                GetComponent<TextMeshProUGUI>().text = "-"+damageNumber.ToString();   
            }
            else
            {
                GetComponent<TextMeshProUGUI>().text = damageNumber.ToString();   
            }

            GetComponent<Animator>().SetBool("ShowDamageNumber", true);     
            StartCoroutine(DestroyText());
        }
        
        IEnumerator DestroyText()
        {    
            yield return new WaitForSeconds(0.8f);   
            Destroy(gameObject);
        } 
    }
}