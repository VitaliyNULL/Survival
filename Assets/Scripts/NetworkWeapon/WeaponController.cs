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
        private NetworkGun _currentGun;

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
            if (GetInput(out NetworkInputData data) && data.isShoot)
            {
                _currentGun.Shoot();
            }
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
                _currentGun = gun;
                _isGunChose = true;
                Debug.Log("gun is not chose");
            }
        }

        [Rpc]
        private void RPC_ChangeRemoteGun()
        {
            if (HasInputAuthority && HasStateAuthority)
            {
                RPC_TakeChangeGunRPC(_currentGun);
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
                _currentGun = gun;
            }
        }

        #endregion

        #endregion

        #region Private Methods

        private NetworkGun ChooseRandomGun()
        {
            //TODO: Choose random gun 
            int index = Random.Range(0, guns.Count);
            _currentGun = guns[index];
            guns.Remove(_currentGun);
            return _currentGun;
        }

        #endregion
    }
}