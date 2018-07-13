/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 05-Dec-17
 * Studio: Tayx
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
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
        #region Private Variables

        private GraphyManager m_graphyManager;

        private FpsMonitor m_fpsMonitor;

        [SerializeField] private Image m_imageGraph;

        private int m_resolution = 150;

        private ShaderGraph m_shaderGraph;

        public Shader ShaderFull;
        public Shader ShaderLight;

        private int[] m_fpsArray;

        private int m_highestFps;

        #endregion

        #region Unity Methods

        void Awake()
        {
            Init();
        }

        void Update()
        {
            UpdateGraph();
        }

        #endregion
        
        #region Public Methods
        
        public void UpdateParameters()
        {
            switch (m_graphyManager.GraphyMode)
            {
                case GraphyManager.Mode.FULL:
                    m_shaderGraph.ArrayMaxSize = ShaderGraph.ArrayMaxSizeFull;
                    m_shaderGraph.Image.material = new Material(ShaderFull);
                    break;

                case GraphyManager.Mode.LIGHT:
                    m_shaderGraph.ArrayMaxSize = ShaderGraph.ArrayMaxSizeLight;
                    m_shaderGraph.Image.material = new Material(ShaderLight);
                    break;
            }

            m_shaderGraph.InitializeShader();

            m_resolution = m_graphyManager.FpsGraphResolution;
            
            CreatePoints();
        }
        
        #endregion

        #region Private Methods

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
                m_shaderGraph.Array[i] = m_fpsArray[i] / (float) m_highestFps;
            }

            // Update the material values

            m_shaderGraph.UpdatePoints();

            m_shaderGraph.Average = m_fpsMonitor.AverageFPS / m_highestFps;

            m_shaderGraph.UpdateAverage();

            m_shaderGraph.GoodThreshold = (float)m_graphyManager.GoodFPSThreshold / m_highestFps;
            m_shaderGraph.CautionThreshold = (float)m_graphyManager.CautionFPSThreshold / m_highestFps;
            
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

        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_fpsMonitor = GetComponent<FpsMonitor>();
            
            m_shaderGraph = new ShaderGraph();

            m_shaderGraph.Image = m_imageGraph;
            
            UpdateParameters();
        }

        #endregion
    }
}