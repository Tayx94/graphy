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

using UnityEngine;
using System.Runtime.CompilerServices;

namespace Tayx.Graphy.Fps
{
    public class G_FpsMonitor : MonoBehaviour
    {
        /* ----- TODO: ----------------------------
         * Add summaries to the variables.
         * Add summaries to the functions.
         * --------------------------------------*/

        #region Variables -> Serialized Private

        [SerializeField] private    int             m_averageSamples            = 200;

        #endregion

        #region Variables -> Private

        private GraphyManager                       m_graphyManager;

        private                     float           m_currentFps                = 0f;
        private                     float           m_avgFps                    = 0f;
        private                     float           m_minFps                    = 0f;
        private                     float           m_maxFps                    = 0f;

        private                     float[]         m_averageFpsSamples;
        private                     int             m_avgFpsSamplesCapacity     = 0;
        private                     int             m_avgFpsSamplesCount        = 0;
        private                     int             m_timeToResetMinMaxFps      = 10;
        private                     int             m_indexSample               = 0;

        private                     float           m_timeToResetMinFpsPassed   = 0f;
        private                     float           m_timeToResetMaxFpsPassed   = 0f;

        private                     float           m_unscaledDeltaTime         = 0f;

        #endregion

        #region Properties -> Public

        public                      float           CurrentFPS => m_currentFps;
        public                      float           AverageFPS => m_avgFps;

        public                      float           MinFPS => m_minFps;
        public                      float           MaxFPS => m_maxFps;

        #endregion

        #region Methods -> Unity Callbacks

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            m_unscaledDeltaTime = Time.unscaledDeltaTime;

            m_timeToResetMinFpsPassed += m_unscaledDeltaTime;
            m_timeToResetMaxFpsPassed += m_unscaledDeltaTime;

            // Update fps and ms

            m_currentFps = 1 / m_unscaledDeltaTime;

            // Update avg fps

            m_avgFps = 0;

            m_indexSample++;

            if ( m_indexSample >= m_avgFpsSamplesCapacity ) m_indexSample = 0;

            m_averageFpsSamples[ m_indexSample] = m_currentFps;
            
            if (m_avgFpsSamplesCount < m_avgFpsSamplesCapacity)
            {
                m_avgFpsSamplesCount++;
            }

            for (int i = 0; i < m_avgFpsSamplesCount; i++)
            {
                m_avgFps += m_averageFpsSamples[i];
            }

            m_avgFps /= (float)m_avgFpsSamplesCount;

            // Checks to reset min and max fps

            if (    m_timeToResetMinMaxFps    > 0 
                &&  m_timeToResetMinFpsPassed > m_timeToResetMinMaxFps)
            {
                m_minFps = float.MaxValue;
                m_timeToResetMinFpsPassed = 0;
            }

            if (    m_timeToResetMinMaxFps    > 0 
                &&  m_timeToResetMaxFpsPassed > m_timeToResetMinMaxFps)
            {
                m_maxFps = float.MinValue;
                m_timeToResetMaxFpsPassed = 0;
            }

            // Update min fps

            if (m_currentFps < m_minFps)
            {
                m_minFps = m_currentFps;

                m_timeToResetMinFpsPassed = 0;
            }

            // Update max fps

            if (m_currentFps > m_maxFps)
            {
                m_maxFps = m_currentFps;

                m_timeToResetMaxFpsPassed = 0;
            }
        }

        #endregion

        #region Methods -> Public

        public void UpdateParameters()
        {
            m_timeToResetMinMaxFps = m_graphyManager.TimeToResetMinMaxFps;
        }

        #endregion

        #region Methods -> Private

        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            ResizeSamplesBuffer(m_averageSamples);
            
            UpdateParameters();
        }

        
        private void ResizeSamplesBuffer(int size)
        {
            m_avgFpsSamplesCapacity = Mathf.NextPowerOfTwo(size);

            m_averageFpsSamples = new float[m_avgFpsSamplesCapacity];
        }
        
        #endregion
    }
}
