/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 22-Nov-17
 * Studio: Tayx
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

namespace Tayx.Graphy
{
    public class ShaderGraph
    {
        public const int ArrayMaxSizeFull  = 512;
        public const int ArrayMaxSizeLight = 128;

        public int ArrayMaxSize = 128;

        public Image Image;
     
        private string Name = "GraphValues";                  // The name of the array
        private string Name_Length = "GraphValues_Length";    

        public float[] Array;                                 // The values

        // Average
        
        public float Average;
        private int averagePropertyId;

        // Thresholds
        
        public float GoodThreshold;
        public float CautionThreshold;
        private int goodThresholdPropertyId;
        private int cautionThresholdPropertyId;

        // Color
        
        public Color GoodColor;
        public Color CautionColor;
        public Color CriticalColor;
        private int goodColorPropertyId;
        private int cautionColorPropertyId;
        private int criticalColorPropertyId;
        

        /// <summary>
        /// This is done to avoid a design problem that arrays in shaders have, 
        /// and should be called before initializing any shader graph.
        /// The first time that you use initialize an array, the size of the array in the shader is fixed.
        /// This is why sometimes you will get a warning saying that the array size will be capped.
        /// It shouldn't generate any issues, but in the worst case scenario just reset the Unity Editor
        /// (if for some reason the shaders reload).
        /// I also cache the Property IDs, that make access faster to modify shader parameters.
        /// </summary>
        public void InitializeShader()
        {
            Image.material.SetFloatArray(Name, new float[ArrayMaxSize]);

            averagePropertyId           = Shader.PropertyToID("Average");

            goodThresholdPropertyId     = Shader.PropertyToID("_GoodThreshold");
            cautionThresholdPropertyId  = Shader.PropertyToID("_CautionThreshold");

            goodColorPropertyId         = Shader.PropertyToID("_GoodColor");
            cautionColorPropertyId      = Shader.PropertyToID("_CautionColor");
            criticalColorPropertyId     = Shader.PropertyToID("_CriticalColor");
        }

        /// <summary>
        /// Updates the material linked with this shader graph  with the values in the float[] array.
        /// </summary>
        public void UpdateArray()
        {
            Image.material.SetInt(Name_Length, Array.Length);
        }

        /// <summary>
        /// Updates the average parameter in the material.
        /// </summary>
        public void UpdateAverage()
        {
            Image.material.SetFloat(averagePropertyId, Average);
        }
        
        /// <summary>
        /// Updates the thresholds in the material.
        /// </summary>
        public void UpdateThresholds()
        {
            Image.material.SetFloat(goodThresholdPropertyId, GoodThreshold);
            Image.material.SetFloat(cautionThresholdPropertyId, CautionThreshold);
        }
        
        /// <summary>
        /// Updates the colors in the material.
        /// </summary>
        public void UpdateColors()
        {
            Image.material.SetColor(goodColorPropertyId, GoodColor);
            Image.material.SetColor(cautionColorPropertyId, CautionColor);
            Image.material.SetColor(criticalColorPropertyId, CriticalColor);
        }

        /// <summary>
        /// Updates the points in the graph with the set array of values.
        /// </summary>
        public void UpdatePoints()
        {
            // Requires an array called "name"
            // and another one called "name_Length"

            Image.material.SetFloatArray(Name, Array);
         }
    }
}

