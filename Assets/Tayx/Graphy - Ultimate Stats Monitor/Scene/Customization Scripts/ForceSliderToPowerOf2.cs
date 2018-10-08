﻿/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 05-Mar-18
 * Studio: Tayx
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

using System.Collections;

namespace Tayx.Graphy.CustomizationScene
{
    //TODO: Check if we can seal the class.
	public class ForceSliderToPowerOf2 : MonoBehaviour
	{
        //TODO: Add summaries.
        #region Variables -> Serialized Private

        [SerializeField] private Slider m_slider;

        #endregion

        #region Variables -> Private

        private int[] m_powerOf2Values =
		{
			128,
			256,
			512,
			1024,
			2048,
			4096,
			8192
		};
		
		private Text m_text;

        #endregion

        #region Methods -> Unity Callbacks

        void Start()
		{
			m_slider.onValueChanged.AddListener(UpdateValue);
		}

        #endregion

        #region Methods -> Private

        private void UpdateValue(float value)
		{
			int closestSpectrumIndex = 0;
			int minDistanceToSpectrumValue = 100000;

            //TODO: Put the int cast outside of the loop.
			for (int i = 0; i < m_powerOf2Values.Length; i++)
			{
				int newDistance = Mathf.Abs((int)value - m_powerOf2Values[i]);
				if (newDistance < minDistanceToSpectrumValue)
				{
					minDistanceToSpectrumValue = newDistance;
					closestSpectrumIndex = i;
				}
			}
			
			m_slider.value = m_powerOf2Values[closestSpectrumIndex];
		}

        #endregion
    }
}