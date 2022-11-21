using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Map;

public class ShopHealthBar : MonoBehaviour
{
    float _previousHP; 
    [SerializeField] Image HeroHealthBarElement;    
    [SerializeField] TextMeshProUGUI HeroHealthTextElement;

    // Start is called before the first frame update
    void Start()
    {
        //DEBUG
        MapManager.GetInstance().HeroHP = 100;
        MapManager.GetInstance().HeroMaxHP = 200;
    }

    // Update is called once per frame
    void Update()
    {
        if(MapManager.GetInstance() != null)
        {
            float _currentHP = MapManager.GetInstance().HeroHP;
            if(_currentHP != _previousHP)
            {
                
                float _currentMaxHP = MapManager.GetInstance().HeroMaxHP;
                _previousHP = _currentHP;
                SetHealthbar(_previousHP, _currentMaxHP);
            }
        }
    }

    void SetHealthbar(float HP, float maxHP)
    {
        HeroHealthBarElement.fillAmount = MapManager.GetInstance().GetHeroHPForFill();
        HeroHealthTextElement.text = string.Format("{0:0}", MapManager.GetInstance().HeroHP) + "/" + MapManager.GetInstance().HeroMaxHP;
    }
}
