using Cinemachine;
using Fusion;
using UnityEngine;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerController : NetworkBehaviour, IPlayerLeft
    {
        private CinemachineVirtualCamera _camera;
        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                _camera = FindObjectOfType<CinemachineVirtualCamera>();
                _camera.Follow = transform;
            }
        }

        #region IPlayerLeft

        public void PlayerLeft(PlayerRef player)
        {
            if(HasInputAuthority) return;
            Runner.Despawn(Object);
            Debug.Log("Despawn Object");
        }

        #endregion
    }
}