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
using Tayx.Graphy.Utils.NumString;

namespace Tayx.Graphy.Ram
{
    public class G_RamText : MonoBehaviour
    {
        /* ----- TODO: ----------------------------
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we should add a "RequireComponent" for "RamMonitor".
         * Improve the FloatString Init to come from the core instead.
         * --------------------------------------*/

        #region Variables -> Serialized Private

        [SerializeField] private    Text            m_allocatedSystemMemorySizeText         = null;
        [SerializeField] private    Text            m_reservedSystemMemorySizeText          = null;
        [SerializeField] private    Text            m_monoSystemMemorySizeText              = null;

        #endregion

        #region Variables -> Private

        private                     GraphyManager   m_graphyManager                         = null;

        private                     G_RamMonitor    m_ramMonitor                            = null;

        private                     float           m_updateRate                            = 4f;  // 4 updates per sec.

        private                     float           m_deltaTime                             = 0.0f;

        private readonly            string          m_memoryStringFormat                    = "0.0";

        #endregion

        #region Methods -> Unity Callbacks

        private void Awake()
        {
            Init();
        }

        private void Update()
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
            if (!G_FloatString.Inited || G_FloatString.MinValue > -1000f || G_FloatString.MaxValue < 16384f)
            {
                G_FloatString.Init
                (
                    minNegativeValue: -1001f,
                    maxPositiveValue: 16386f
                );
            }

            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

            m_ramMonitor = GetComponent<G_RamMonitor>();
           
            UpdateParameters();
        }

        #endregion
    }
}