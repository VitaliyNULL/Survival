using Fusion;
using UnityEngine;

namespace VitaliyNULL.NetworkPlayer
{
    public struct NetworkInputData : INetworkInput
    {
        #region Public Fields

        public Vector3 directionToMove;
        public Vector3 directionToShoot;
        public bool isShoot;

        #endregion
    }
}