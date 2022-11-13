 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomCameraRaiserScript : MonoBehaviour
{
    //public PostProcessProfile profile;
    public float BloomIntesifier = 1.1f;
    public float bloomSpeed = 0.5f;
    private float direction = 100;
    private Bloom _bloom;
    private Volume _volume;

    // Start is called before the first frame update
    void Start()
    {
        _volume = GetComponent<Volume>();
        _volume.profile.TryGet(out _bloom);
    }

    // Update is called once per frame
    void Update()
    {
        if(_shouldBloom)
        {
            bloomSpeed = bloomSpeed * BloomIntesifier;
            _bloom.intensity.value += direction * bloomSpeed * Time.deltaTime;
            if (_bloom.intensity.value >= 99999) {
                //done
            } 
            Debug.Log ("Bloom Intensity: " + _bloom.intensity.value);
        }
    }

    private bool _shouldBloom;
    public void StartBloom()
    {
        _shouldBloom = true;
    }
}
