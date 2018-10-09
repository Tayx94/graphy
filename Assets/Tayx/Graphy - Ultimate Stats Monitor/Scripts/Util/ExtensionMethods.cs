﻿/* ---------------------------------------
 * Author:      Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:     Graphy - Ultimate Stats Monitor
 * Date:        04-Jan-18
 * Studio:      Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Tayx.Graphy.Utils
{
    public static class ExtensionMethods
    {
        /* ----- TODO: ----------------------------
         * Add summaries to the functions.
         * Check if we can remove "using System.Collections;".
         * Figure out why we're returning something on a "this" with reference elements.
         * --------------------------------------*/

        #region Methods -> Public

        /// <summary>
        /// Functions as the SetActive function in the GameObject class, but for a list of them.
        /// </summary>
        /// <param name="gameObjects">
        /// List of GameObjects.
        /// </param>
        /// <param name="active">
        /// Wether to turn them on or off.
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public static List<GameObject> SetAllActive(this List<GameObject> gameObjects, bool active)
        {
            foreach (var gameObj in gameObjects)
            {
                gameObj.SetActive(active);
            }

            return gameObjects;
        }

        public static List<Image> SetOneActive(this List<Image> images, int active)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].gameObject.SetActive(i == active);
            }

            return images;
        }
        
        public static List<Image> SetAllActive(this List<Image> images, bool active)
        {
            foreach (var image in images)
            {
                image.gameObject.SetActive(active);
            }

            return images;
        }

        #endregion
    }
}
