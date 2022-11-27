using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Escort
{
    public class ProgressBar : MonoBehaviour
    {

        public GameObject FillImage;
        public TMP_Text ProgressText;
        public float FillSpeed;
        private float _fillAmount;
        private Color _fillColor;
        private float previousFill;

        public void SetFill(float fillAmount)
        {
            _fillAmount = fillAmount;
        }
        public void SetColor(Color fillColor)
        {
            _fillColor = fillColor;
        }

        float currentFill;
        public void Update()
        {
            FillImage.GetComponent<Image>().fillAmount = _fillAmount;
            ProgressText.text = String.Format("{0:0.##}", (_fillAmount * 100)) + "% frustrated";

            AnimateWhenCrossingTreshold();
            previousFill = _fillAmount;
        }

        private void AnimateWhenCrossingTreshold()
        {
            var x = (int)(previousFill * 100) / 10;
            var y = (int)(_fillAmount* 100) / 10;

            if (x == y) return;

            StartCoroutine(AnimateIn());
        }

        private IEnumerator AnimateIn()
        {
            FillImage.transform.localScale = new Vector3(2, 2, 2);

            yield return new WaitForSeconds(0.2f);
            AnimateBack();
        }

        private void AnimateBack()
        {
            FillImage.transform.localScale = new Vector3(1, 1, 1);
        }

    }
}