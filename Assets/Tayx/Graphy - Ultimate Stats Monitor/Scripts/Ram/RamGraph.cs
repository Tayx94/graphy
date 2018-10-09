/* ---------------------------------------
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

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

using System.Collections;
using Tayx;

namespace Tayx.Graphy.Ram
{
    public class RamGraph : Graph.Graph
    {
        /* ----- TODO: ----------------------------
         * Check if we can seal this class.
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we can remove "using System.Collections;".
         * Check if we should add "private" to the Unity Callbacks.
         * Check if we can remove "using Tayx;".
         * Check if we should add a "RequireComponent" for "RamMonitor".
         * --------------------------------------*/

        #region Variables -> Serialized Private

        [SerializeField] private    Image           m_imageAllocated;
        [SerializeField] private    Image           m_imageReserved;
        [SerializeField] private    Image           m_imageMono;

        [SerializeField] private    Shader          ShaderFull;
        [SerializeField] private    Shader          ShaderLight;

        #endregion

        #region Variables -> Private

        private                     GraphyManager   m_graphyManager;

        private                     RamMonitor      m_ramMonitor;

        private                     int             m_resolution                = 150;

        private                     ShaderGraph     m_shaderGraphAllocated;
        private                     ShaderGraph     m_shaderGraphReserved;
        private                     ShaderGraph     m_shaderGraphMono;

        private                     float[]         m_allocatedArray;
        private                     float[]         m_reservedArray;
        private                     float[]         m_monoArray;

        private                     float           m_highestMemory;

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
                    m_shaderGraphAllocated  .ArrayMaxSize = ShaderGraph.ArrayMaxSizeFull;
                    m_shaderGraphReserved   .ArrayMaxSize = ShaderGraph.ArrayMaxSizeFull;
                    m_shaderGraphMono       .ArrayMaxSize = ShaderGraph.ArrayMaxSizeFull;

                    m_shaderGraphAllocated  .Image.material = new Material(ShaderFull);
                    m_shaderGraphReserved   .Image.material = new Material(ShaderFull);
                    m_shaderGraphMono       .Image.material = new Material(ShaderFull);
                    break;

                case GraphyManager.Mode.LIGHT:
                    m_shaderGraphAllocated  .ArrayMaxSize = ShaderGraph.ArrayMaxSizeLight;
                    m_shaderGraphReserved   .ArrayMaxSize = ShaderGraph.ArrayMaxSizeLight;
                    m_shaderGraphMono       .ArrayMaxSize = ShaderGraph.ArrayMaxSizeLight;

                    m_shaderGraphAllocated  .Image.material = new Material(ShaderLight);
                    m_shaderGraphReserved   .Image.material = new Material(ShaderLight);
                    m_shaderGraphMono       .Image.material = new Material(ShaderLight);
                    break;
            }

            m_shaderGraphAllocated.InitializeShader();
            m_shaderGraphReserved.InitializeShader();
            m_shaderGraphMono.InitializeShader();

            m_resolution = m_graphyManager.RamGraphResolution;
            
            CreatePoints();
        }

        #endregion

        #region Methods -> Protected Override

        protected override void UpdateGraph()
        {
            float allocatedMemory   = m_ramMonitor.AllocatedRam;
            float reservedMemory    = m_ramMonitor.ReservedRam;
            float monoMemory        = m_ramMonitor.MonoRam;

            m_highestMemory = 0;

            for (int i = 0; i <= m_resolution - 1; i++)
            {
                if (i >= m_resolution - 1)
                {
                    m_allocatedArray[i] = allocatedMemory;
                    m_reservedArray[i]  = reservedMemory;
                    m_monoArray[i]      = monoMemory;
                }
                else
                {
                    m_allocatedArray[i] = m_allocatedArray[i + 1];
                    m_reservedArray[i]  = m_reservedArray[i + 1];
                    m_monoArray[i]      = m_monoArray[i + 1];
                }

                if (m_highestMemory < m_reservedArray[i])
                {
                    m_highestMemory = m_reservedArray[i];
                }
            }

            for (int i = 0; i <= m_resolution - 1; i++)
            {
                m_shaderGraphAllocated.Array[i] = m_allocatedArray[i] / m_highestMemory;

                m_shaderGraphReserved.Array[i]  = m_reservedArray[i] / m_highestMemory;

                m_shaderGraphMono.Array[i]      = m_monoArray[i] / m_highestMemory;
            }

            m_shaderGraphAllocated.UpdatePoints();
            m_shaderGraphReserved.UpdatePoints();
            m_shaderGraphMono.UpdatePoints();
        }

        protected override void CreatePoints()
        {

            m_shaderGraphAllocated.Array    = new float[m_resolution];
            m_shaderGraphReserved.Array     = new float[m_resolution];
            m_shaderGraphMono.Array         = new float[m_resolution];

            m_allocatedArray    = new float[m_resolution];
            m_reservedArray     = new float[m_resolution];
            m_monoArray         = new float[m_resolution];

            for (int i = 0; i < m_resolution; i++)
            {
                m_shaderGraphAllocated.Array[i] = 0;
                m_shaderGraphReserved.Array[i]  = 0;
                m_shaderGraphMono.Array[i]      = 0;
            }

            // Initialize the material values
            
            // Colors
            
            m_shaderGraphAllocated.GoodColor     = m_graphyManager.AllocatedRamColor;
            m_shaderGraphAllocated.CautionColor  = m_graphyManager.AllocatedRamColor;
            m_shaderGraphAllocated.CriticalColor = m_graphyManager.AllocatedRamColor;
            
            m_shaderGraphAllocated.UpdateColors();

            m_shaderGraphReserved.GoodColor     = m_graphyManager.ReservedRamColor;
            m_shaderGraphReserved.CautionColor  = m_graphyManager.ReservedRamColor;
            m_shaderGraphReserved.CriticalColor = m_graphyManager.ReservedRamColor;
            
            m_shaderGraphReserved.UpdateColors();
            
            m_shaderGraphMono.GoodColor     = m_graphyManager.MonoRamColor;
            m_shaderGraphMono.CautionColor  = m_graphyManager.MonoRamColor;
            m_shaderGraphMono.CriticalColor = m_graphyManager.MonoRamColor;
            
            m_shaderGraphMono.UpdateColors();

            // Thresholds
            
            m_shaderGraphAllocated.GoodThreshold    = 0;
            m_shaderGraphAllocated.CautionThreshold = 0;
            m_shaderGraphAllocated.UpdateThresholds();
            
            m_shaderGraphReserved.GoodThreshold     = 0;
            m_shaderGraphReserved.CautionThreshold  = 0;
            m_shaderGraphReserved.UpdateThresholds();
            
            m_shaderGraphMono.GoodThreshold         = 0;
            m_shaderGraphMono.CautionThreshold      = 0;
            m_shaderGraphMono.UpdateThresholds();

            m_shaderGraphAllocated.UpdateArray();
            m_shaderGraphReserved.UpdateArray();
            m_shaderGraphMono.UpdateArray();
            
            // Average
            
            m_shaderGraphAllocated.Average  = 0;
            m_shaderGraphReserved.Average   = 0;
            m_shaderGraphMono.Average       = 0;

            m_shaderGraphAllocated.UpdateAverage();
            m_shaderGraphReserved.UpdateAverage();
            m_shaderGraphMono.UpdateAverage();
        }

        #endregion

        #region Methods -> Private

        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_ramMonitor = GetComponent<RamMonitor>();
            
            m_shaderGraphAllocated  = new ShaderGraph();
            m_shaderGraphReserved   = new ShaderGraph();
            m_shaderGraphMono       = new ShaderGraph();

            m_shaderGraphAllocated  .Image = m_imageAllocated;
            m_shaderGraphReserved   .Image = m_imageReserved;
            m_shaderGraphMono       .Image = m_imageMono;
            
            UpdateParameters();
        }

        #endregion
    }
}