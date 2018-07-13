/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 15-Dec-17
 * Studio: Tayx
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
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
        #region Private Variables

        private float m_allocatedRam;
        private float m_reservedRam;
        private float m_monoRam;

        #endregion

        #region Properties

        public float AllocatedRam   { get { return m_allocatedRam; } }
        public float ReservedRam    { get { return m_reservedRam; } }
        public float MonoRam        { get { return m_monoRam; } }
        
        #endregion

        #region Unity Methods

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