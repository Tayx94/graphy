/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            03-Jan-18
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using System.Collections.Generic;
using Tayx.Graphy.UI;
using Tayx.Graphy.Utils;
using UnityEngine.UI;

namespace Tayx.Graphy.Ram
{
    public class G_RamManager : MonoBehaviour, IMovable, IModifiableState
    {
        /* ----- TODO: ----------------------------
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we should add a "RequireComponent" for "RectTransform".
         * Check if we should add a "RequireComponent" for "RamGraph".
         * Check why this manager doesnt use RamMonitor, as all the other managers have a monitor script.
         * Check if we should add a "RequireComponent" for "RamText".
         * --------------------------------------*/

        #region Variables -> Serialized Private

        [SerializeField] private    GameObject              m_ramGraphGameObject = null;

        [SerializeField] private    List<Image>             m_backgroundImages          = new List<Image>();

        #endregion

        #region Variables -> Private

        private                 GraphyManager               m_graphyManager = null;
        
        private                 G_RamGraph                  m_ramGraph = null;
        private                 G_RamText                   m_ramText = null;

        private                 RectTransform               m_rectTransform = null;

        private                 List<GameObject>            m_childrenGameObjects       = new List<GameObject>();

        private                 GraphyManager.ModuleState   m_previousModuleState = GraphyManager.ModuleState.FULL;
        private                 GraphyManager.ModuleState   m_currentModuleState = GraphyManager.ModuleState.FULL;
        
        #endregion

        #region Methods -> Unity Callbacks

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            UpdateParameters();
        }

        #endregion

        #region Methods -> Public

        public void SetPosition(GraphyManager.ModulePosition newModulePosition)
        {
            float xSideOffset = Mathf.Abs(m_rectTransform.anchoredPosition.x);
            float ySideOffset = Mathf.Abs(m_rectTransform.anchoredPosition.y);

            switch (newModulePosition)
            {
                case GraphyManager.ModulePosition.TOP_LEFT:

                    m_rectTransform.anchorMax           = Vector2.up;
                    m_rectTransform.anchorMin           = Vector2.up;
                    m_rectTransform.anchoredPosition    = new Vector2(xSideOffset, -ySideOffset);

                    break;

                case GraphyManager.ModulePosition.TOP_RIGHT:

                    m_rectTransform.anchorMax           = Vector2.one;
                    m_rectTransform.anchorMin           = Vector2.one;
                    m_rectTransform.anchoredPosition    = new Vector2(-xSideOffset, -ySideOffset);

                    break;

                case GraphyManager.ModulePosition.BOTTOM_LEFT:

                    m_rectTransform.anchorMax           = Vector2.zero;
                    m_rectTransform.anchorMin           = Vector2.zero;
                    m_rectTransform.anchoredPosition    = new Vector2(xSideOffset, ySideOffset);

                    break;

                case GraphyManager.ModulePosition.BOTTOM_RIGHT:

                    m_rectTransform.anchorMax           = Vector2.right;
                    m_rectTransform.anchorMin           = Vector2.right;
                    m_rectTransform.anchoredPosition    = new Vector2(-xSideOffset, ySideOffset);

                    break;

                case GraphyManager.ModulePosition.FREE:
                    break;
            }
        }

        public void SetState(GraphyManager.ModuleState state, bool silentUpdate = false)
        {
            if (!silentUpdate)
            {
                m_previousModuleState = m_currentModuleState;
            }

            m_currentModuleState = state;

            switch (state)
            {
                case GraphyManager.ModuleState.FULL:
                    gameObject.SetActive(true);
                    m_childrenGameObjects.SetAllActive(true);
                    SetGraphActive(true);
                    
                    if (m_graphyManager.Background)
                    {
                        m_backgroundImages.SetOneActive(0);
                    }
                    else
                    {
                        m_backgroundImages.SetAllActive(false);
                    }
                    
                    break;

                case GraphyManager.ModuleState.TEXT:
                case GraphyManager.ModuleState.BASIC:
                    gameObject.SetActive(true);
                    m_childrenGameObjects.SetAllActive(true);
                    SetGraphActive(false);
                    
                    if (m_graphyManager.Background)
                    {
                        m_backgroundImages.SetOneActive(1);
                    }
                    else
                    {
                        m_backgroundImages.SetAllActive(false);
                    }
                    
                    break;

                case GraphyManager.ModuleState.BACKGROUND:
                    gameObject.SetActive(true);
                    SetGraphActive(false);

                    m_childrenGameObjects.SetAllActive(false);
                    m_backgroundImages.SetAllActive(false);

                    break;

                case GraphyManager.ModuleState.OFF:
                    gameObject.SetActive(false);
                    break;
            }
        }

        public void RestorePreviousState()
        {
            SetState(m_previousModuleState);
        }
      
        public void UpdateParameters()
        {
            foreach (var image in m_backgroundImages)
            {
                image.color = m_graphyManager.BackgroundColor;
            }
            
            m_ramGraph  .UpdateParameters();
            m_ramText   .UpdateParameters();
            
            SetState(m_graphyManager.RamModuleState);
        }

        public void RefreshParameters()
        {
            foreach (var image in m_backgroundImages)
            {
                image.color = m_graphyManager.BackgroundColor;
            }

            m_ramGraph  .UpdateParameters();
            m_ramText   .UpdateParameters();

            SetState(m_currentModuleState, true);
        }

        #endregion

        #region Methods -> Private

        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_ramGraph      = GetComponent<G_RamGraph>();
            m_ramText       = GetComponent<G_RamText>();

            m_rectTransform = GetComponent<RectTransform>();
            
            foreach (Transform child in transform)
            {
                if (child.parent == transform)
                {
                    m_childrenGameObjects.Add(child.gameObject);
                }
            }
        }

        private void SetGraphActive(bool active)
        {
            m_ramGraph.enabled = active;
            m_ramGraphGameObject.SetActive(active);
        }

        #endregion
    }
}