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
    public static class G_FloatString
    {
        /* ----- TODO: ----------------------------
         * Try and move the Init to a core method.
         * Try and replace the Pow function with a better algorithm.
         * --------------------------------------*/

        #region Variables -> Private

        /// <summary>
        /// Float represented as a string, formatted.
        /// </summary>
        private const   string      floatFormat         = "0.0";

        /// <summary>
        /// The currently defined, globally used decimal multiplier.
        /// </summary>
        private static  float       decimalMultiplier   = 1f;

        /// <summary>
        /// List of negative floats casted to strings.
        /// </summary>
        private static  string[]    negativeBuffer      = new string[0];

        /// <summary>
        /// List of positive floats casted to strings.
        /// </summary>
        private static  string[]    positiveBuffer      = new string[0];

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
        /// The lowest float value of the existing number buffer.
        /// </summary>
        public static float MinValue
        {
            get
            {
                return -(negativeBuffer.Length - 1).FromIndex();
            }
        }

        /// <summary>
        /// The highest float value of the existing number buffer.
        /// </summary>
        public static float MaxValue
        {
            get
            {
                return (positiveBuffer.Length - 1).FromIndex();
            }
        }

        #endregion

        #region Methods -> Public

        //TODO: Figure out what the negative buffer doe, why we dont have default values and why the range is so high.
        /// <summary>
        /// Initialize the buffers.
        /// </summary>
        /// <param name="minNegativeValue">
        /// Lowest negative value allowed.
        /// </param>
        /// <param name="maxPositiveValue">
        /// Highest positive value allowed.
        /// </param>
        /// <param name="decimals">
        /// How many decimals will the values use?
        /// </param>
        public static void Init(float minNegativeValue, float maxPositiveValue, int decimals = 1)
        {
            decimalMultiplier = Pow(10, Mathf.Clamp(decimals, 1, 5));

            int negativeLength = minNegativeValue.ToIndex();
            int positiveLength = maxPositiveValue.ToIndex();

            if (negativeLength >= 0)
            {
                negativeBuffer = new string[negativeLength];
                for (int i = 0; i < negativeLength; i++)
                {
                    negativeBuffer[i] = (-i).FromIndex().ToString(floatFormat);
                }
            }

            if (positiveLength >= 0)
            {
                positiveBuffer = new string[positiveLength];
                for (int i = 0; i < positiveLength; i++)
                {
                    positiveBuffer[i] = i.FromIndex().ToString(floatFormat);
                }
            }
        }

        /// <summary>
        /// Returns this float as a cached string.
        /// </summary>
        /// <param name="value">
        /// The required float.
        /// </param>
        /// <returns>
        /// A cached number string.
        /// </returns>
        public static string ToStringNonAlloc(this float value)
        {
            int valIndex = value.ToIndex();

            if (value < 0 && valIndex < negativeBuffer.Length)
            {
                return negativeBuffer[valIndex];
            }

            if (value >= 0 && valIndex < positiveBuffer.Length)
            {
                return positiveBuffer[valIndex];
            }

            return value.ToString();
        }

        //TODO: Convert this to use floatFormat instead, but investigate which functions require and dont require one first.
        /// <summary>
        /// Returns this float as a cached string.
        /// </summary>
        /// <param name="value">
        /// The required float.
        /// </param>
        /// <returns>
        /// A cached number string.
        /// </returns>
        public static string ToStringNonAlloc(this float value, string format)
        {
            int valIndex = value.ToIndex();

            if (value < 0 && valIndex < negativeBuffer.Length)
            {
                return negativeBuffer[valIndex];
            }

            if (value >= 0 && valIndex < positiveBuffer.Length)
            {
                return positiveBuffer[valIndex];
            }

            return value.ToString(format);
        }

        /// <summary>
        /// Returns a float as a casted int.
        /// </summary>
        /// <param name="f">
        /// The given float.
        /// </param>
        /// <returns>
        /// The given float as an int.
        /// </returns>
        public static int ToInt(this float f)
        {
            return (int)f;
        }

        /// <summary>
        /// Returns an int as a casted float.
        /// </summary>
        /// <param name="f">
        /// The given int.
        /// </param>
        /// <returns>
        /// The given int as a float.
        /// </returns>
        public static float ToFloat(this int i)
        {
            return (float)i;
        }

        #endregion

        #region Methods -> Private

        //TODO: Replace this with a better algorithm.
        private static int Pow(int f, int p)
        {
            for (int i = 1; i < p; i++)
            {
                f *= f;
            }
            return f;
        }

        private static int ToIndex(this float f)
        {
            return Mathf.Abs((f * decimalMultiplier).ToInt());
        }

        private static float FromIndex(this int i)
        {
            return (i.ToFloat() / decimalMultiplier);
        }

        #endregion
    }
}
