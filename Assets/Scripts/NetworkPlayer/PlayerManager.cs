using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;
        private PlayerSkin _playerSkin;
        [SerializeField] private List<Vector3> posForSpawnPlayers;
        

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                Debug.Log("Spawned Player Manager");
            }
            else
            {
                Debug.Log("Spawned remote Player Manager");
            }
        }

        public void SetPlayerSkin(PlayerSkin playerSkin)
        {
            _playerSkin = playerSkin;
            Debug.Log($"Network objects is {Object}");
            if (Object.Runner.IsClient)
            {
                Instantiate(playerController, posForSpawnPlayers[1], Quaternion.identity);
            }
            else
            {
                Instantiate(playerController, posForSpawnPlayers[0], Quaternion.identity);
            }
        }
    }
}