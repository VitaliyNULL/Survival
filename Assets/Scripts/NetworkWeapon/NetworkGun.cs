using System;
using System.Collections;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.NetworkWeapon
{
    public class NetworkGun : NetworkBehaviour, INetworkGun
    {
        [SerializeField] private GunConfig gunConfig;
        private string _gunName;
        private int _damage;
        private int _storageCapacity;
        private int _ammoCapacity;
        private float _bulletSpeed;
        private GunBullet _gunBullet;
        private AudioClip _gunShootSound;
        private Sprite _gunImageUI;
        private float _timeToWaitBetweenShoot;
        private float _timeToReload;
        private Coroutine _waitBetweenShoot;
        private Action _gunEvent;
        private Vector2 _gunDirection;

        public void Shoot()
        {
            _gunEvent?.Invoke();
            _waitBetweenShoot ??= StartCoroutine(WaitBeetwenShoot());
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                _gunDirection = data.directionToShoot;
            }
        }

        private void SpawnBullet()
        {
            GunBullet bullet = Runner.Spawn(_gunBullet, transform.position, Quaternion.identity);
            bullet.SetDirection(_gunDirection);
            Debug.Log("Shoot");
        }

        public void Reload()
        {
            Debug.Log("Reload");
        }

        private IEnumerator WaitBeetwenShoot()
        {
            _gunEvent -= RPC_GunShoot;
            _gunEvent -= RPC_RemoteShoot;
            yield return new WaitForSeconds(_timeToWaitBetweenShoot);
            _waitBetweenShoot = null;
            if (Object.HasInputAuthority)
            {
                _gunEvent += RPC_GunShoot;
            }

            _gunEvent += RPC_RemoteShoot;
        }

        private IEnumerator WaitForReload()
        {
            _gunEvent -= RPC_GunShoot;
            _gunEvent -= RPC_RemoteShoot;
            yield return new WaitForSeconds(_timeToReload);
            if (Object.HasInputAuthority)
            {
                _gunEvent += RPC_GunShoot;
            }

            _gunEvent += RPC_RemoteShoot;
        }

        private void Awake()
        {
            _gunName = gunConfig.GunName;
            _damage = gunConfig.Damage;
            _storageCapacity = gunConfig.StorageCapacity;
            _ammoCapacity = gunConfig.AmmoCapacity;
            _bulletSpeed = gunConfig.BulletSpeed;
            _gunBullet = gunConfig.GunBullet;
            _gunShootSound = gunConfig.GunShootSound;
            _gunImageUI = gunConfig.GunImageUI;
            _timeToWaitBetweenShoot = gunConfig.TimeToWaitBetweenShoot;
            _timeToReload = gunConfig.TimeToReload;
            if (Object.HasInputAuthority)
            {
                _gunEvent += RPC_GunShoot;
            }

            _gunEvent += RPC_RemoteShoot;
        }

        #region Only for InputAuthority

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        private void RPC_GunShoot()
        {
            SpawnBullet();
        }

        #endregion

        [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies)]
        private void RPC_TakeShootRPC()
        {
            if (!HasInputAuthority && !HasStateAuthority)
            {
                SpawnBullet();
            }
        }

        [Rpc]
        private void RPC_RemoteShoot()
        {
            if (HasInputAuthority && HasStateAuthority)
            {
                SpawnBullet();
            }
        }
    }
}