 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomController : MonoBehaviour
{
    //public PostProcessProfile profile;
    public float BloomIntesifier = 1.1f;
    public float bloomSpeed = 0.5f;
    private float initialBloomSpeed = 0.5f;
    private float direction = 100;
    private Bloom _bloom;
    private Volume _volume;

    // Start is called before the first frame update
    void Start()
    {
        _volume = GetComponent<Volume>();
        _volume.profile.TryGet(out _bloom);
        initialBloomSpeed = bloomSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(_shouldBloom)
        {
            bloomSpeed = bloomSpeed * BloomIntesifier;
            _bloom.intensity.value += direction * bloomSpeed * Time.deltaTime;
            if (_bloom.intensity.value >= _intensityTarget) {
                //done
            } 
            Debug.Log ("Bloom Intensity: " + _bloom.intensity.value);
        }
        else
        {
            bloomSpeed = bloomSpeed / BloomIntesifier;
             _bloom.intensity.value -= direction * bloomSpeed * Time.deltaTime;
            if (_bloom.intensity.value <= 1) {
                bloomSpeed = initialBloomSpeed;
            } 
        }
    }

    private bool _shouldBloom;
    private float _intensityTarget;
    public void StartBloom(float bloomTarget)
    {
        _shouldBloom = true;
        _intensityTarget = bloomTarget;
    }

    public void StopBloom()
    {
        _shouldBloom = false;
    }
}