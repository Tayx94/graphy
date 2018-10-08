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

using System.Collections;

using Tayx.Graphy.UI;

namespace Tayx.Graphy.Console
{
    public class ConsoleManager : MonoBehaviour, IMovable
    {

        #region Private Variables

        private GraphyManager           m_graphyManager;

        private ConsoleView             m_consoleView;

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

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        #endregion

        #region Public Methods

        public void SetPosition(GraphyManager.ModulePosition newModulePosition)
        {
            m_consoleView.SetPosition(newModulePosition);
        }


        public void UpdateParameters()
        {

        }

        #endregion

        #region Private Methods

        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_consoleView = GetComponent<ConsoleView>();
        }

        

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            m_consoleView.UpdateLogTextValue(type.ToString() + ": " + logString);
        }

        #endregion
    }
}