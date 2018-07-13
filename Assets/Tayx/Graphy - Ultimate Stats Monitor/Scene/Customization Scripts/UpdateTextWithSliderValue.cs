/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 05-Mar-18
 * Studio: Tayx
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

using System.Collections;

namespace Tayx.Graphy.CustomizationScene
{
	public class UpdateTextWithSliderValue : MonoBehaviour
	{
		[SerializeField] private Slider m_slider;

		private Text m_text;
		
		void Start()
		{
			m_text = GetComponent<Text>();

			m_slider.onValueChanged.AddListener(UpdateText);
		}

		private void UpdateText(float value)
		{
			m_text.text = value.ToString();
		}
	}
}