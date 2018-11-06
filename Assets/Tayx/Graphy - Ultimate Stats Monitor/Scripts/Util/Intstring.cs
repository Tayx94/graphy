/* ---------------------------------------
 * Author:          Started by David Mkrtchyan, modified by Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            18-May-18
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;

namespace Tayx.Graphy.Utils.NumString
{
    public static class G_IntString
    {
        /* ----- TODO: ----------------------------
         * Try and move the Init to a core method.
         * --------------------------------------*/

        #region Variables -> Private

        /// <summary>
        /// List of negative ints casted to strings.
        /// </summary>
        private static string[] negativeBuffer = new string[0];

        /// <summary>
        /// List of positive ints casted to strings.
        /// </summary>
        private static string[] positiveBuffer = new string[0];

        #endregion

        #region Properties -> Public

        /// <summary>
        /// Have the int buffers been initialized?
        /// </summary>
        public static bool Inited
        {
            get
            {
                return negativeBuffer.Length > 0 || positiveBuffer.Length > 0;
            }
        }

        /// <summary>
        /// The lowest int value of the existing number buffer.
        /// </summary>
        public static int MinValue
        {
            get
            {
                return -(negativeBuffer.Length - 1);
            }
        }

        /// <summary>
        /// The highest int value of the existing number buffer.
        /// </summary>
        public static int MaxValue
        {
            get
            {
                return positiveBuffer.Length - 1;
            }
        }

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
        public static void Init(int minNegativeValue, int maxPositiveValue)
        {
            if (minNegativeValue <= 0)
            {
                int length = Mathf.Abs(minNegativeValue);
                negativeBuffer = new string[length];
                for (int i = 0; i < length; i++)
                {
                    negativeBuffer[i] = (-i).ToString();
                }
            }
            if (maxPositiveValue >= 0)
            {
                positiveBuffer = new string[maxPositiveValue];
                for (int i = 0; i < maxPositiveValue; i++)
                {
                    positiveBuffer[i] = i.ToString();
                }
            }
        }
        
        /// <summary>
        /// Returns this int as a cached string.
        /// </summary>
        /// <param name="value">
        /// The required int.
        /// </param>
        /// <returns>
        /// A cached number string.
        /// </returns>
        public static string ToStringNonAlloc(this int value)
        {
            if (value < 0 && -value < negativeBuffer.Length)
            {
                return negativeBuffer[-value];
            }

            if (value >= 0 && value < positiveBuffer.Length)
            {
                return positiveBuffer[value];
            }

            return value.ToString();
        }

        #endregion
    }
}
