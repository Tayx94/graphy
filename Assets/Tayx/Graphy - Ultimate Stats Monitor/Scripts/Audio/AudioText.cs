﻿/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 15-Dec-17
 * Studio: Tayx
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using Tayx.Graphy.Utils;
using Tayx.Graphy.Utils.NumString;

namespace Tayx.Graphy.Audio
{
    public class AudioText : MonoBehaviour
    {

        #region Private Variables

        private GraphyManager m_graphyManager;

        private AudioMonitor m_audioMonitor;

        [SerializeField] private Text m_DBText;

        private int m_updateRate = 4;

        private float m_deltaTimeOffset = 0;

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
                if (m_deltaTimeOffset > 1f / m_updateRate)
                {
                    m_deltaTimeOffset = 0;

                    m_DBText.text = Mathf.Clamp(m_audioMonitor.MaxDB, -80, 0).ToStringNonAlloc();
                }
                else
                {
                    m_deltaTimeOffset += Time.deltaTime;
                }
            }
        }

        #endregion

        #region Public Methods

        public void UpdateParameters()
        {
            m_updateRate = m_graphyManager.AudioTextUpdateRate;
        }

        #endregion

        #region Private Methods

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

            m_audioMonitor = GetComponent<AudioMonitor>();
                       
            UpdateParameters();
        }

        #endregion

    }
}