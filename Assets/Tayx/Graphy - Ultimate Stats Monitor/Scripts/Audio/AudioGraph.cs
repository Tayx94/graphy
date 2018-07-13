/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 15-Dec-17
 * Studio: Tayx
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Tayx;

namespace Tayx.Graphy.Audio
{
    public class AudioGraph : Graph.Graph
    {
        #region Private Variables

        private GraphyManager m_graphyManager;

        private AudioMonitor m_audioMonitor;

        [SerializeField] private Image m_imageGraph;

        private int m_resolution = 40;

        private ShaderGraph m_shaderGraph;

        public Shader ShaderFull;
        public Shader ShaderLight;

        private float[] m_graphArray;

        #endregion

        #region Unity Methods
        
        void Awake()
        {
            Init();
        }

        void Update()
        {
            if (m_audioMonitor.SpectrumDataAvailable)
            {
                UpdateGraph();
            }
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

            m_resolution = m_graphyManager.AudioGraphResolution;

            CreatePoints();
        }

        #endregion

        #region Private Methods

        protected override void UpdateGraph()
        {
            int incrementPerIteration = Mathf.FloorToInt(m_audioMonitor.Spectrum.Length / (float)m_resolution);

            for (int i = 0; i <= m_resolution - 1; i++)
            {
                float currentValue = 0;

                for (int j = 0; j < incrementPerIteration; j++)
                {
                    currentValue += m_audioMonitor.Spectrum[i * incrementPerIteration + j];
                }

                // Uses 3 values for each bar to accomplish that look

                if ((i + 1) % 3 == 0 && i > 1)
                {
                    float value =
                    (
                        m_audioMonitor.dBNormalized(m_audioMonitor.lin2dB(currentValue / incrementPerIteration))
                        + m_graphArray[i - 1]
                        + m_graphArray[i - 2]
                    ) / 3;

                    m_graphArray[i]     = value;
                    m_graphArray[i - 1] = value;
                    m_graphArray[i - 2] = -1; // Always set the third one to -1 to leave gaps in the graph and improve readability
                }
                else
                {
                    m_graphArray[i] = m_audioMonitor.dBNormalized(m_audioMonitor.lin2dB(currentValue / incrementPerIteration));
                }
            }

            for (int i = 0; i <= m_resolution - 1; i++)
            {
                m_shaderGraph.Array[i] = m_graphArray[i];
            }

            m_shaderGraph.UpdatePoints();

        }

        protected override void CreatePoints()
        {
            m_shaderGraph.Array = new float[m_resolution];

            m_graphArray = new float[m_resolution];

            for (int i = 0; i < m_resolution; i++)
            {
                m_shaderGraph.Array[i] = 0;
            }

            m_shaderGraph.GoodColor     = m_graphyManager.AudioGraphColor;
            m_shaderGraph.CautionColor  = m_graphyManager.AudioGraphColor;
            m_shaderGraph.CriticalColor = m_graphyManager.AudioGraphColor;
            
            m_shaderGraph.UpdateColors();
            
            m_shaderGraph.GoodThreshold = 0;
            m_shaderGraph.CautionThreshold = 0;
            
            m_shaderGraph.UpdateThresholds();
           
            m_shaderGraph.UpdateArray();

            m_shaderGraph.Average = 0;

            m_shaderGraph.UpdateAverage();
        }


        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_audioMonitor = GetComponent<AudioMonitor>();
            
            m_shaderGraph = new ShaderGraph();

            m_shaderGraph.Image = m_imageGraph;

            UpdateParameters();
        }

        #endregion

    }
}