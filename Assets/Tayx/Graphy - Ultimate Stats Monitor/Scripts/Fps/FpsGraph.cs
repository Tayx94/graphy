﻿/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            15-Dec-17
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Runtime.CompilerServices;
using Tayx;

namespace Tayx.Graphy.Fps
{
    public class FpsGraph : Graph.Graph
    {
        /* ----- TODO: ----------------------------
         * Check if we can seal this class.
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we can remove "using System.Collections;".
         * Check if we should add "private" to the Unity Callbacks.
         * Check if we can remove "using System.Runtime.CompilerServices;".
         * Check if we can remove "using Tayx;".
         * Check if we should add a "RequireComponent" for "FpsMonitor".
         * --------------------------------------*/

        #region Variables -> Serialized Private

        [SerializeField] private    Image           m_imageGraph;

        [SerializeField] private    Shader          ShaderFull;
        [SerializeField] private    Shader          ShaderLight;

        #endregion

        #region Variables -> Private

        private GraphyManager   m_graphyManager;

        private                     FpsMonitor      m_fpsMonitor;

        private                     int             m_resolution        = 150;

        private                     ShaderGraph     m_shaderGraph;

        private                     int[]           m_fpsArray;

        private                     int             m_highestFps;

        #endregion

        #region Methods -> Unity Callbacks

        void Awake()
        {
            Init();
        }

        void Update()
        {
            UpdateGraph();
        }

        #endregion
        
        #region Methods -> Public
        
        public void UpdateParameters()
        {
            switch (m_graphyManager.GraphyMode)
            {
                case GraphyManager.Mode.FULL:
                    m_shaderGraph.ArrayMaxSize      = ShaderGraph.ArrayMaxSizeFull;
                    m_shaderGraph.Image.material    = new Material(ShaderFull);
                    break;

                case GraphyManager.Mode.LIGHT:
                    m_shaderGraph.ArrayMaxSize      = ShaderGraph.ArrayMaxSizeLight;
                    m_shaderGraph.Image.material    = new Material(ShaderLight);
                    break;
            }

            m_shaderGraph.InitializeShader();

            m_resolution = m_graphyManager.FpsGraphResolution;
            
            CreatePoints();
        }
        
        #endregion

        #region Methods -> Protected Override

        protected override void UpdateGraph()
        {
            int fps = (int)(1 / Time.unscaledDeltaTime);

            int currentMaxFps = 0;

            for (int i = 0; i <= m_resolution - 1; i++)
            {
                if (i >= m_resolution - 1)
                {
                    m_fpsArray[i] = fps;
                }
                else
                {
                    m_fpsArray[i] = m_fpsArray[i + 1];
                }

                // Store the highest fps to use as the highest point in the graph

                if (currentMaxFps < m_fpsArray[i])
                {
                    currentMaxFps = m_fpsArray[i];
                }

            }

            m_highestFps = m_highestFps < 1 || m_highestFps <= currentMaxFps ? currentMaxFps : m_highestFps - 1;

            for (int i = 0; i <= m_resolution - 1; i++)
            {
                m_shaderGraph.Array[i]      = m_fpsArray[i] / (float) m_highestFps;
            }

            // Update the material values

            m_shaderGraph.UpdatePoints();

            m_shaderGraph.Average           = m_fpsMonitor.AverageFPS / m_highestFps;
            m_shaderGraph.UpdateAverage();

            m_shaderGraph.GoodThreshold     = (float)m_graphyManager.GoodFPSThreshold / m_highestFps;
            m_shaderGraph.CautionThreshold  = (float)m_graphyManager.CautionFPSThreshold / m_highestFps;
            m_shaderGraph.UpdateThresholds();
        }

        protected override void CreatePoints()
        {
            m_shaderGraph.Array = new float[m_resolution];

            m_fpsArray = new int[m_resolution];

            for (int i = 0; i < m_resolution; i++)
            {
                m_shaderGraph.Array[i] = 0;
            }

            m_shaderGraph.GoodColor     = m_graphyManager.GoodFPSColor;
            m_shaderGraph.CautionColor  = m_graphyManager.CautionFPSColor;
            m_shaderGraph.CriticalColor = m_graphyManager.CriticalFPSColor;
            
            m_shaderGraph.UpdateColors();
            
            m_shaderGraph.UpdateArray();
        }

        #endregion

        #region Methods -> Private

        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_fpsMonitor    = GetComponent<FpsMonitor>();

            m_shaderGraph   = new ShaderGraph
            {
                Image       = m_imageGraph
            };

            UpdateParameters();
        }

        #endregion
    }
}