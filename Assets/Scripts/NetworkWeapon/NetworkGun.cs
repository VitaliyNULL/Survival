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
        protected Action<Vector2, float, Quaternion> _gunEvent;
        protected Vector2 _gunDirection;
        protected Quaternion _gunRotation;
        public GunType GunType;

        public void Shoot()
        {
            _waitBetweenShoot ??= StartCoroutine(WaitBetweenShoot());
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                _gunDirection = data.directionToShoot.normalized;
                Vector3 rotation = data.directionToShoot - transform.position;
                float rotateZ = Mathf.Atan2(rotation.y, rotation.z) * Mathf.Rad2Deg;
                _gunRotation = Quaternion.Euler(0, 0, rotateZ);
            }
        }

        protected virtual void SpawnBullet(Vector2 direction, float speed, Quaternion rotation)
        {
            if (!HasStateAuthority) return;
            GunBullet bullet = Runner.Spawn(_gunBullet, transform.position, rotation, Runner.LocalPlayer);
            bullet.SetDirectionAndSpeed(direction, speed, rotation);
        }

        public void Reload()
        {
            Debug.Log("Reload");
        }

        private IEnumerator WaitBetweenShoot()
        {
            _gunEvent?.Invoke(_gunDirection, _bulletSpeed, _gunRotation);
            _gunEvent -= RPC_GunShoot;
            yield return new WaitForSeconds(_timeToWaitBetweenShoot);
            _waitBetweenShoot = null;
            _gunEvent += RPC_GunShoot;
      
        }

        private IEnumerator WaitForReload()
        {
            _gunEvent -= RPC_GunShoot;
            yield return new WaitForSeconds(_timeToReload);
            _gunEvent += RPC_GunShoot;
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
            GunType = gunConfig.GunType;
            _gunEvent += RPC_GunShoot;
        }

        #region Only for InputAuthority

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_GunShoot(Vector2 direction, float speed, Quaternion rotation)
        {
            Debug.LogError("RPC_GunShoot");
            SpawnBullet(direction, speed, rotation);
        }

        #endregion

    }
}