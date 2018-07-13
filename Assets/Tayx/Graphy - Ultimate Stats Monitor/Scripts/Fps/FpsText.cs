/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 22-Nov-17
 * Studio: Tayx
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Text;
using Tayx.Graphy.Utils;

namespace Tayx.Graphy.Fps
{
    public class FpsText : MonoBehaviour
    {
        #region Private Variables

        private GraphyManager m_graphyManager;

        private FpsMonitor m_fpsMonitor;

        [SerializeField] private Text m_fpsText;
        [SerializeField] private Text m_msText;

        [SerializeField] private Text m_avgFpsText;
        [SerializeField] private Text m_minFpsText;
        [SerializeField] private Text m_maxFpsText;

        private int m_updateRate = 4;  // 4 updates per sec.

        private int m_frameCount = 0;

        private float m_deltaTime = 0;

        private float m_fps = 0;

        private const string m_msStringFormat = "0.0";

        #endregion

        #region Unity Methods

        void Awake()
        {
            Init();
        }

        void Update()
        {
            m_deltaTime += Time.unscaledDeltaTime;

            m_frameCount++;

            // Only update texts 'm_updateRate' times per second

            if (m_deltaTime > 1.0 / m_updateRate)
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

                //m_deltaTime -= 1.0f / m_updateRate;
                m_deltaTime = 0;
                m_frameCount = 0;
            }
        }

        #endregion
        
        #region Public Methods

        public void UpdateParameters()
        {
            m_updateRate = m_graphyManager.FpsTextUpdateRate;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Assigns color to a text according to their fps numeric value and
        /// the colors specified in the 3 categories (Good, Caution, Critical).
        /// </summary>
        /// <param name="text">UI Text component to change its color</param>
        /// <param name="fps">Numeric fps value</param>
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
            if (!FloatString.Inited || FloatString.minValue > -1000f || FloatString.maxValue < 16384f)
            {
                FloatString.Init(-1001f, 16386f);
            }

            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_fpsMonitor = GetComponent<FpsMonitor>();
            
            UpdateParameters();
        }

        #endregion
    }
}
