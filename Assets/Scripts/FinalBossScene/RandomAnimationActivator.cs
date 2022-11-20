using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationActivator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Animator>().enabled = false;
    }

    public void ShowAnimation(){

    }

    void Update()
    {
        if(_shouldShow && !_isShowing)
        {
            if(UnityEngine.Random.Range(0, _randomAmount) == 0)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<Animator>().enabled = true;
                _isShowing = true;
                explosionSound.Play();
                ShowAnimation();
                StartCoroutine(FinishAnimation());
            }
        }
    }

    IEnumerator FinishAnimation()
    {        
            yield return new WaitForSeconds(_animationDuration);  
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Animator>().enabled = false;
            _isShowing = false;
    }

    private bool _shouldShow = false;
    private bool _isShowing = false;
    private float _animationDuration;
    private int _randomAmount;
    public AudioSource explosionSound;
    public void StartAnimationRandomly(int randomAmount, float animationDuration)
    {
        _randomAmount = randomAmount;
        _animationDuration = animationDuration;
        _shouldShow = true;

    }
}
