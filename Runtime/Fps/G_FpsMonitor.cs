/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@martinTayx), modified by Paul Sinnett (paul.sinnett@gmail.com) (@paulsinnett)
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

using Tayx.Graphy.Utils;
using UnityEngine;

namespace Tayx.Graphy.Fps
{
    public class G_FpsMonitor : MonoBehaviour
    {
        #region Variables -> Private

        private G_DoubleEndedQueue m_fpsSamples;
        private short[] m_fpsSamplesSorted;
        private short m_fpsSamplesCapacity = 1024;
        private short m_onePercentSamples = 10;
        private short m_fpsSamplesCount = 0;
        private float m_unscaledDeltaTime = 0f;
        private int m_fpsAverageWindowSum = 0;
        private G_Histogram m_histogram;

        // This cap prevents the histogram from re-allocating memory in the
        // case of an unexpectedly high frame rate. The limit is somewhat
        // arbitrary. The only real cost to a higher cap is memory.
        private const short m_histogramFpsCap = 999;

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

            m_fpsSamplesCount = UpdateStatistics( CurrentFPS );

            averageAddedFps = (uint) m_fpsAverageWindowSum;

            AverageFPS = (short) ((float) averageAddedFps / (float) m_fpsSamplesCount);

            // Update percent lows

            short samplesBelowOnePercent = (short) Mathf.Min( m_fpsSamplesCount - 1, m_onePercentSamples );

            m_histogram.WriteToSortedArray( m_fpsSamplesSorted, samplesBelowOnePercent + 1 );

            // Calculate 0.1% and 1% quantiles, these values represent the fps
            // values below which fall 0.1% and 1% of the samples within the
            // moving window.

            Zero1PercentFps = (short) Mathf.RoundToInt( CalculateQuantile( 0.001f ) );

            OnePercentFPS = (short) Mathf.RoundToInt( CalculateQuantile( 0.01f ) );
        }

        #endregion

        #region Methods -> Public

        public void UpdateParameters()
        {
            m_onePercentSamples = (short) (m_fpsSamplesCapacity / 100);
            if( m_onePercentSamples + 1 > m_fpsSamplesSorted.Length )
            {
                m_fpsSamplesSorted = new short[ m_onePercentSamples + 1 ];
            }
        }

        #endregion

        #region Methods -> Private

        private void Init()
        {
            m_fpsSamples = new G_DoubleEndedQueue( m_fpsSamplesCapacity );
            m_fpsSamplesSorted = new short[ m_onePercentSamples + 1 ];
            m_histogram = new G_Histogram( 0, m_histogramFpsCap );
            UpdateParameters();
        }

        private short UpdateStatistics( short fps )
        {
            if( m_fpsSamples.Full )
            {
                short remove = m_fpsSamples.PopFront();
                m_fpsAverageWindowSum -= remove;
                m_histogram.RemoveSample( remove );
            }
            m_fpsSamples.PushBack( fps );
            m_fpsAverageWindowSum += fps;
            m_histogram.AddSample( fps );
            return m_fpsSamples.Count;
        }

        private float CalculateQuantile( float quantile )
        {
            // If there aren't enough samples to calculate the quantile yet,
            // this function will instead return the lowest value in the
            // histogram.

            short samples = m_fpsSamples.Count;
            float position = ( samples + 1 ) * quantile - 1;
            short indexLow = (short) ( position > 0 ? Mathf.FloorToInt( position ) : 0 );
            short indexHigh = (short) ( indexLow + 1 < samples? indexLow + 1 : indexLow );
            float valueLow = m_fpsSamplesSorted[ indexLow ];
            float valueHigh = m_fpsSamplesSorted[ indexHigh ];
            float lerp = Mathf.Max( position - indexLow, 0 );
            return Mathf.Lerp( valueLow, valueHigh, lerp );
        }

        #endregion
    }
}