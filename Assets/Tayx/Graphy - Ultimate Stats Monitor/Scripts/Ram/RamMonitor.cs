﻿/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            15-Dec-17
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using System.Collections;

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace Tayx.Graphy.Ram
{
    public class RamMonitor : MonoBehaviour
    {
        /* ----- TODO: ----------------------------
         * Check if we can seal this class.
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we can remove "using System.Collections;".
         * Check if we should add "private" to the Unity Callbacks.
         * --------------------------------------*/

        #region Variables -> Private

        private float m_allocatedRam;
        private float m_reservedRam;
        private float m_monoRam;

        #endregion

        #region Properties -> Public

        public float AllocatedRam   { get { return m_allocatedRam; } }
        public float ReservedRam    { get { return m_reservedRam; } }
        public float MonoRam        { get { return m_monoRam; } }
        
        #endregion

        #region Methods -> Unity Callbacks

        void Update()
        {
#if UNITY_5_6_OR_NEWER
            m_allocatedRam  = Profiler.GetTotalAllocatedMemoryLong()/ 1048576f;
            m_reservedRam   = Profiler.GetTotalReservedMemoryLong() / 1048576f;
            m_monoRam       = Profiler.GetMonoUsedSizeLong()        / 1048576f;
#else
            m_allocatedRam  = Profiler.GetTotalAllocatedMemory()    / 1048576f;
            m_reservedRam   = Profiler.GetTotalReservedMemory()     / 1048576f;
            m_monoRam       = Profiler.GetMonoUsedSize()            / 1048576f;
#endif
        }

        #endregion
        
    }
}