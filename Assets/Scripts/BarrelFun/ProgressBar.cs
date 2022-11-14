using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.BarrelFun
{
	public class ProgressBar : MonoBehaviour {
		
		public GameObject FillImage;
		public TMP_Text ProgressText;
		public float FillSpeed;
		private float _fillAmount;
		private Color _fillColor;
		private string _text;

		public void InitFill(float fill, string text)
		{
			currentFill = fill;
			_text = text;
		}

		public void SetFill(float fillAmount, string text)
		{
			_fillAmount = fillAmount;
			_text = text;
		}
		public void SetColor(Color fillColor)
		{
			_fillColor = fillColor;
		}

		float currentFill;
		public void Update() {
			if(currentFill > _fillAmount)
			{
				currentFill = currentFill - FillSpeed * Time.deltaTime;
				FillImage.GetComponent<Image>().fillAmount = currentFill;
				ProgressText.text = _text;
			}
		}
	}
}