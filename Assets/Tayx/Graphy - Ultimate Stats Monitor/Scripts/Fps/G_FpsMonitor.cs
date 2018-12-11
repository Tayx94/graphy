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
using System.Collections.Generic;

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

        private GraphyManager m_graphyManager;

        private                     float           m_currentFps                = 0f;
        private                     float           m_avgFps                    = 0f;
        private                     float           m_minFps                    = 0f;
        private                     float           m_maxFps                    = 0f;

        private                     List<float>     m_averageFpsSamples;

        private                     int             m_timeToResetMinMaxFps      = 10;

        private                     float           m_timeToResetMinFpsPassed   = 0f;
        private                     float           m_timeToResetMaxFpsPassed   = 0f;

        private                     float           unscaledDeltaTime           = 0f;

        #endregion

        #region Properties -> Public

        public                      float           CurrentFPS  { get { return m_currentFps; } }
        public                      float           AverageFPS  { get { return m_avgFps;} }

        public                      float           MinFPS      { get { return m_minFps;} }
        public                      float           MaxFPS      { get { return m_maxFps;} }
        
        #endregion

        #region Methods -> Unity Callbacks

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            unscaledDeltaTime = Time.unscaledDeltaTime;

            m_timeToResetMinFpsPassed += unscaledDeltaTime;
            m_timeToResetMaxFpsPassed += unscaledDeltaTime;

            // Update fps and ms

            m_currentFps = 1 / unscaledDeltaTime;

            // Update avg fps

            m_avgFps = 0;

            if (m_averageFpsSamples.Count >= m_averageSamples)
            {
                m_averageFpsSamples.Add(m_currentFps);
                m_averageFpsSamples.RemoveAt(0);
            }
            else
            {
                m_averageFpsSamples.Add(m_currentFps);
            }

            for (int i = 0; i < m_averageFpsSamples.Count; i++)
            {
                m_avgFps += m_averageFpsSamples[i];
            }

            m_avgFps /= m_averageSamples;

            // Checks to reset min and max fps

            if (    m_timeToResetMinMaxFps    > 0 
                &&  m_timeToResetMinFpsPassed > m_timeToResetMinMaxFps)
            {
                m_minFps = 0;
                m_timeToResetMinFpsPassed = 0;
            }

            if (    m_timeToResetMinMaxFps    > 0 
                &&  m_timeToResetMaxFpsPassed > m_timeToResetMinMaxFps)
            {
                m_maxFps = 0;
                m_timeToResetMaxFpsPassed = 0;
            }

            // Update min fps

            if (m_currentFps < m_minFps || m_minFps <= 0)
            {
                m_minFps = m_currentFps;

                m_timeToResetMinFpsPassed = 0;
            }

            // Update max fps

            if (m_currentFps > m_maxFps || m_maxFps <= 0)
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

            m_averageFpsSamples = new List<float>();
            
            UpdateParameters();
        }

        #endregion
    }
}