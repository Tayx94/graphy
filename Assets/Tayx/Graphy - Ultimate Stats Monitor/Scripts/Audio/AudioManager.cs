/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 03-Jan-18
 * Studio: Tayx
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

using Tayx.Graphy.UI;
using Tayx.Graphy.Utils;

namespace Tayx.Graphy.Audio
{
    public class AudioManager : MonoBehaviour, IMovable, IModifiableState
    {

        #region Private Variables
        
        private GraphyManager m_graphyManager;

        private AudioGraph m_audioGraph;
        private AudioMonitor m_audioMonitor;
        private AudioText m_audioText;

        private RectTransform m_rectTransform;

        [SerializeField] private GameObject m_audioGraphGameObject;
        [SerializeField] private Text m_audioDbText;

        private List<GameObject> m_childrenGameObjects = new List<GameObject>();
        
        [SerializeField] private List<Image> m_backgroundImages = new List<Image>();

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

                    m_audioDbText.alignment = TextAnchor.UpperLeft;
                    
                    break;

                case GraphyManager.ModulePosition.TOP_RIGHT:

                    m_rectTransform.anchorMax = Vector2.one;
                    m_rectTransform.anchorMin = Vector2.one;
                    m_rectTransform.anchoredPosition = new Vector2(-xSideOffset, -ySideOffset);

                    m_audioDbText.alignment = TextAnchor.UpperRight;
                    
                    break;

                case GraphyManager.ModulePosition.BOTTOM_LEFT:

                    m_rectTransform.anchorMax = Vector2.zero;
                    m_rectTransform.anchorMin = Vector2.zero;
                    m_rectTransform.anchoredPosition = new Vector2(xSideOffset, ySideOffset);

                    m_audioDbText.alignment = TextAnchor.UpperLeft;

                    break;

                case GraphyManager.ModulePosition.BOTTOM_RIGHT:

                    m_rectTransform.anchorMax = Vector2.right;
                    m_rectTransform.anchorMin = Vector2.right;
                    m_rectTransform.anchoredPosition = new Vector2(-xSideOffset, ySideOffset);

                    m_audioDbText.alignment = TextAnchor.UpperRight;

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
            
            m_audioGraph.UpdateParameters();
            m_audioMonitor.UpdateParameters();
            m_audioText.UpdateParameters();
            
            SetState(m_graphyManager.AudioModuleState);
        }
        
        #endregion

        #region Private Methods

        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_rectTransform = GetComponent<RectTransform>();

            m_audioGraph = GetComponent<AudioGraph>();
            m_audioMonitor = GetComponent<AudioMonitor>();
            m_audioText = GetComponent<AudioText>();
            
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
            m_audioGraph.enabled = active;
            m_audioGraphGameObject.SetActive(active);
        }

        #endregion
    }


}
