/* ---------------------------------------
 * Author:          Started by David Mkrtchyan, modified by Martin Pane (martintayx@gmail.com) (@martinTayx)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            18-May-18
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;

namespace Tayx.Graphy.Utils.NumString
{
    public static class G_FloatString
    {
        #region Variables -> Private

        /// <summary>
        /// Float represented as a string, formatted.
        /// </summary>
        private const string m_floatFormat = "0.0";

        /// <summary>
        /// The currently defined, globally used decimal multiplier.
        /// </summary>
        private static float m_decimalMultiplier = 10f;

        /// <summary>
        /// List of negative floats casted to strings.
        /// </summary>
        private static string[] m_negativeBuffer = new string[0];

        /// <summary>
        /// List of positive floats casted to strings.
        /// </summary>
        private static string[] m_positiveBuffer = new string[0];

        #endregion

        #region Properties -> Public

        /// <summary>
        /// The lowest float value of the existing number buffer.
        /// </summary>
        public static float MinValue => -(m_negativeBuffer.Length - 1).FromIndex();

        /// <summary>
        /// The highest float value of the existing number buffer.
        /// </summary>
        public static float MaxValue => (m_positiveBuffer.Length - 1).FromIndex();

        #endregion

        #region Methods -> Public

        /// <summary>
        /// Initialize the buffers.
        /// </summary>
        /// <param name="minNegativeValue">
        /// Lowest negative value allowed.
        /// </param>
        /// <param name="maxPositiveValue">
        /// Highest positive value allowed.
        /// </param>
        public static void Init( float minNegativeValue, float maxPositiveValue )
        {
            int negativeLength = minNegativeValue.ToIndex();
            int positiveLength = maxPositiveValue.ToIndex();

            if( MinValue > minNegativeValue && negativeLength >= 0 )
            {
                m_negativeBuffer = new string[negativeLength];
                for( int i = 0; i < negativeLength; i++ )
                {
                    m_negativeBuffer[ i ] = (-i - 1).FromIndex().ToString( m_floatFormat );
                }
            }

            if( MaxValue < maxPositiveValue && positiveLength >= 0 )
            {
                m_positiveBuffer = new string[positiveLength + 1];
                for( int i = 0; i < positiveLength + 1; i++ )
                {
                    m_positiveBuffer[ i ] = i.FromIndex().ToString( m_floatFormat );
                }
            }
        }

        public static void Dispose()
        {
            m_negativeBuffer = new string[0];
            m_positiveBuffer = new string[0];
        }

        /// <summary>
        /// Returns this float as a cached string.
        /// </summary>
        /// <param name="value">The required float.</param>
        /// <returns>A cached number string.</returns>
        public static string ToStringNonAlloc( this float value )
        {
            int valIndex = value.ToIndex();

            if( value < 0 && valIndex < m_negativeBuffer.Length )
            {
                return m_negativeBuffer[ valIndex ];
            }

            if( value >= 0 && valIndex < m_positiveBuffer.Length )
            {
                return m_positiveBuffer[ valIndex ];
            }

            return value.ToString();
        }

        /// <summary>
        /// Returns this float as a cached string.
        /// </summary>
        /// <param name="value">The required float.</param>
        /// <returns>A cached number string.</returns>
        public static string ToStringNonAlloc( this float value, string format )
        {
            int valIndex = value.ToIndex();

            if( value < 0 && valIndex < m_negativeBuffer.Length )
            {
                return m_negativeBuffer[ valIndex ];
            }

            if( value >= 0 && valIndex < m_positiveBuffer.Length )
            {
                return m_positiveBuffer[ valIndex ];
            }

            return value.ToString( format );
        }

        /// <summary>
        /// Returns a float as a casted int.
        /// </summary>
        /// <param name="f">The given float.</param>
        /// <returns>The given float as an int.</returns>
        public static int ToInt( this float f )
        {
            return (int) f;
        }

        /// <summary>
        /// Returns an int as a casted float.
        /// </summary>
        /// <param name="f">The given int.</param>
        /// <returns>The given int as a float.</returns>
        public static float ToFloat( this int i )
        {
            return (float) i;
        }

        #endregion

        #region Methods -> Private

        private static int ToIndex( this float f )
        {
            return Mathf.Abs( (f * m_decimalMultiplier).ToInt() );
        }

        private static float FromIndex( this int i )
        {
            return (i.ToFloat() / m_decimalMultiplier);
        }

        #endregion
    }
}