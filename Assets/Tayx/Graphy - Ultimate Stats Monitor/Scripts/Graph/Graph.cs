/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 23-Jan-18
 * Studio: Tayx
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Tayx.Graphy.Graph
{
    public abstract class Graph : MonoBehaviour
    {
        /// <summary>
        /// Updates the graph/s.
        /// </summary>
        protected abstract void UpdateGraph();

        /// <summary>
        /// Creates the points for the graph/s.
        /// </summary>
        protected abstract void CreatePoints();
    }

}
