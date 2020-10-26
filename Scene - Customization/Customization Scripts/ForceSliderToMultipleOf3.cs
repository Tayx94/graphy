/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            05-Mar-18
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.CustomizationScene
{
	public class ForceSliderToMultipleOf3 : MonoBehaviour
	{
        #region Variables -> Serialized Private

        [SerializeField] private Slider m_slider = null;

		#endregion

		#region Methods -> Unity Callbacks

		void Update()
        {
            int roundedValue = (int)m_slider.value;

            // Forces the value to be a multiple of 3, this way the audio graph is painted correctly
            if ( roundedValue % 3 != 0 && roundedValue < 300 )
            {
                roundedValue += 3 - roundedValue % 3;
            }

            m_slider.value = roundedValue;
        }

		#endregion
    }
}