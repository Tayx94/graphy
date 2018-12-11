/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            23-Jan-18
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;

namespace Tayx.Graphy.Graph
{
    public abstract class G_Graph : MonoBehaviour
    {
        /* ----- TODO: ----------------------------
         * 
         * --------------------------------------*/

        #region Methods -> Protected

        /// <summary>
        /// Updates the graph/s.
        /// </summary>
        protected abstract void UpdateGraph();

        /// <summary>
        /// Creates the points for the graph/s.
        /// </summary>
        protected abstract void CreatePoints();

        #endregion
    }

}
