/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Collaborators:  (mosthated@instance.id) (@MostHated)
 * Date: 08-Oct-18
 * Studio: Tayx
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

using Tayx.Graphy.UI;

namespace Tayx.Graphy.Console
{
    public class ConsoleView : MonoBehaviour, IMovable
    {
        #region Private Variables

        private RectTransform                   m_rectTransform;

        [SerializeField] private List<Image>    m_backgroundImages = new List<Image>();

        [SerializeField] private Text           m_logText;

        private GraphyManager.ModulePosition    m_modulePosition;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            UpdateParameters();
        }

        #endregion

        #region Public Methods

        // TODO: Pending implementation
        public void SetPosition(GraphyManager.ModulePosition newModulePosition)
        {
            switch (newModulePosition)
            {
                case GraphyManager.ModulePosition.TOP_LEFT:

                    break;

                case GraphyManager.ModulePosition.TOP_RIGHT:

                    break;

                case GraphyManager.ModulePosition.BOTTOM_LEFT:

                    break;

                case GraphyManager.ModulePosition.BOTTOM_RIGHT:

                    break;
            }
        }


        public void UpdateParameters()
        {

        }

        public void UpdateLogTextValue(string newText)
        {
            m_logText.text = newText;
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            
        }

        #endregion
    }
}