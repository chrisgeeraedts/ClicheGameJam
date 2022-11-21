using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Map;

public class CoinCanvas : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI coinsText;
    private int _previousCoins;

    // Start is called before the first frame update
    void Start()
    {
        _previousCoins = MapManager.GetInstance().Coins;
        SetCoins(_previousCoins);
    }

    // Update is called once per frame
    void Update()
    {
        if(MapManager.GetInstance() != null)
        {
            int _currentCoins = MapManager.GetInstance().Coins;
            if(_currentCoins != _previousCoins)
            {
                _previousCoins = _currentCoins;
                SetCoins(_previousCoins);
            }
        }
    }

    void SetCoins(int coins)
    {
         coinsText.text = $"<color=#fede34>{coins}</color>";
    }
}
