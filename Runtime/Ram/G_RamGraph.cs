/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            15-Dec-17
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using Tayx.Graphy.Graph;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace Tayx.Graphy.Ram
{
    public class G_RamGraph : G_Graph
    {
        #region Variables -> Serialized Private

        [SerializeField] private    Image           m_imageAllocated = null;
        [SerializeField] private    Image           m_imageReserved = null;
        [SerializeField] private    Image           m_imageMono = null;

        [SerializeField] private    Shader          ShaderFull = null;
        [SerializeField] private    Shader          ShaderLight = null;

        [SerializeField] private    bool            m_isInitialized = false;

        #endregion

        #region Variables -> Private

        private                     GraphyManager   m_graphyManager = null;

        private                     G_RamMonitor    m_ramMonitor = null;

        private                     int             m_resolution                = 150;

        private                     G_GraphShader   m_shaderGraphAllocated = null;
        private                     G_GraphShader   m_shaderGraphReserved = null;
        private                     G_GraphShader   m_shaderGraphMono = null;

        private                     float[]         m_allocatedArray;
        private                     float[]         m_reservedArray;
        private                     float[]         m_monoArray;

        private                     float           m_highestMemory = 0;

        #endregion

        #region Methods -> Unity Callbacks

        private void Update()
        {
            UpdateGraph();
        }

        #endregion
        
        #region Methods -> Public

        public void UpdateParameters()
        { 
            if (    m_shaderGraphAllocated  == null
                ||  m_shaderGraphReserved   == null
                ||  m_shaderGraphMono       == null)
            {
                /*
                 * Note: this is fine, since we don't much
                 * care what granularity we use if the graph
                 * has not been initialized, i.e. it's disabled.
                 * There is no chance that for some reason 
                 * parameters will not stay up to date if
                 * at some point in the future the graph is enabled:
                 * at the end of Init(), UpdateParameters() is
                 * called again.
                 */
                return;
            }
             
            switch (m_graphyManager.GraphyMode)
            {
                case GraphyManager.Mode.FULL:
                    m_shaderGraphAllocated  .ArrayMaxSize = G_GraphShader.ArrayMaxSizeFull;
                    m_shaderGraphReserved   .ArrayMaxSize = G_GraphShader.ArrayMaxSizeFull;
                    m_shaderGraphMono       .ArrayMaxSize = G_GraphShader.ArrayMaxSizeFull;

                    m_shaderGraphAllocated  .Image.material = new Material(ShaderFull);
                    m_shaderGraphReserved   .Image.material = new Material(ShaderFull);
                    m_shaderGraphMono       .Image.material = new Material(ShaderFull);
                    break;

                case GraphyManager.Mode.LIGHT:
                    m_shaderGraphAllocated  .ArrayMaxSize = G_GraphShader.ArrayMaxSizeLight;
                    m_shaderGraphReserved   .ArrayMaxSize = G_GraphShader.ArrayMaxSizeLight;
                    m_shaderGraphMono       .ArrayMaxSize = G_GraphShader.ArrayMaxSizeLight;

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
            // Since we no longer initialize by default OnEnable(), 
            // we need to check here, and Init() if needed
            if (!m_isInitialized)
            {
                Init();
            }

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
                m_shaderGraphAllocated.ShaderArrayValues[i] = m_allocatedArray[i] / m_highestMemory;

                m_shaderGraphReserved.ShaderArrayValues[i]  = m_reservedArray[i] / m_highestMemory;

                m_shaderGraphMono.ShaderArrayValues[i]      = m_monoArray[i] / m_highestMemory;
            }

            m_shaderGraphAllocated.UpdatePoints();
            m_shaderGraphReserved.UpdatePoints();
            m_shaderGraphMono.UpdatePoints();
        }

        protected override void CreatePoints()
        {
            if (m_shaderGraphAllocated.ShaderArrayValues == null || m_shaderGraphAllocated.ShaderArrayValues.Length != m_resolution)
            {
                m_allocatedArray                = new float[m_resolution];
                m_reservedArray                 = new float[m_resolution];
                m_monoArray                     = new float[m_resolution];

                m_shaderGraphAllocated.ShaderArrayValues    = new float[m_resolution];
                m_shaderGraphReserved.ShaderArrayValues     = new float[m_resolution];
                m_shaderGraphMono.ShaderArrayValues         = new float[m_resolution];
            }

            for (int i = 0; i < m_resolution; i++)
            {
                m_shaderGraphAllocated.ShaderArrayValues[i] = 0;
                m_shaderGraphReserved.ShaderArrayValues[i]  = 0;
                m_shaderGraphMono.ShaderArrayValues[i]      = 0;
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

            m_ramMonitor = GetComponent<G_RamMonitor>();
            
            m_shaderGraphAllocated  = new G_GraphShader();
            m_shaderGraphReserved   = new G_GraphShader();
            m_shaderGraphMono       = new G_GraphShader();

            m_shaderGraphAllocated  .Image = m_imageAllocated;
            m_shaderGraphReserved   .Image = m_imageReserved;
            m_shaderGraphMono       .Image = m_imageMono;
            
            UpdateParameters();

            m_isInitialized = true;
        }

        #endregion
    }
}