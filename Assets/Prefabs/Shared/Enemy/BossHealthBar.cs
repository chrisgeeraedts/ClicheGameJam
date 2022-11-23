using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Shared
{
	public class BossHealthBar : MonoBehaviour {
		
		public GameObject FillImage;
		public TMP_Text ProgressText;
		public float FillSpeed;
		private float _fillAmount;
		private Color _fillColor;      
        private bool Toggled;

		public void Toggle(bool toggled)
        {
            Toggled = toggled;
        }

        public bool IsToggled()
        {
            return Toggled;
        }

		public void SetFill(float fillAmount)
		{
			Debug.Log(fillAmount);
			_fillAmount = fillAmount;
		}
		public void SetColor(Color fillColor)
		{
			_fillColor = fillColor;
		}

		public void SetProgressText(string text)
		{
			ProgressText.text = text;
		}

		float currentFill;
		void Update() {
		if(Toggled)
			{
				FillImage.GetComponent<Image>().fillAmount = _fillAmount;
			}
		}
	}
}