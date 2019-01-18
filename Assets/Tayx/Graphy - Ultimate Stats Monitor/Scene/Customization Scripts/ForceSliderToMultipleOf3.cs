/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            05-Mar-18
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

using System.Collections;

namespace Tayx.Graphy.CustomizationScene
{
	public class ForceSliderToMultipleOf3 : MonoBehaviour
	{
        /* ----- TODO: ----------------------------
         * Check if we can seal this class.
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we can remove "using System.Collections;".
         * Check if we should add "private" to the Unity Callbacks.
         * --------------------------------------*/

        #region Variables -> Serialized Private

        [SerializeField] private Slider m_slider = null;

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
			int roundedValue = (int)value;
			
			// Forces the value to be a multiple of 3, this way the audio graph is painted correctly
			if (roundedValue % 3 != 0 && roundedValue < 300)
			{
				roundedValue += 3 - roundedValue % 3;
			}

			m_slider.value = roundedValue;
		}

        #endregion
    }
}