/* ---------------------------------------
 * Author: Started by David Mkrtchyan, modified by Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 18-May-18
 * Studio: Tayx
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tayx.Graphy.Utils
{
    //TODO: Figure out why these arent in their own class file under the same namespace instead.
    public static class IntString
    {
        #region Private Variables

        /// <summary>
        /// List of negative ints casted to strings.
        /// </summary>
        private static string[] negativeBuffer = new string[0];

        /// <summary>
        /// List of positive ints casted to strings.
        /// </summary>
        private static string[] positiveBuffer = new string[0];

        #endregion

        #region Properties

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

        #region Public Methods

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

    public static class FloatString
    {
        #region Private Variables

        /// <summary>
        /// Float represented as a string, formatted.
        /// </summary>
        private const string floatFormat = "0.0";

        /// <summary>
        /// The currently defined, globally used decimal multiplier.
        /// </summary>
        private static float decimalMultiplier = 1f;

        /// <summary>
        /// List of negative floats casted to strings.
        /// </summary>
        private static string[] negativeBuffer = new string[0];

        /// <summary>
        /// List of positive floats casted to strings.
        /// </summary>
        private static string[] positiveBuffer = new string[0];

        #endregion

        #region Properties

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

        #region Public Methods

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

        #region Private Methods

        //TODO: Figure out why this doesnt just use Mathf.Pow() and add the remaining descriptions.
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
