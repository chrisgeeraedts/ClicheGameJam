using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Map;

public class FishCanvas : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI FishText;
    private int _previousFish;

    // Start is called before the first frame update
    void Start()
    {
        _previousFish = MapManager.GetInstance().NumberOfFishInInventory;
        SetFish(_previousFish);
    }

    // Update is called once per frame
    void Update()
    {
        if(MapManager.GetInstance() != null)
        {
            int _currentFish = MapManager.GetInstance().NumberOfFishInInventory;
            if(_currentFish != _previousFish)
            {
                _previousFish = _currentFish;
                SetFish(_previousFish);
            }
        }
    }

    void SetFish(int Fish)
    {
         FishText.text = $"<color=#4734FE>{Fish}</color>";
    }
}
