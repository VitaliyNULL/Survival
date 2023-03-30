using Fusion;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerController : NetworkBehaviour
    {
        // static int testcount = 0;
        private Animator _animator;
        [SerializeField] private RuntimeAnimatorController farmer0;
        [SerializeField] private RuntimeAnimatorController farmer1;
        [SerializeField] private RuntimeAnimatorController farmer2;
        [SerializeField] private RuntimeAnimatorController farmer3;
        private readonly string _mySkin = "MY_SKIN";

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                RPC_ChangeSkin((PlayerSkin)PlayerPrefs.GetInt(_mySkin));
                Debug.Log("spawned local player");
            }
            else
            {
                Debug.Log("spawned remote player");
            }
            
        }

        // public void ChangeSkin()
        // {
        //     Debug.Log(Object);
        //     
        //         RPC_ChangeSkin((PlayerSkin)PlayerPrefs.GetInt(_mySkin));
        //         Destroy(Object);
        //         Debug.Log("spawned local player");
        //
        // }
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

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_ChangeSkin(PlayerSkin skin, RpcInfo info = default)
        {
            Debug.Log($"[RPC] {info.Source.PlayerId} called RPC");
            SetAnimator(skin);
        }

    }
}