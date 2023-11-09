/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@martinTayx)
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

using System;
using UnityEngine;

namespace Tayx.Graphy.Fps
{
    public class G_FpsMonitor : MonoBehaviour
    {
        #region Variables -> Private

        private short[] m_fpsSamples;
        private short[] m_fpsSamplesSorted;
        private short m_fpsSamplesCapacity = 1024;
        private short m_onePercentSamples = 10;
        private short m_zero1PercentSamples = 1;
        private short m_fpsSamplesCount = 0;
        private short m_indexSample = 0;

        private float m_unscaledDeltaTime = 0f;

        #endregion

        #region Properties -> Public

        public short CurrentFPS { get; private set; } = 0;
        public short AverageFPS { get; private set; } = 0;
        public short OnePercentFPS { get; private set; } = 0;
        public short Zero1PercentFps { get; private set; } = 0;

        #endregion

        #region Methods -> Unity Callbacks

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            m_unscaledDeltaTime = Time.unscaledDeltaTime;

            // Update fps and ms

            CurrentFPS = (short) (Mathf.RoundToInt( 1f / m_unscaledDeltaTime ));

            // Update avg fps

            uint averageAddedFps = 0;

            m_indexSample++;

            if( m_indexSample >= m_fpsSamplesCapacity ) m_indexSample = 0;

            m_fpsSamples[ m_indexSample ] = CurrentFPS;

            if( m_fpsSamplesCount < m_fpsSamplesCapacity )
            {
                m_fpsSamplesCount++;
            }

            for( int i = 0; i < m_fpsSamplesCount; i++ )
            {
                averageAddedFps += (uint) m_fpsSamples[ i ];
            }

            AverageFPS = (short) ((float) averageAddedFps / (float) m_fpsSamplesCount);

            // Update percent lows

            m_fpsSamples.CopyTo( m_fpsSamplesSorted, 0 );

            /*
             * TODO: Find a faster way to do this.
             *      We can probably avoid copying the full array every time
             *      and insert the new item already sorted in the list.
             */
            Array.Sort( m_fpsSamplesSorted,
                ( x, y ) => x.CompareTo( y ) ); // The lambda expression avoids garbage generation

            bool zero1PercentCalculated = false;

            uint totalAddedFps = 0;

            short samplesToIterateThroughForOnePercent = m_fpsSamplesCount < m_onePercentSamples
                ? m_fpsSamplesCount
                : m_onePercentSamples;

            short samplesToIterateThroughForZero1Percent = m_fpsSamplesCount < m_zero1PercentSamples
                ? m_fpsSamplesCount
                : m_zero1PercentSamples;

            short sampleToStartIn = (short) (m_fpsSamplesCapacity - m_fpsSamplesCount);

            for( short i = sampleToStartIn; i < sampleToStartIn + samplesToIterateThroughForOnePercent; i++ )
            {
                totalAddedFps += (ushort) m_fpsSamplesSorted[ i ];

                if( !zero1PercentCalculated && i >= samplesToIterateThroughForZero1Percent - 1 )
                {
                    zero1PercentCalculated = true;

                    Zero1PercentFps = (short) ((float) totalAddedFps / (float) m_zero1PercentSamples);
                }
            }

            OnePercentFPS = (short) ((float) totalAddedFps / (float) m_onePercentSamples);
        }

        #endregion

        #region Methods -> Public

        public void UpdateParameters()
        {
            m_onePercentSamples = (short) (m_fpsSamplesCapacity / 100);
            m_zero1PercentSamples = (short) (m_fpsSamplesCapacity / 1000);
        }

        #endregion

        #region Methods -> Private

        private void Init()
        {
            m_fpsSamples = new short[m_fpsSamplesCapacity];
            m_fpsSamplesSorted = new short[m_fpsSamplesCapacity];

            UpdateParameters();
        }

        #endregion
    }
}