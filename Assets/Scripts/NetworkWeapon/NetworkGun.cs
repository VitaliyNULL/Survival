using System;
using System.Collections;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.GameSceneUI;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.NetworkWeapon
{
    public class NetworkGun : NetworkBehaviour, INetworkGun
    {
        [SerializeField] protected GunConfig gunConfig;
        private GameUI _gameUI;
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
        private bool _canReload = true;

        private int _currentAmmo;
        private int _allAmmo;

        public int AllAmmo
        {
            get => _allAmmo;
            set
            {
                _allAmmo = Mathf.Clamp(value, 0, _ammoCapacity);
                if (_allAmmo < _storageCapacity)
                {
                    _canReload = false;
                }
            }
        }

        public int CurrentAmmo
        {
            get => _currentAmmo;
            set
            {
                _currentAmmo = Mathf.Clamp(value, 0, _storageCapacity);
                if (_currentAmmo != _storageCapacity && AllAmmo != 0)
                {
                    _canReload = true;
                }
            }
        }
        // private int AllAmmo
        // {
        //     get => _allAmmo;
        //     set
        //     {
        //         _allAmmo = Mathf.Clamp(value, 0, reloadingWeapon.AmmoCapacity);
        //         if (_allAmmo < reloadingWeapon.StorageCapacity)
        //         {
        //             _canReload = false;
        //             print("FUCK");
        //         }
        //     }
        // }
        //
        // private int CurrentAmmo
        // {
        //     get => _currentAmmoStore;
        //     set
        //     {
        //         if (_canShoot)
        //         {
        //             _currentAmmoStore = Mathf.Clamp(value, 0, reloadingWeapon.StorageCapacity);
        //         }
        //
        //         if (_currentAmmoStore != reloadingWeapon.StorageCapacity && AllAmmo != 0)
        //         {
        //             _canReload = true;
        //         }
        //
        //         if (photonView.IsMine) _playerUIManager.UpdateAmmoBar(_currentAmmoStore, _allAmmo);
        //     }
        // }

        public void Shoot()
        {
            // if (_canShoot)
            // {
            //     _canShoot = false;
            //     lastShootTime = _timeToWaitBetweenShoot;
            //     _gunEvent.Invoke(_gunDirection, _bulletSpeed, _gunRotation);
            // }
            if (_canShoot)
            {
                _canShoot = false;
                StartCoroutine(WaitBetweenShoot());
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

        // private void Update()
        // {
        //     if (lastShootTime > 0)
        //     {
        //         _canShoot = false;
        //         lastShootTime -= Time.deltaTime;
        //     }
        //     else
        //     {
        //         _canShoot = true;
        //     }
        // }

        protected virtual void SpawnBullet(Vector2 direction, float speed, Quaternion rotation)
        {
            if (!HasStateAuthority)
            {
                return;
            }

            GunBullet bullet = Runner.Spawn(_gunBullet, transform.position, rotation, Runner.LocalPlayer);
            bullet.SetDirectionAndSpeed(direction, speed, rotation, _damage);
        }

        public void Reload()
        {
            Debug.Log("Reload");
            if (_canReload)
            {
                StartCoroutine(WaitForReload());
            }
        }



        private IEnumerator WaitBetweenShoot()
        {
            _gunEvent.Invoke(_gunDirection, _bulletSpeed, _gunRotation);
            // RPC_GunShoot(_gunDirection, _bulletSpeed, _gunRotation);
            yield return new WaitForSeconds(_timeToWaitBetweenShoot);
            // _gunEvent += RPC_GunShoot;
            _canShoot = true;
        }

        private IEnumerator WaitForReload()
        {
            _gunEvent -= RPC_GunShoot;
            _canReload = false;
            _canShoot = false;
            yield return new WaitForSeconds(_timeToReload);
            _canShoot = true;
            if (AllAmmo <= _storageCapacity)
            {
                CurrentAmmo = AllAmmo;
                AllAmmo = 0;
            }
            else
            {
                AllAmmo -= _storageCapacity - CurrentAmmo;
                CurrentAmmo = _storageCapacity;
            }
            _gunEvent += RPC_GunShoot;
        }

        private void Awake()
        {
            _gunName = gunConfig.GunName;
            _damage = gunConfig.Damage;
            _storageCapacity = gunConfig.StorageCapacity;
            _ammoCapacity = gunConfig.AmmoCapacity;
            _allAmmo = _ammoCapacity;
            _currentAmmo = _storageCapacity;
            _bulletSpeed = gunConfig.BulletSpeed;
            _gunBullet = gunConfig.GunBullet;
            _gunShootSound = gunConfig.GunShootSound;
            _gunImageUI = gunConfig.GunImageUI;
            _timeToWaitBetweenShoot = gunConfig.TimeToWaitBetweenShoot;
            _timeToReload = gunConfig.TimeToReload;
            GunType = gunConfig.GunType;
            _gunEvent += RPC_GunShoot;
            _gameUI = GetComponentInParent<GameUI>();
            _gameUI.SetAmmoUI(CurrentAmmo, AllAmmo);
        }

        #region Only for InputAuthority

        [Rpc]
        private void RPC_GunShoot(Vector2 direction, float speed, Quaternion rotation)
        {
            if (HasStateAuthority)
            {
                SpawnBullet(direction, speed, rotation);
            }
        }

        #endregion
    }
}