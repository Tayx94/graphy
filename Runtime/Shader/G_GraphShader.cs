/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            22-Nov-17
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy
{
    /// <summary>
    /// This class communicates directly with the shader to draw the graphs. Performance here is very important
    /// to reduce as much overhead as possible, as we are updating hundreds of values every frame.
    /// </summary>
    public class G_GraphShader
    {
        #region Variables

        public const int ArrayMaxSizeFull = 512;
        public const int ArrayMaxSizeLight = 128;

        public int ArrayMaxSize = 128;

        public float[] ShaderArrayValues;


        public Image Image = null;


        private string Name = "GraphValues"; // The name of the array
        private string Name_Length = "GraphValues_Length";


        public float Average = 0;
        private int m_averagePropertyId = 0;


        public float GoodThreshold = 0;
        public float CautionThreshold = 0;

        private int m_goodThresholdPropertyId = 0;
        private int m_cautionThresholdPropertyId = 0;


        public Color GoodColor = Color.white;
        public Color CautionColor = Color.white;
        public Color CriticalColor = Color.white;

        private int m_goodColorPropertyId = 0;
        private int m_cautionColorPropertyId = 0;
        private int m_criticalColorPropertyId = 0;

        #endregion

        #region Methods -> Public

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
            Image.material.SetFloatArray( Name, new float[ ArrayMaxSize ] );

            m_averagePropertyId = Shader.PropertyToID( "Average" );

            m_goodThresholdPropertyId = Shader.PropertyToID( "_GoodThreshold" );
            m_cautionThresholdPropertyId = Shader.PropertyToID( "_CautionThreshold" );

            m_goodColorPropertyId = Shader.PropertyToID( "_GoodColor" );
            m_cautionColorPropertyId = Shader.PropertyToID( "_CautionColor" );
            m_criticalColorPropertyId = Shader.PropertyToID( "_CriticalColor" );
        }

        /// <summary>
        /// Updates the material linked with this shader graph  with the values in the float[] array.
        /// </summary>
        public void UpdateArray()
        {
            Image.material.SetInt( Name_Length, ShaderArrayValues.Length );
        }

        /// <summary>
        /// Updates the average parameter in the material.
        /// </summary>
        public void UpdateAverage()
        {
            Image.material.SetFloat( m_averagePropertyId, Average );
        }

        /// <summary>
        /// Updates the thresholds in the material.
        /// </summary>
        public void UpdateThresholds()
        {
            Image.material.SetFloat( m_goodThresholdPropertyId, GoodThreshold );
            Image.material.SetFloat( m_cautionThresholdPropertyId, CautionThreshold );
        }

        /// <summary>
        /// Updates the colors in the material.
        /// </summary>
        public void UpdateColors()
        {
            Image.material.SetColor( m_goodColorPropertyId, GoodColor );
            Image.material.SetColor( m_cautionColorPropertyId, CautionColor );
            Image.material.SetColor( m_criticalColorPropertyId, CriticalColor );
        }

        /// <summary>
        /// Updates the points in the graph with the set array of values.
        /// </summary>
        public void UpdatePoints()
        {
            // Requires an array called "name"
            // and another one called "name_Length"

            Image.material.SetFloatArray( Name, ShaderArrayValues );
        }

        #endregion
    }
}

