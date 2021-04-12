/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            03-Jan-18
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

namespace Tayx.Graphy.UI
{
    public interface IMovable
    {
        /// <summary>
        /// Sets the position of the module.
        /// </summary>
        /// <param name="newModulePosition">
        /// The new position of the module.
        /// </param>
        void SetPosition(GraphyManager.ModulePosition newModulePosition);
    }
}
