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
using Tayx.Graphy.Utils.NumString;

namespace Tayx.Graphy.Audio
{
    public class G_AudioText : MonoBehaviour
    {
        /* ----- TODO: ----------------------------
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we should add a "RequireComponent" for "AudioMonitor".
         * Improve the FloatString Init to come from the core instead.
         * --------------------------------------*/

        #region Variables -> Serialized Private

        [SerializeField] private    Text            m_DBText            = null;

        #endregion

        #region Variables -> Private

        private                     GraphyManager   m_graphyManager     = null;

        private                     G_AudioMonitor  m_audioMonitor      = null;

        private                     int             m_updateRate        = 4;

        private                     float           m_deltaTimeOffset   = 0;

        #endregion

        #region Methods -> Unity Callbacks

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (m_audioMonitor.SpectrumDataAvailable)
            {
                if (m_deltaTimeOffset > 1f / m_updateRate)
                {
                    m_deltaTimeOffset = 0f;

                    m_DBText.text = Mathf.Clamp(m_audioMonitor.MaxDB, -80f, 0f).ToStringNonAlloc();
                }
                else
                {
                    m_deltaTimeOffset += Time.deltaTime;
                }
            }
        }

        #endregion

        #region Methods -> Public

        public void UpdateParameters()
        {
            m_updateRate = m_graphyManager.AudioTextUpdateRate;
        }

        #endregion

        #region Methods -> Private

        private void Init()
        {
            //TODO: Replace this with one activated from the core and figure out the min value.
            if (!G_FloatString.Inited || G_FloatString.MinValue > -1000f || G_FloatString.MaxValue < 16384f)
            {
                G_FloatString.Init
                (
                    minNegativeValue: -1001f,
                    maxPositiveValue: 16386f
                );
            }

            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_audioMonitor = GetComponent<G_AudioMonitor>();
                       
            UpdateParameters();
        }

        #endregion
    }
}