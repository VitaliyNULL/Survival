using System;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerController : NetworkBehaviour, IPlayerLeft
    {
        #region Private Fields

        private NetworkCharacterControllerPrototype _controller;
        private Animator _animator;
        [SerializeField] private RuntimeAnimatorController farmer0;
        [SerializeField] private RuntimeAnimatorController farmer1;
        [SerializeField] private RuntimeAnimatorController farmer2;
        [SerializeField] private RuntimeAnimatorController farmer3;
        private readonly string _mySkin = "MY_SKIN";

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _controller = GetComponentInParent<NetworkCharacterControllerPrototype>();
        }

        #endregion
        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                RPC_ChangeSkin((PlayerSkin)PlayerPrefs.GetInt(_mySkin));
                Debug.Log("spawned local player");
            }

            RPC_ChangeSkinRemotePlayer();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                data.direction.Normalize();
                _controller.Move(5*data.direction*Runner.DeltaTime);
                // Debug.Log("Moving");
            }
        }

        #endregion

        #region Private methods

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            
        }
        private void SetAnimator(PlayerSkin playerSkin)
        {
            _animator ??= GetComponent<Animator>();
            switch (playerSkin)
            {
                case PlayerSkin.Farmer0:
                    _animator.runtimeAnimatorController = farmer0;
                    break;
                case PlayerSkin.Farmer1:
                    _animator.runtimeAnimatorController = farmer1;
                    break;
                case PlayerSkin.Farmer2:
                    _animator.runtimeAnimatorController = farmer2;
                    break;
                case PlayerSkin.Farmer3:
                    _animator.runtimeAnimatorController = farmer3;
                    break;
            }
        }

        #endregion

        #region RPC

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ChangeSkin(PlayerSkin skin, RpcInfo info = default)
        {
            Debug.Log($"[RPC_ChangeSkin] {info.Source.PlayerId} called RPC");
            SetAnimator(skin);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_ChangeSkinRemotePlayer(RpcInfo info = default)
        {
            if (HasInputAuthority && HasStateAuthority)
            {
                Debug.Log($"[RPC_ChangeSkinRemotePlayer] {info.Source.PlayerId} called RPC");
                SetAnimator((PlayerSkin)PlayerPrefs.GetInt(_mySkin));
                RPC_TakeRpc((PlayerSkin)PlayerPrefs.GetInt(_mySkin));
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_TakeRpc(PlayerSkin skin, RpcInfo info = default)
        {
            if (!HasInputAuthority && !HasStateAuthority)
            {
                SetAnimator(skin);
            }
        }

        #endregion

        #region IPlayerLeft

        public void PlayerLeft(PlayerRef player)
        {
            Runner.Despawn(Object);
            Debug.Log("Despawn Object");
        }

        #endregion
    }
}