﻿/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@martinTayx)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            03-Jan-18
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Tayx.Graphy.UI;
using Tayx.Graphy.Utils;
using TMPro;

namespace Tayx.Graphy.Audio
{
    public class G_AudioManager : MonoBehaviour, IMovable, IModifiableState
    {
        #region Variables -> Serialized Private

        [SerializeField] private GameObject m_audioGraphGameObject = null;
        [SerializeField] private TMP_Text m_audioDbText = null;

        [SerializeField] private List<Image> m_backgroundImages = new List<Image>();

        #endregion

        #region Variables -> Private

        private GraphyManager m_graphyManager = null;

        private G_AudioGraph m_audioGraph = null;
        private G_AudioMonitor m_audioMonitor = null;
        private G_AudioText m_audioText = null;

        private RectTransform m_rectTransform = null;
        private Vector2 m_origPosition = Vector2.zero;

        private List<GameObject> m_childrenGameObjects = new List<GameObject>();

        private GraphyManager.ModuleState m_previousModuleState = GraphyManager.ModuleState.FULL;
        private GraphyManager.ModuleState m_currentModuleState = GraphyManager.ModuleState.FULL;

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

        public void SetPosition( GraphyManager.ModulePosition newModulePosition, Vector2 offset )
        {
            if ( newModulePosition == GraphyManager.ModulePosition.FREE )
                return;
            
            m_rectTransform.anchoredPosition = m_origPosition;

            float xSideOffset = Mathf.Abs( m_rectTransform.anchoredPosition.x ) + offset.x;
            float ySideOffset = Mathf.Abs( m_rectTransform.anchoredPosition.y ) + offset.y;

            m_audioDbText.alignment = TextAlignmentOptions.TopRight;

            switch( newModulePosition )
            {
                case GraphyManager.ModulePosition.TOP_LEFT:

                    m_rectTransform.anchorMax = Vector2.up;
                    m_rectTransform.anchorMin = Vector2.up;
                    m_rectTransform.anchoredPosition = new Vector2( xSideOffset, -ySideOffset );

                    break;

                case GraphyManager.ModulePosition.TOP_RIGHT:

                    m_rectTransform.anchorMax = Vector2.one;
                    m_rectTransform.anchorMin = Vector2.one;
                    m_rectTransform.anchoredPosition = new Vector2( -xSideOffset, -ySideOffset );

                    break;

                case GraphyManager.ModulePosition.BOTTOM_LEFT:

                    m_rectTransform.anchorMax = Vector2.zero;
                    m_rectTransform.anchorMin = Vector2.zero;
                    m_rectTransform.anchoredPosition = new Vector2( xSideOffset, ySideOffset );

                    break;

                case GraphyManager.ModulePosition.BOTTOM_RIGHT:

                    m_rectTransform.anchorMax = Vector2.right;
                    m_rectTransform.anchorMin = Vector2.right;
                    m_rectTransform.anchoredPosition = new Vector2( -xSideOffset, ySideOffset );

                    break;
            }
        }

        public void SetState( GraphyManager.ModuleState state, bool silentUpdate = false )
        {
            if( !silentUpdate )
            {
                m_previousModuleState = m_currentModuleState;
            }

            m_currentModuleState = state;

            switch( state )
            {
                case GraphyManager.ModuleState.FULL:
                    gameObject.SetActive( true );
                    m_childrenGameObjects.SetAllActive( true );
                    SetGraphActive( true );

                    if( m_graphyManager.Background )
                    {
                        m_backgroundImages.SetOneActive( 0 );
                    }
                    else
                    {
                        m_backgroundImages.SetAllActive( false );
                    }

                    break;

                case GraphyManager.ModuleState.TEXT:
                case GraphyManager.ModuleState.BASIC:
                    gameObject.SetActive( true );
                    m_childrenGameObjects.SetAllActive( true );
                    SetGraphActive( false );

                    if( m_graphyManager.Background )
                    {
                        m_backgroundImages.SetOneActive( 1 );
                    }
                    else
                    {
                        m_backgroundImages.SetAllActive( false );
                    }

                    break;

                case GraphyManager.ModuleState.BACKGROUND:
                    gameObject.SetActive( true );
                    SetGraphActive( false );
                    m_childrenGameObjects.SetAllActive( false );

                    m_backgroundImages.SetAllActive( false );

                    break;

                case GraphyManager.ModuleState.OFF:
                    gameObject.SetActive( false );
                    break;
            }
        }

        public void RestorePreviousState()
        {
            SetState( m_previousModuleState );
        }

        public void UpdateParameters()
        {
            foreach( var image in m_backgroundImages )
            {
                image.color = m_graphyManager.BackgroundColor;
            }

            m_audioGraph.UpdateParameters();
            m_audioMonitor.UpdateParameters();
            m_audioText.UpdateParameters();

            SetState( m_graphyManager.AudioModuleState );
        }

        public void RefreshParameters()
        {
            foreach( var image in m_backgroundImages )
            {
                image.color = m_graphyManager.BackgroundColor;
            }

            m_audioGraph.UpdateParameters();
            m_audioMonitor.UpdateParameters();
            m_audioText.UpdateParameters();

            SetState( m_currentModuleState, true );
        }

        #endregion

        #region Methods -> Private

        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_rectTransform = GetComponent<RectTransform>();
            m_origPosition = m_rectTransform.anchoredPosition;

            m_audioGraph = GetComponent<G_AudioGraph>();
            m_audioMonitor = GetComponent<G_AudioMonitor>();
            m_audioText = GetComponent<G_AudioText>();

            foreach( Transform child in transform )
            {
                if( child.parent == transform )
                {
                    m_childrenGameObjects.Add( child.gameObject );
                }
            }
        }

        private void SetGraphActive( bool active )
        {
            m_audioGraph.enabled = active;
            m_audioGraphGameObject.SetActive( active );
        }

        #endregion
    }
}