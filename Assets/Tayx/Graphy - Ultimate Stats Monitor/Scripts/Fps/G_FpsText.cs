/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            22-Nov-17
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;
using Tayx.Graphy.Utils.NumString;

namespace Tayx.Graphy.Fps
{
    public class G_FpsText : MonoBehaviour
    {
        /* ----- TODO: ----------------------------
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we should add a "RequireComponent" for "FpsMonitor".
         * Improve the IntString Init to come from the core instead.
         * --------------------------------------*/

        #region Variables -> Serialized Private

        [SerializeField] private    Text            m_fpsText           = null;
        [SerializeField] private    Text            m_msText            = null;

        [SerializeField] private    Text            m_avgFpsText        = null;
        [SerializeField] private    Text            m_minFpsText        = null;
        [SerializeField] private    Text            m_maxFpsText        = null;

        #endregion

        #region Variables -> Private

        private                     GraphyManager   m_graphyManager     = null;

        private                     G_FpsMonitor    m_fpsMonitor        = null;

        private                     int             m_updateRate        = 4;  // 4 updates per sec.

        private                     int             m_frameCount        = 0;

        private                     float           m_deltaTime         = 0f;

        private                     float           m_fps               = 0f;

        private const               int             m_minFps            = 0;
        private const               int             m_maxFps            = 10000;

        private const               string          m_msStringFormat    = "0.0";

        #endregion

        #region Methods -> Unity Callbacks

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            m_deltaTime += Time.unscaledDeltaTime;

            m_frameCount++;

            // Only update texts 'm_updateRate' times per second

            if (m_deltaTime > 1f / m_updateRate)
            {
                m_fps = m_frameCount / m_deltaTime;

                // Update fps and ms

                m_fpsText.text = m_fps.ToInt().ToStringNonAlloc();
                m_msText.text = (m_deltaTime / m_frameCount * 1000f).ToStringNonAlloc(m_msStringFormat);

                // Update min fps

                m_minFpsText.text = m_fpsMonitor.MinFPS.ToInt().ToStringNonAlloc();

                SetFpsRelatedTextColor(m_minFpsText, m_fpsMonitor.MinFPS);

                // Update max fps

                m_maxFpsText.text = m_fpsMonitor.MaxFPS.ToInt().ToStringNonAlloc();

                SetFpsRelatedTextColor(m_maxFpsText, m_fpsMonitor.MaxFPS);

                // Update avg fps

                m_avgFpsText.text = m_fpsMonitor.AverageFPS.ToInt().ToStringNonAlloc();

                SetFpsRelatedTextColor(m_avgFpsText, m_fpsMonitor.AverageFPS);

                // Reset variables

                m_deltaTime = 0f;
                m_frameCount = 0;
            }
        }

        #endregion
        
        #region Methods -> Public

        public void UpdateParameters()
        {
            m_updateRate = m_graphyManager.FpsTextUpdateRate;
        }

        #endregion

        #region Methods -> Private

        /// <summary>
        /// Assigns color to a text according to their fps numeric value and
        /// the colors specified in the 3 categories (Good, Caution, Critical).
        /// </summary>
        /// 
        /// <param name="text">
        /// UI Text component to change its color
        /// </param>
        /// 
        /// <param name="fps">
        /// Numeric fps value
        /// </param>
        private void SetFpsRelatedTextColor(Text text, float fps)
        {
            if (fps > m_graphyManager.GoodFPSThreshold)
            {
                text.color = m_graphyManager.GoodFPSColor;
            }
            else if (fps > m_graphyManager.CautionFPSThreshold)
            {
                text.color = m_graphyManager.CautionFPSColor;
            }
            else
            {
                text.color = m_graphyManager.CriticalFPSColor;
            }
        }

        private void Init()
        {
            //TODO: Replace this with one activated from the core and figure out the min value.
            if (!G_IntString.Inited || G_IntString.MinValue > m_minFps || G_IntString.MaxValue < m_maxFps)
            {
                G_IntString.Init
                (
                    minNegativeValue: m_minFps,
                    maxPositiveValue: m_maxFps
                );
            }

            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_fpsMonitor = GetComponent<G_FpsMonitor>();
            
            UpdateParameters();
        }

        #endregion
    }
}
