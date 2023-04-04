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
        protected bool _canShoot = true;
        private float lastShootTime = 0;

        public void Shoot()
        {
            if (_canShoot)
            {
                _canShoot = false;
                lastShootTime = _timeToWaitBetweenShoot;
                _gunEvent.Invoke(_gunDirection, _bulletSpeed, _gunRotation);
            }
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

        private void Update()
        {
            if (lastShootTime > 0)
            {
                _canShoot = false;
                lastShootTime -= Time.deltaTime;
            }
            else
            {
                _canShoot = true;
            }
        }

        protected virtual void SpawnBullet(Vector2 direction, float speed, Quaternion rotation)
        {
            if (!HasStateAuthority)
            {
                Debug.Log("Has no state authority");
                return;
            }

            GunBullet bullet = Runner.Spawn(_gunBullet, transform.position, rotation, Runner.LocalPlayer);
            Debug.LogError("Spawned Bullet");
            bullet.SetDirectionAndSpeed(direction, speed, rotation);
        }

        public void Reload()
        {
            Debug.Log("Reload");
        }


        private IEnumerator WaitBetweenShoot()
        {
            Debug.LogError("Start Shoot");
            // _gunEvent.Invoke(_gunDirection, _bulletSpeed, _gunRotation);
            RPC_GunShoot(_gunDirection, _bulletSpeed, _gunRotation);
            yield return new WaitForSeconds(_timeToWaitBetweenShoot);
            // _gunEvent += RPC_GunShoot;
            Debug.LogError("Stop Shoot");
            _canShoot = true;
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