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
        [SerializeField] private Image m_imageGraphHighestValues;

        private int m_resolution = 40;

        private ShaderGraph m_shaderGraph;
        private ShaderGraph m_shaderGraphHighestValues;

        public Shader ShaderFull;
        public Shader ShaderLight;

        private float[] m_graphArray;
        private float[] m_graphArrayHighestValue;

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

                    m_shaderGraphHighestValues.ArrayMaxSize = ShaderGraph.ArrayMaxSizeFull;
                    m_shaderGraphHighestValues.Image.material = new Material(ShaderFull);
                    break;

                case GraphyManager.Mode.LIGHT:
                    m_shaderGraph.ArrayMaxSize = ShaderGraph.ArrayMaxSizeLight;
                    m_shaderGraph.Image.material = new Material(ShaderLight);

                    m_shaderGraphHighestValues.ArrayMaxSize = ShaderGraph.ArrayMaxSizeLight;
                    m_shaderGraphHighestValues.Image.material = new Material(ShaderLight);
                    break;
            }

            m_shaderGraph.InitializeShader();
            m_shaderGraphHighestValues.InitializeShader();

            m_resolution = m_graphyManager.AudioGraphResolution;

            CreatePoints();
        }

        #endregion

        #region Private Methods

        protected override void UpdateGraph()
        {
            int incrementPerIteration = Mathf.FloorToInt(m_audioMonitor.Spectrum.Length / (float)m_resolution);

            // Current values -------------------------

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


            // Highest values -------------------------

            for (int i = 0; i <= m_resolution - 1; i++)
            {
                float currentValue = 0;

                for (int j = 0; j < incrementPerIteration; j++)
                {
                    currentValue += m_audioMonitor.SpectrumHighestValues[i * incrementPerIteration + j];
                }

                // Uses 3 values for each bar to accomplish that look

                if ((i + 1) % 3 == 0 && i > 1)
                {
                    float value =
                    (
                        m_audioMonitor.dBNormalized(m_audioMonitor.lin2dB(currentValue / incrementPerIteration))
                        + m_graphArrayHighestValue[i - 1]
                        + m_graphArrayHighestValue[i - 2]
                    ) / 3;

                    m_graphArrayHighestValue[i] = value;
                    m_graphArrayHighestValue[i - 1] = value;
                    m_graphArrayHighestValue[i - 2] = -1; // Always set the third one to -1 to leave gaps in the graph and improve readability
                }
                else
                {
                    m_graphArrayHighestValue[i] = m_audioMonitor.dBNormalized(m_audioMonitor.lin2dB(currentValue / incrementPerIteration));
                }
            }

            for (int i = 0; i <= m_resolution - 1; i++)
            {
                m_shaderGraphHighestValues.Array[i] = m_graphArrayHighestValue[i];
            }

            m_shaderGraphHighestValues.UpdatePoints();

        }

        protected override void CreatePoints()
        {
            // Init Arrays

            m_shaderGraph.Array = new float[m_resolution];
            m_shaderGraphHighestValues.Array = new float[m_resolution];

            m_graphArray = new float[m_resolution];
            m_graphArrayHighestValue = new float[m_resolution];

            for (int i = 0; i < m_resolution; i++)
            {
                m_shaderGraph.Array[i] = 0;
                m_shaderGraphHighestValues.Array[i] = 0;
            }

            // Color

            m_shaderGraph.GoodColor     = m_graphyManager.AudioGraphColor;
            m_shaderGraph.CautionColor  = m_graphyManager.AudioGraphColor;
            m_shaderGraph.CriticalColor = m_graphyManager.AudioGraphColor;

            m_shaderGraph.UpdateColors();

            m_shaderGraphHighestValues.GoodColor = m_graphyManager.AudioGraphColor;
            m_shaderGraphHighestValues.CautionColor = m_graphyManager.AudioGraphColor;
            m_shaderGraphHighestValues.CriticalColor = m_graphyManager.AudioGraphColor;
                        
            m_shaderGraphHighestValues.UpdateColors();

            // Threshold

            m_shaderGraph.GoodThreshold = 0;
            m_shaderGraph.CautionThreshold = 0;
            
            m_shaderGraph.UpdateThresholds();

            m_shaderGraphHighestValues.GoodThreshold = 0;
            m_shaderGraphHighestValues.CautionThreshold = 0;

            m_shaderGraphHighestValues.UpdateThresholds();

            // Update Array

            m_shaderGraph.UpdateArray();

            m_shaderGraphHighestValues.UpdateArray();

            // Average

            m_shaderGraph.Average = 0;

            m_shaderGraph.UpdateAverage();
            
            m_shaderGraphHighestValues.Average = 0;

            m_shaderGraphHighestValues.UpdateAverage();
        }


        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_audioMonitor = GetComponent<AudioMonitor>();
            
            m_shaderGraph = new ShaderGraph();

            m_shaderGraph.Image = m_imageGraph;

            m_shaderGraphHighestValues = new ShaderGraph();

            m_shaderGraphHighestValues.Image = m_imageGraphHighestValues;

            UpdateParameters();
        }

        #endregion

    }
}