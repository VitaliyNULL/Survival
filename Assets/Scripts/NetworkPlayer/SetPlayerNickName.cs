using System;
using Fusion;
using TMPro;
using UnityEngine;

namespace VitaliyNULL.NetworkPlayer
{
    public class SetPlayerNickName : NetworkBehaviour
    {
        #region Private Fields

        private string _username = String.Empty;
        [SerializeField] private TMP_Text text;
        private readonly string _nameKey = "USERNAME";

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                RPC_ChangeNickName(PlayerPrefs.GetString(_nameKey));
                Debug.Log("spawned local player");
            }

            RPC_ChangeNickNameRemotePlayer();
        }

        #endregion

        #region Private Methods

        private void SetUserNickname(string username)
        {
            _username = username;
            text.text = _username;
        }

        #endregion

        #region RPC

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ChangeNickName(string username,RpcInfo info = default)
        {
            Debug.Log($"[RPC_ChangeSkin] {info.Source.PlayerId} called RPC");
            SetUserNickname(username);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ChangeNickNameRemotePlayer(RpcInfo info = default)
        {
            if (HasInputAuthority && HasStateAuthority)
            {
                Debug.Log($"[RPC_ChangeSkinRemotePlayer] {info.Source.PlayerId} called RPC");
                SetUserNickname(PlayerPrefs.GetString(_nameKey));
                RPC_TakeRpc(PlayerPrefs.GetString(_nameKey));
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_TakeRpc(string username,RpcInfo info = default)
        {
            if (!HasInputAuthority && !HasStateAuthority)
            {
                SetUserNickname(username);
            }
        }

        #endregion
    }
}