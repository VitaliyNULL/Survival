using Fusion;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;
        private PlayerSkin _playerSkin;
        public void SetPlayerSkin(PlayerSkin playerSkin)
        {
            _playerSkin = playerSkin;
        }
    }
}