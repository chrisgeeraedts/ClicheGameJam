using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Map
{
	public class ProgressBar : MonoBehaviour {
		
		public GameObject FillImage;
		public TMP_Text ProgressText;
		public float FillSpeed;
		private float _fillAmount;
		private Color _fillColor;


		public void SetFill(float fillAmount)
		{
			_fillAmount = fillAmount;
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
			}
		}
	}
}