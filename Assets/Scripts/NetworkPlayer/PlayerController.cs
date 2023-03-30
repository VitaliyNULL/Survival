using Fusion;
using UnityEngine;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerController : NetworkBehaviour, IPlayerLeft
    {

        #region IPlayerLeft

        public void PlayerLeft(PlayerRef player)
        {
            Runner.Despawn(Object);
            Debug.Log("Despawn Object");
        }

        #endregion
    }
}