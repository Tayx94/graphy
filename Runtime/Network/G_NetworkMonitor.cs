/* ---------------------------------------
 * Author:          Davina Armstrong (davina@playableworlds.com)
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.Profiling;

namespace Tayx.Graphy.Network
{
    public class G_NetworkMonitor: MonoBehaviour
    {
        #region Variables -> Private

        private int m_bytesReceived = 0;
        private const string m_dllName = "network.bindings.dll";

        #endregion

        #region Properties -> Public

        public float BytesReceived => m_bytesReceived;

        #endregion

        #region Methods -> Unity Callbacks

        private void Update()
        {
            m_bytesReceived = network_recv()
        }

        #endregion

        #region Methods -> Private

        // pattern copied from StarsReach NativeBindings.cs
        [DllImport(m_dllName, CallingConvention = CallingConvention.Cdecl]
        private static int network_recv(long sock, void* buffer, int len, ref int error);
    }
}