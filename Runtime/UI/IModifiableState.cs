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
    public interface IModifiableState
    {
        /// <summary>
        /// Set the module state.
        /// </summary>
        /// <param name="newState">
        /// The new state.
        /// </param>
        void SetState(GraphyManager.ModuleState newState, bool silentUpdate);
    }
}
