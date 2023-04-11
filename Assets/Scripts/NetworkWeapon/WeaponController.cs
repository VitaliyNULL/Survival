using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VitaliyNULL.NetworkPlayer;
using Random = UnityEngine.Random;

namespace VitaliyNULL.NetworkWeapon
{
    public class WeaponController : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private List<NetworkGun> guns = new List<NetworkGun>();
        private bool _isGunChose;
        [HideInInspector]public NetworkGun currentGun;
        private bool _isGameOver = false;

        #endregion

        #region NetworBehaviour Callbacks

        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                RPC_ChooseGun(ChooseRandomGun());
            }

            RPC_ChangeRemoteGun();
        }

        public override void FixedUpdateNetwork()
        {
            if(_isGameOver) return;
            if (GetInput(out NetworkInputData data) && data.isShoot)
            {
                currentGun.Shoot(Object.InputAuthority);
            }
        }

        #endregion

        #region Public Methods

        public void SetGameOver()
        {
            _isGameOver = true;
        }

        #endregion
        #region RPC

        #region Rpc for All

        [Rpc]
        private void RPC_ChooseGun(NetworkGun gun)
        {
            //TODO: SPAWN GUN
            if (_isGunChose)
            {
                Debug.Log("gun is chose");
            }
            else
            {
                // Runner.Spawn(ChooseRandomGun(), transform.position, Quaternion.identity, Runner.LocalPlayer);
                gun.gameObject.SetActive(true);
                currentGun = gun;
                _isGunChose = true;
                Debug.Log("gun is not chose");
            }
        }

        [Rpc]
        private void RPC_ChangeRemoteGun()
        {
            if (HasInputAuthority && HasStateAuthority)
            {
                RPC_TakeChangeGunRPC(currentGun);
            }
        }

        #endregion


        #region Only for StateAuthority

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_TakeChangeGunRPC(NetworkGun gun)
        {
            if (!HasInputAuthority && !HasStateAuthority)
            {
                gun.gameObject.SetActive(true);
                currentGun = gun;
            }
        }

        #endregion

        #endregion

        #region Private Methods

        private NetworkGun ChooseRandomGun()
        {
            //TODO: Choose random gun 
            int index = Random.Range(0, guns.Count);
            currentGun = guns[index];
            guns.Remove(currentGun);
            return currentGun;
        }

        #endregion
    }
}