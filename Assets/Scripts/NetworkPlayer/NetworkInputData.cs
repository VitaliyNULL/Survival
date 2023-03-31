using Fusion;
using UnityEngine;

namespace VitaliyNULL.NetworkPlayer
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector3 directionToMove;
        public Vector3 directionToShoot;
        public bool isShoot;
    }
}