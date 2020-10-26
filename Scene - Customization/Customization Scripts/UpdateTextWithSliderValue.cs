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
    [RequireComponent(typeof(Text))]
	public class UpdateTextWithSliderValue : MonoBehaviour
	{
        #region Variables -> Serialized Private

        [SerializeField] private Slider m_slider = null;

        #endregion

        #region Variables -> Private

        private Text m_text;

        #endregion

        #region Methods -> Unity Callbacks

        void Start()
		{
			m_text = GetComponent<Text>();
		}

        void LateUpdate()
        {
            m_text.text = m_slider.value.ToString();
        }

        #endregion
    }
}