/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 03-Jan-18
 * Studio: Tayx
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using System.Collections;

namespace Tayx.Graphy.UI
{
    public interface IMovable
    {
        /// <summary>
        /// Sets the position of the module.
        /// </summary>
        /// <param name="newModulePosition"></param>
        void SetPosition(GraphyManager.ModulePosition newModulePosition);
    }

}

