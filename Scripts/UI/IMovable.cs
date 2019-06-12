/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            03-Jan-18
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

namespace Tayx.Graphy.UI
{
    public interface IMovable
    {
        /* ----- TODO: ----------------------------
         * 
         * --------------------------------------*/

        /// <summary>
        /// Sets the position of the module.
        /// </summary>
        /// <param name="newModulePosition">
        /// The new position of the module.
        /// </param>
        void SetPosition(GraphyManager.ModulePosition newModulePosition);
    }

}

