using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.FinalBossScene
{
	public class EyeLaserTimer : MonoBehaviour {
		
		public GameObject FillImage;
		public TMP_Text ProgressText;
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
			FillImage.GetComponent<Image>().fillAmount = _fillAmount;
			ProgressText.text = _text;
		}
	}
}