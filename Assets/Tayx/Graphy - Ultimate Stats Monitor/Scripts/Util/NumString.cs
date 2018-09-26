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
    public static class IntString
    {
        public static string[] positiveBuffer = new string[0];

        public static string[] negativeBuffer = new string[0];

        public static int MaxValue
        {
            get
            {
                return positiveBuffer.Length;
            }
        }

        public static int MinValue
        {
            get
            {
                return negativeBuffer.Length;
            }
        }
        
        public static bool Inited
        {
            get
            {
                return positiveBuffer.Length > 0 || negativeBuffer.Length > 0;
            }
        }
        
        public static void Init(int minNegativeValue, int maxPositiveValue)
        {
            if(maxPositiveValue >= 0)
            {
                positiveBuffer = new string[maxPositiveValue];
                for (int i = 0; i < maxPositiveValue; i++)
                {
                    positiveBuffer[i] = i.ToString();
                }
            }
            if (minNegativeValue <= 0)
            {
                int length = Mathf.Abs(minNegativeValue); 
                negativeBuffer = new string[length];
                for (int i = 0; i < length; i++)
                {
                    negativeBuffer[i] = (-i).ToString();
                }
            }
        }
                
        public static string ToStringNonAlloc(this int value)
        {
            if(value >= 0 && value < positiveBuffer.Length)
            {
                return positiveBuffer[value];
            }

            if(value < 0 && -value < negativeBuffer.Length)
            {
                return negativeBuffer[-value];
            }

            return value.ToString();
        }
        
    }

    public static class FloatString
    {
        private const string format = "0.0";
        private static float decimalMultiplayer = 1;

        public static bool Inited
        {
            get
            {
                return positiveBuffer.Length > 0 || negativeBuffer.Length > 0;
            }
        }

        public static string[] positiveBuffer = new string[0];

        public static string[] negativeBuffer = new string[0];

        public static float MaxValue
        {
            get
            {
                return (positiveBuffer.Length - 1).FromIndex();
            }
        }

        public static float MinValue
        {
            get
            {
                return -(negativeBuffer.Length - 1).FromIndex();
            }
        }

        public static void Init(float minNegativeValue, float maxPositiveValue, int deciminals = 1)
        {
            decimalMultiplayer = Pow(10, Mathf.Clamp(deciminals, 1, 5));

            int negativeLength = minNegativeValue.ToIndex();
            int positiveLength = maxPositiveValue.ToIndex();

            if (positiveLength >= 0)
            {
                positiveBuffer = new string[positiveLength];
                for (int i = 0; i < positiveLength; i++)
                {
                    positiveBuffer[i] = i.FromIndex().ToString(format);
                }
            }

            if (negativeLength >= 0)
            {
                negativeBuffer = new string[negativeLength];
                for (int i = 0; i < negativeLength; i++)
                {
                    negativeBuffer[i] = (-i).FromIndex().ToString(format);
                }
            }
        }
        
        public static string ToStringNonAlloc(this float value)
        {
            int valIndex = value.ToIndex();
            if (value >= 0 && valIndex < positiveBuffer.Length)
            {
                return positiveBuffer[valIndex];
            }

            if (value < 0 && valIndex < negativeBuffer.Length)
            {
                return negativeBuffer[valIndex];
            }

            return value.ToString();
        }

        public static string ToStringNonAlloc(this float value, string format)
        {
            int valIndex = value.ToIndex();
            if (value >= 0 && valIndex < positiveBuffer.Length)
            {
                return positiveBuffer[valIndex];
            }

            if (value < 0 && valIndex < negativeBuffer.Length)
            {
                return negativeBuffer[valIndex];
            }

            return value.ToString(format);
        }

        private static int Pow(int f, int p)
        {
            for (int i = 1; i < p; i++)
                f *= f;

            return f;
        }

        private static int ToIndex(this float f)
        {
            return Mathf.Abs((f * decimalMultiplayer).ToInt());
        }

        private static float FromIndex(this int i)
        {
            return (i.ToFloat() / decimalMultiplayer);
        }

        public static int ToInt(this float f)
        {
            return (int)f;
        }

        public static float ToFloat(this int i)
        {
            return (float)i;
        }

    }
}
