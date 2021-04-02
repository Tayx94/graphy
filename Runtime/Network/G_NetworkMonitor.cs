/* --------------------------------------------------------------
 * Author:          Davina Armstrong (davina@playableworlds.com)
 * --------------------------------------------------------------*/

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
            Bootstrap.ClientWorld.GetExistingSystem<ApathyTransportClientSystem>();
            m_bytesReceived = network_recv()
        }

        #endregion
    }
}