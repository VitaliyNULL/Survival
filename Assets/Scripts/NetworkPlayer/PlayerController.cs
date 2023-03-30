using Fusion;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerController : NetworkBehaviour, IPlayerLeft
    {
        private Animator _animator;
        [SerializeField] private RuntimeAnimatorController farmer0;
        [SerializeField] private RuntimeAnimatorController farmer1;
        [SerializeField] private RuntimeAnimatorController farmer2;
        [SerializeField] private RuntimeAnimatorController farmer3;
        private readonly string _mySkin = "MY_SKIN";

        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                RPC_ChangeSkin((PlayerSkin)PlayerPrefs.GetInt(_mySkin));
                Debug.Log("spawned local player");
            }
            RPC_ChangeSkinRemotePlayer();
            
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
        private void RPC_TakeRpc(PlayerSkin skin,RpcInfo info = default)
        {
            if (!HasInputAuthority && !HasStateAuthority)
            {
                SetAnimator(skin);
            }
        }

        public void PlayerLeft(PlayerRef player)
        {
            Runner.Despawn(Object);
            Debug.Log("Despawn Object");
        }
    }
}