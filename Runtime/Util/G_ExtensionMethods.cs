/* ---------------------------------------
 * Author:      Martin Pane (martintayx@gmail.com) (@martinTayx)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:     Graphy - Ultimate Stats Monitor
 * Date:        04-Jan-18
 * Studio:      Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Tayx.Graphy.Utils
{
    public static class G_ExtensionMethods
    {
        #region Methods -> Extension Methods

        /// <summary>
        /// Functions as the SetActive function in the GameObject class, but for a list of them.
        /// </summary>
        /// <param name="gameObjects">
        /// List of GameObjects.
        /// </param>
        /// <param name="active">
        /// Wether to turn them on or off.
        /// </param>
        public static List<GameObject> SetAllActive( this List<GameObject> gameObjects, bool active )
        {
            foreach( var gameObj in gameObjects )
            {
                gameObj.SetActive( active );
            }

            return gameObjects;
        }

        public static List<Image> SetOneActive( this List<Image> images, int active )
        {
            for( int i = 0; i < images.Count; i++ )
            {
                images[ i ].gameObject.SetActive( i == active );
            }

            return images;
        }

        public static List<Image> SetAllActive( this List<Image> images, bool active )
        {
            foreach( var image in images )
            {
                image.gameObject.SetActive( active );
            }

            return images;
        }

        #endregion
    }
}