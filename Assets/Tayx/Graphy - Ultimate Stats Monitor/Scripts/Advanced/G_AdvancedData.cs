/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            05-Dec-17
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using Tayx.Graphy.UI;
using Tayx.Graphy.Utils;
using Tayx.Graphy.Utils.NumString;
using TMPro;

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace Tayx.Graphy.Advanced
{
    public class G_AdvancedData : MonoBehaviour, IMovable, IModifiableState
    {
        /* ----- TODO: ----------------------------
         * Add summaries to the variables.
         * Add summaries to the functions.
         * --------------------------------------*/

        #region Variables -> Serialized Private

        [SerializeField] private    List<Image>                 m_backgroundImages              = new List<Image>();

        [SerializeField] private    TMP_Text                    m_screenResolutionText          = null;
        [SerializeField] private    TMP_Text                    m_windowResolutionText          = null;
        [SerializeField] private    TMP_Text                    m_operatingSystemText           = null;
        [SerializeField] private    TMP_Text                    m_graphicsDeviceVersionText     = null;
        [SerializeField] private    TMP_Text                    m_processorTypeText             = null;
        [SerializeField] private    TMP_Text                    m_threadCountText               = null;
        [SerializeField] private    TMP_Text                    m_systemMemoryText              = null;
        [SerializeField] private    TMP_Text                    m_graphicsDeviceNameText        = null;
        [SerializeField] private    TMP_Text                    m_graphicsMemorySizeText        = null;
        [SerializeField] private    TMP_Text                    m_maxTextureSizeText            = null;
        [SerializeField] private    TMP_Text                    m_graphicsShaderLevelText       = null;

        [SerializeField] private    RectTransform               m_labelRectTransform            = null;
        [SerializeField] private    RectTransform               m_valueRectTransform            = null;

        [Range(1, 60)]
        [SerializeField] private    float                       m_updateRate                    = 1f;  // 1 update per sec.

        #endregion

        #region Variables -> Private

        private                     GraphyManager               m_graphyManager                 = null;

        private                     RectTransform               m_rectTransform                 = null;

        private                     float                       m_deltaTime                     = 0.0f;

        private                     StringBuilder               m_sb                            = null;

        private                     GraphyManager.ModuleState   m_previousModuleState           = GraphyManager.ModuleState.FULL;
        private                     GraphyManager.ModuleState   m_currentModuleState            = GraphyManager.ModuleState.FULL;

        private readonly            string[]                    m_windowStrings                 =
        {
            " x ",
            " @ ",
            "Hz",
            " [",
            "dpi]"
        };

        private                     int                         m_previousScreenWidth           = 0;
        private                     int                         m_previousScreenHeight          = 0;
        private                     int                         m_previousScreenRefreshrate     = 0;
        private                     float                       m_previousScreenDPI             = 0f;

        #endregion

        #region Methods -> Unity Callbacks

        private void OnEnable()
        {
            Init();
        }

        private void Update()
        {
            m_deltaTime += Time.unscaledDeltaTime;

            if (m_deltaTime > 1f / m_updateRate)
            {
                // Check if anything changed
                if (   m_previousScreenWidth != Screen.width
                    || m_previousScreenHeight != Screen.height
                    || m_previousScreenRefreshrate != Screen.currentResolution.refreshRate
                    || !Mathf.Approximately(m_previousScreenDPI, Screen.dpi))
                {
                    // Update screen window resolution
                    m_sb.Length = 0;

                    m_sb.Append(Screen.width).Append(m_windowStrings[0]).Append(Screen.height)                              // ####x####
                        .Append(m_windowStrings[1]).Append(Screen.currentResolution.refreshRate).Append(m_windowStrings[2]) // @##Hz
                        .Append(m_windowStrings[3]).Append(Screen.dpi).Append(m_windowStrings[4]);                          // [##.##]

                    m_windowResolutionText.SetText(m_sb);

                    // Reset variables
                    m_deltaTime = 0f;
                }
            }
        }

        #endregion

        #region Methods -> Public

        public void SetPosition(GraphyManager.ModulePosition newModulePosition)
        {
            // commented for now
            /*
            float xSideOffsetBackgroundImage    = Mathf.Abs(m_backgroundImages[0].rectTransform.anchoredPosition.x);
            float ySideOffset                   = Mathf.Abs(m_rectTransform.anchoredPosition.y);

            switch (newModulePosition)
            {
                case GraphyManager.ModulePosition.TOP_LEFT:

                    m_rectTransform.anchorMax                               = Vector2.one;
                    m_rectTransform.anchorMin                               = Vector2.up;
                    m_rectTransform.anchoredPosition                        = new Vector2(0, -ySideOffset);
                    
                    
                    m_backgroundImages[0].rectTransform.anchorMax           = Vector2.up;
                    m_backgroundImages[0].rectTransform.anchorMin           = Vector2.zero;
                    m_backgroundImages[0].rectTransform.anchoredPosition    = new Vector2(xSideOffsetBackgroundImage, 0);

                    break;

                case GraphyManager.ModulePosition.TOP_RIGHT:

                    m_rectTransform.anchorMax                               = Vector2.one;
                    m_rectTransform.anchorMin                               = Vector2.up;
                    m_rectTransform.anchoredPosition                        = new Vector2(0, -ySideOffset);

                    m_backgroundImages[0].rectTransform.anchorMax           = Vector2.one;
                    m_backgroundImages[0].rectTransform.anchorMin           = Vector2.right;
                    m_backgroundImages[0].rectTransform.anchoredPosition    = new Vector2(-xSideOffsetBackgroundImage, 0);
                    
                    break;

                case GraphyManager.ModulePosition.BOTTOM_LEFT:

                    m_rectTransform.anchorMax                               = Vector2.right;
                    m_rectTransform.anchorMin                               = Vector2.zero;
                    m_rectTransform.anchoredPosition                        = new Vector2(0, ySideOffset);

                    m_backgroundImages[0].rectTransform.anchorMax           = Vector2.up;
                    m_backgroundImages[0].rectTransform.anchorMin           = Vector2.zero;
                    m_backgroundImages[0].rectTransform.anchoredPosition    = new Vector2(xSideOffsetBackgroundImage, 0);
                    
                    break;

                case GraphyManager.ModulePosition.BOTTOM_RIGHT:

                    m_rectTransform.anchorMax                               = Vector2.right;
                    m_rectTransform.anchorMin                               = Vector2.zero;
                    m_rectTransform.anchoredPosition                        = new Vector2(0, ySideOffset);

                    m_backgroundImages[0].rectTransform.anchorMax           = Vector2.one;
                    m_backgroundImages[0].rectTransform.anchorMin           = Vector2.right;
                    m_backgroundImages[0].rectTransform.anchoredPosition    = new Vector2(-xSideOffsetBackgroundImage, 0);
                    
                    break;

                case GraphyManager.ModulePosition.FREE:
                    break;
            }

            switch (newModulePosition)
            {
                case GraphyManager.ModulePosition.TOP_LEFT:
                case GraphyManager.ModulePosition.BOTTOM_LEFT:

                    m_processorTypeText             .alignment = TextAnchor.UpperLeft;
                    m_systemMemoryText              .alignment = TextAnchor.UpperLeft;
                    m_graphicsDeviceNameText        .alignment = TextAnchor.UpperLeft;
                    m_graphicsDeviceVersion     .alignment = TextAnchor.UpperLeft;
                    m_graphicsMemorySizeText        .alignment = TextAnchor.UpperLeft;
                    m_screenResolutionText          .alignment = TextAnchor.UpperLeft;
                    m_windowResolutionText      .alignment = TextAnchor.UpperLeft;
                    m_operatingSystemText           .alignment = TextAnchor.UpperLeft;

                    break;

                case GraphyManager.ModulePosition.TOP_RIGHT:
                case GraphyManager.ModulePosition.BOTTOM_RIGHT:

                    m_processorTypeText             .alignment = TextAnchor.UpperRight;
                    m_systemMemoryText              .alignment = TextAnchor.UpperRight;
                    m_graphicsDeviceNameText        .alignment = TextAnchor.UpperRight;
                    m_graphicsDeviceVersion     .alignment = TextAnchor.UpperRight;
                    m_graphicsMemorySizeText        .alignment = TextAnchor.UpperRight;
                    m_screenResolutionText          .alignment = TextAnchor.UpperRight;
                    m_windowResolutionText      .alignment = TextAnchor.UpperRight;
                    m_operatingSystemText           .alignment = TextAnchor.UpperRight;
                    
                    break;

                case GraphyManager.ModulePosition.FREE:
                    break;
            }
            */
        }

        public void SetState(GraphyManager.ModuleState state, bool silentUpdate = false)
        {
            if (!silentUpdate)
            {
                m_previousModuleState = m_currentModuleState;
            }

            m_currentModuleState = state;

            bool active = state == GraphyManager.ModuleState.FULL
                          || state == GraphyManager.ModuleState.TEXT
                          || state == GraphyManager.ModuleState.BASIC;

            gameObject.SetActive(active);

            m_backgroundImages.SetAllActive(active && m_graphyManager.Background);
        }

        /// <summary>
        /// Restores state to the previous one.
        /// </summary>
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
            
            SetPosition(m_graphyManager.AdvancedModulePosition);
            SetState(m_graphyManager.AdvancedModuleState);
        }

        public void RefreshParameters()
        {
            foreach (var image in m_backgroundImages)
            {
                image.color = m_graphyManager.BackgroundColor;
            }

            SetPosition(m_graphyManager.AdvancedModulePosition);
            SetState(m_currentModuleState, true);
        }

        #endregion

        #region Methods -> Private

        private void Init()
        {
            //TODO: Replace this with one activated from the core and figure out the min value.
            // commented for now
            /*
            if (  !G_FloatString.Inited
                || G_FloatString.MinValue > -1000f
                || G_FloatString.MaxValue < 16384f)
            {
                G_FloatString.Init
                (
                    minNegativeValue: -1001f,
                    maxPositiveValue: 16386f
                );
            }
            */

            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_sb = new StringBuilder();

            m_rectTransform = GetComponent<RectTransform>();

            #region Section -> Text

            m_screenResolutionText      .SetText("{0} x {1} @ {2}Hz", Screen.currentResolution.width, Screen.currentResolution.height, Screen.currentResolution.refreshRate);
            m_operatingSystemText       .SetText(SystemInfo.operatingSystem + " [" + SystemInfo.deviceType + "]");
            m_graphicsDeviceVersionText .SetText(SystemInfo.graphicsDeviceVersion);
            m_processorTypeText         .SetText(SystemInfo.processorType);
            m_threadCountText           .SetText(SystemInfo.processorCount + " threads");
            m_systemMemoryText          .SetText("{0} MB", SystemInfo.systemMemorySize);
            m_graphicsDeviceNameText    .SetText(SystemInfo.graphicsDeviceName);
            m_graphicsMemorySizeText    .SetText("{0} MB", SystemInfo.graphicsMemorySize);
            m_maxTextureSizeText        .SetText("{0} px", SystemInfo.maxTextureSize);
            m_graphicsShaderLevelText   .SetText("{0}", SystemInfo.graphicsShaderLevel);

            float preferredWidth = 0;
            
            // Resize the background overlay
            
            List<TMP_Text> texts = new List<TMP_Text>()
            {
                m_screenResolutionText,
                m_operatingSystemText,
                m_graphicsDeviceVersionText,
                m_processorTypeText,
                m_threadCountText,
                m_systemMemoryText,
                m_graphicsDeviceNameText,
                m_graphicsMemorySizeText,
                m_maxTextureSizeText,
                m_graphicsShaderLevelText
            };

            foreach (var text in texts)
            {
                if (text.preferredWidth > preferredWidth)
                {
                    preferredWidth = text.preferredWidth;
                }
            }

            #endregion

            #region Section -> Rect Transform Resize

            m_valueRectTransform.sizeDelta = new Vector2
            (
                x: preferredWidth,
                y: 295
            );

            m_rectTransform.sizeDelta = new Vector2
            (
                x: m_labelRectTransform.sizeDelta.x + preferredWidth + 20,
                y: 315
            );

            #endregion

            UpdateParameters();
        }

        #endregion
    }
}