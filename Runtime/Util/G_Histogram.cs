/* ---------------------------------------
 * Author:          Paul Sinnett (paul.sinnett@gmail.com) (@paulsinnett)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            06-Sep-24
 * Studio:          Powered Up Games
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Tayx.Graphy.Utils
{
    public class G_Histogram
    {
        #region Variables -> Private

        /// <summary>
        /// Fixed size array for holding the histogram values.
        /// </summary>
        private SortedList<short, short> m_histogram;

        /// <summary>
        /// Total number of data points in the histogram.
        /// </summary>
        private short m_count;

        /// <summary>
        /// Minimum limit for histogram values. Values below this limit
        /// will be clamped to this value.
        private short m_minimum;

        /// <summary>
        /// Maximum limit for histogram values. Values above this limit
        /// will be clamped to this value.
        private short m_maximum;

        /// <summary>
        /// Programming error messages for assert failures.
        /// </summary>
        private const string m_errorMinimumLessThanMaximum = "minimum must be less than maximum";
        private const string m_errorSampleNotFound = "sample not found";
        private const string m_errorOutputArrayTooSmall = "output array too small";
        private const string m_errorNotEnoughData = "not enough data in histogram";

        #endregion

        #region Methods -> Public

        /// <summary>
        /// Construct a histogram.
        /// </summary>
        /// <param name="minimum">
        /// Values added below this value will be clamped.
        /// </param>
        /// <param name="maximum">
        /// Values added above this value will be clamped.
        /// </param>
        public G_Histogram( short minimum, short maximum )
        {
            Assert.IsTrue( minimum < maximum, m_errorMinimumLessThanMaximum );
            m_histogram = new SortedList<short, short>( maximum - minimum );
            m_minimum = minimum;
            m_maximum = maximum;
            m_count = 0;
        }

        /// <summary>
        /// Add a sample to the histogram, O(log₂ n) where n is the
        /// number of distinct sample entries.
        /// </summary>
        /// <param name="sample">
        /// The sample to add to this histogram.
        /// </param>
        public void AddSample( short sample )
        {
            sample = (short) Mathf.Clamp( sample, m_minimum, m_maximum );
            if( m_histogram.ContainsKey( sample ) )
            {
                m_histogram[ sample ]++;
            }
            else
            {
                m_histogram.Add( sample, 1 );
            }
            m_count++;
        }

        /// <summary>
        /// Remove a sample from the histogram, O(log₂ n) where n is the
        /// number of distinct sample values.
        /// </summary>
        /// <param name="sample">
        /// The sample to remove.
        /// </param>
        public void RemoveSample( short sample )
        {
            sample = (short) Mathf.Clamp( sample, m_minimum, m_maximum );
            Assert.IsTrue( m_histogram.ContainsKey( sample ), m_errorSampleNotFound );
            m_histogram[ sample ]--;
            if( m_histogram[ sample ] == 0 )
            {
                m_histogram.Remove( sample );
            }
            m_count--;
        }

        /// <summary>
        /// Write out the required number of samples in order from lowest
        /// to the highest, O(n) where n is the count of samples to write
        /// out.
        /// </summary>
        /// <param name="output">
        /// An array to write into.
        /// </param>
        /// <param name="count">
        /// The number of samples to write out.
        /// </param>
        public void WriteToSortedArray( short[] output, int count )
        {
            Assert.IsTrue( count <= output.Length, m_errorOutputArrayTooSmall );
            Assert.IsTrue( count <= m_count, m_errorNotEnoughData );

            int index = 0;
            var keys = m_histogram.Keys;
            var values = m_histogram.Values;
            int entries = keys.Count;
            for( short entry = 0; entry < entries && index < count; entry++ )
            {
                short sample = keys[ entry ];
                int instances = values[ entry ];
                for( int i = 0; i < instances && index < count; i++ )
                {
                    output[ index ] = sample;
                    index++;
                }
            }
        }

        /// <summary>
        /// Clear the histogram, O(n) where n is the number of entries
        /// in the histogram.
        /// </summary>
        public void Clear()
        {
            m_histogram.Clear();
            m_count = 0;
        }

        #endregion
    }
}