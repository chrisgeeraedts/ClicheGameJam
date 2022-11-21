using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealingNumberScript : MonoBehaviour
{
    public void ShowText(int damageNumber)
    { 
        GetComponent<TextMeshProUGUI>().text = "+"+damageNumber.ToString();   
        GetComponent<Animator>().SetBool("ShowDamageNumber", true);     
        StartCoroutine(DestroyText());
    }
    
    IEnumerator DestroyText()
    {    
        yield return new WaitForSeconds(0.8f);   
        Destroy(gameObject);
    } 
}
