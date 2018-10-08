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
    [RequireComponent(typeof(Text))]
	public class UpdateTextWithSliderValue : MonoBehaviour
	{
        /* ----- TODO: ----------------------------
         * Check if we can seal this class.
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we can remove "using System.Collections;".
         * Check if we should add "private" to the Unity Callbacks.
         * --------------------------------------*/

        #region Variables -> Serialized Private

        [SerializeField] private Slider m_slider;

        #endregion

        #region Variables -> Private

        private Text m_text;

        #endregion

        #region Methods -> Unity Callbacks

        void Start()
		{
			m_text = GetComponent<Text>();

			m_slider.onValueChanged.AddListener(UpdateText);
		}

        #endregion

        #region Methods -> Private

        private void UpdateText(float value)
		{
			m_text.text = value.ToString();
		}

        #endregion
    }
}