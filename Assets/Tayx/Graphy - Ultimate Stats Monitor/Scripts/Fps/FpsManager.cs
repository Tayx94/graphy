/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 03-Jan-18
 * Studio: Tayx
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * -------------------------------------*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tayx.Graphy.UI;
using Tayx.Graphy.Utils;
using UnityEngine.UI;

namespace Tayx.Graphy.Fps
{
    public class FpsManager : MonoBehaviour, IMovable, IModifiableState
    {

        #region Private Variables
        
        private GraphyManager m_graphyManager;
        
        private FpsGraph m_fpsGraph;
        private FpsMonitor m_fpsMonitor;
        private FpsText m_fpsText;

        private RectTransform m_rectTransform;


        [SerializeField] private GameObject m_fpsGraphGameObject;

        [SerializeField] private List<GameObject> m_nonBasicTextGameObjects = new List<GameObject>();

        [SerializeField] private List<Image> m_backgroundImages = new List<Image>();

        private List<GameObject> m_childrenGameObjects = new List<GameObject>();

        private GraphyManager.ModuleState m_previousModuleState;
        private GraphyManager.ModuleState m_currentModuleState;
        
        #endregion

        #region Unity Methods

        void Awake()
        {
            Init();
        }
        
        void Start()
        {
            UpdateParameters();
        }

        #endregion

        #region Public Methods

        public void SetPosition(GraphyManager.ModulePosition newModulePosition)
        {
            float xSideOffset = Mathf.Abs(m_rectTransform.anchoredPosition.x);
            float ySideOffset = Mathf.Abs(m_rectTransform.anchoredPosition.y);

            switch (newModulePosition)
            {
                case GraphyManager.ModulePosition.TOP_LEFT:

                    m_rectTransform.anchorMax = Vector2.up;
                    m_rectTransform.anchorMin = Vector2.up;
                    m_rectTransform.anchoredPosition = new Vector2(xSideOffset, -ySideOffset);

                    break;

                case GraphyManager.ModulePosition.TOP_RIGHT:

                    m_rectTransform.anchorMax = Vector2.one;
                    m_rectTransform.anchorMin = Vector2.one;
                    m_rectTransform.anchoredPosition = new Vector2(-xSideOffset, -ySideOffset);

                    break;

                case GraphyManager.ModulePosition.BOTTOM_LEFT:

                    m_rectTransform.anchorMax = Vector2.zero;
                    m_rectTransform.anchorMin = Vector2.zero;
                    m_rectTransform.anchoredPosition = new Vector2(xSideOffset, ySideOffset);

                    break;

                case GraphyManager.ModulePosition.BOTTOM_RIGHT:

                    m_rectTransform.anchorMax = Vector2.right;
                    m_rectTransform.anchorMin = Vector2.right;
                    m_rectTransform.anchoredPosition = new Vector2(-xSideOffset, ySideOffset);

                    break;
            }
        }

        public void SetState(GraphyManager.ModuleState state)
        {
            m_previousModuleState = m_currentModuleState;
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

                case GraphyManager.ModuleState.BASIC:
                    gameObject.SetActive(true);
                    m_childrenGameObjects.SetAllActive(true);
                    m_nonBasicTextGameObjects.SetAllActive(false);
                    SetGraphActive(false);
                    
                    if (m_graphyManager.Background)
                    {
                        m_backgroundImages.SetOneActive(2);
                    }
                    else
                    {
                        m_backgroundImages.SetAllActive(false);
                    }

                    break;

                case GraphyManager.ModuleState.BACKGROUND:
                    gameObject.SetActive(true);
                    m_childrenGameObjects.SetAllActive(false);
                    SetGraphActive(false);
                    
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
            
            m_fpsGraph.UpdateParameters();
            m_fpsMonitor.UpdateParameters();
            m_fpsText.UpdateParameters();
            
            SetState(m_graphyManager.FpsModuleState);
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();
            
            m_rectTransform = GetComponent<RectTransform>();

            m_fpsGraph = GetComponent<FpsGraph>();
            m_fpsMonitor = GetComponent<FpsMonitor>();
            m_fpsText = GetComponent<FpsText>();

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
            m_fpsGraph.enabled = active;
            m_fpsGraphGameObject.SetActive(active);
        }

        #endregion
    }
}