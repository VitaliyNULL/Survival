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
        [SerializeField] protected GunConfig gunConfig;
        protected string _gunName;
        protected int _damage;
        protected int _storageCapacity;
        protected int _ammoCapacity;
        protected float _bulletSpeed;
        protected GunBullet _gunBullet;
        protected AudioClip _gunShootSound;
        protected Sprite _gunImageUI;
        protected float _timeToWaitBetweenShoot;
        protected float _timeToReload;
        protected Coroutine _waitBetweenShoot;
        protected Action _gunEvent;
        protected Vector2 _gunDirection;
        protected Quaternion _gunRotation;

        public void Shoot()
        {
            _gunEvent?.Invoke();
            Debug.Log("Invoke gunEvent");
            _waitBetweenShoot ??= StartCoroutine(WaitBetweenShoot());
            Debug.Log("Invoke Coroutine");
        }

        public override void FixedUpdateNetwork()
        {
            if(!HasInputAuthority) return;
            if (GetInput(out NetworkInputData data))
            {
                _gunDirection = data.directionToShoot.normalized;
                Vector3 rotation = data.directionToShoot - transform.position;
                float rotateZ = Mathf.Atan2(rotation.y, rotation.z) * Mathf.Rad2Deg;
                _gunRotation = Quaternion.Euler(0, 0, rotateZ);
            }
        }

        protected virtual void SpawnBullet()
        {
            if (HasInputAuthority)
            {
                GunBullet bullet = Runner.Spawn(_gunBullet, transform.position, _gunRotation);
                Debug.Log($"Bullet Speed {_bulletSpeed} Gun Direction {_gunDirection}");
                bullet.SetDirectionAndSpeed(_gunDirection, _bulletSpeed);
                Debug.Log("Shoot");
            }
        }

        public void Reload()
        {
            Debug.Log("Reload");
        }

        private IEnumerator WaitBetweenShoot()
        {
            _gunEvent -= RPC_GunShoot;
            _gunEvent -= RPC_RemoteShoot;
            yield return new WaitForSeconds(_timeToWaitBetweenShoot);
            _waitBetweenShoot = null;
            if (HasStateAuthority)
            {
                _gunEvent += RPC_RemoteShoot;
            }
            else if (Object.HasInputAuthority)
            {
                _gunEvent += RPC_GunShoot;
            }
        }

        private IEnumerator WaitForReload()
        {
            _gunEvent -= RPC_GunShoot;
            _gunEvent -= RPC_RemoteShoot;
            yield return new WaitForSeconds(_timeToReload);
            if (HasStateAuthority)
            {
                _gunEvent += RPC_RemoteShoot;
            }
            else if (Object.HasInputAuthority)
            {
                _gunEvent += RPC_GunShoot;
            }
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
            if (HasStateAuthority)
            {
                _gunEvent += RPC_RemoteShoot;
            }
            else if (Object.HasInputAuthority)
            {
                _gunEvent += RPC_GunShoot;
            }
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
                // RPC_TakeShootRPC();
            }
        }
    }
}