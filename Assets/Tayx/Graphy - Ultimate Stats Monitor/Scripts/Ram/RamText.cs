/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            05-Dec-17
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Text;
using UnityEngine.Events;
using Tayx.Graphy.Utils;
using Tayx.Graphy.Utils.NumString;

namespace Tayx.Graphy.Ram
{
    public class RamText : MonoBehaviour
    {
        /* ----- TODO: ----------------------------
         * Check if we can seal this class.
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we can remove "using System.Collections;".
         * Check if we should add "private" to the Unity Callbacks.
         * Check if we can remove "using System.Text;".
         * Check if we can remove "using Tayx.Graphy.Utils;".
         * Check if we can remove "UnityEngine.Events;".
         * Check if we should add a "RequireComponent" for "RamMonitor".
         * Improve the FloatString Init to come from the core instead.
         * --------------------------------------*/

        #region Variables -> Serialized Private

        [SerializeField] private    Text            m_allocatedSystemMemorySizeText;
        [SerializeField] private    Text            m_reservedSystemMemorySizeText;
        [SerializeField] private    Text            m_monoSystemMemorySizeText;

        #endregion

        #region Variables -> Private

        private                     GraphyManager   m_graphyManager;

        private                     RamMonitor      m_ramMonitor;

        private                     float           m_updateRate                            = 4f;  // 4 updates per sec.

        private                     float           m_deltaTime                             = 0.0f;

        private readonly            string          m_memoryStringFormat                    = "0.0";

        #endregion

        #region Methods -> Unity Callbacks

        void Awake()
        {
            Init();
        }

        void Update()
        {
            m_deltaTime += Time.unscaledDeltaTime;

            if (m_deltaTime > 1f / m_updateRate)
            {
                // Update allocated, mono and reserved memory
                m_allocatedSystemMemorySizeText .text = m_ramMonitor.AllocatedRam.ToStringNonAlloc(m_memoryStringFormat);
                m_reservedSystemMemorySizeText  .text = m_ramMonitor.ReservedRam.ToStringNonAlloc(m_memoryStringFormat);
                m_monoSystemMemorySizeText      .text = m_ramMonitor.MonoRam.ToStringNonAlloc(m_memoryStringFormat);

                m_deltaTime                     = 0f;
            }
        }

        #endregion
        
        #region Methods -> Public
        
        public void UpdateParameters()
        {
            m_allocatedSystemMemorySizeText .color = m_graphyManager.AllocatedRamColor;
            m_reservedSystemMemorySizeText  .color = m_graphyManager.ReservedRamColor;
            m_monoSystemMemorySizeText      .color = m_graphyManager.MonoRamColor;

            m_updateRate                    = m_graphyManager.RamTextUpdateRate;
        }
        
        #endregion

        #region Methods -> Private

        private void Init()
        {
            //TODO: Replace this with one activated from the core and figure out the min value.
            if (!FloatString.Inited || FloatString.MinValue > -1000f || FloatString.MaxValue < 16384f)
            {
                FloatString.Init
                (
                    minNegativeValue: -1001f,
                    maxPositiveValue: 16386f
                );
            }

            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_ramMonitor = GetComponent<RamMonitor>();
           
            UpdateParameters();
        }

        #endregion
    }
}