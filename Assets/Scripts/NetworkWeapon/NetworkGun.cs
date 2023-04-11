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
        #region Protected Fields

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
        protected event Action<Vector2, float, Quaternion, PlayerRef> _gunEvent;
        protected Vector2 _gunDirection;
        protected Quaternion _gunRotation;
        protected bool _canShoot;

        #endregion

        #region Private Fields

        private GameUI _gameUI;
        private float lastShootTime = 0;
        private bool _canReload = true;

        private int _currentAmmo;
        private int _allAmmo;

        #endregion

        #region Public Fields

        public GunType GunType;

        #endregion

        #region Public Properties

        public int Damage => _damage;

        #endregion

        #region Private Properties

        private int AllAmmo
        {
            get => _allAmmo;
            set
            {
                _allAmmo = Mathf.Clamp(value, 0, _ammoCapacity);
                RPC_TakeUpdate(_currentAmmo, _allAmmo);
                if (_allAmmo < _storageCapacity)
                {
                    _canReload = false;
                }

                if (_allAmmo == 0)
                {
                    _canReload = false;
                    _canShoot = false;
                }
            }
        }

        private int CurrentAmmo
        {
            get => _currentAmmo;
            set
            {
                _currentAmmo = Mathf.Clamp(value, 0, _storageCapacity);
                RPC_TakeUpdate(_currentAmmo, _allAmmo);
                if (_currentAmmo == 0 && _allAmmo == 0)
                {
                    _canReload = false;
                    _canShoot = false;
                    return;
                }

                if (value == 0 && _currentAmmo == 0)
                {
                    Reload();
                    return;
                }

                if (_currentAmmo != _storageCapacity && AllAmmo != 0)
                {
                    _canReload = true;
                }
            }
        }

        #endregion

        #region Public Methods

        public void AddAmmo()
        {
            AllAmmo += _storageCapacity;
        }
        public void Shoot(PlayerRef playerRef)
        {
            if (_canShoot)
            {
                _canShoot = false;
                _gunEvent?.Invoke(_gunDirection, _bulletSpeed, _gunRotation, playerRef);
            }
        }

        #endregion

        #region NetworkBehaviour Callbacks

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

        #endregion

        #region Protected Methods

        protected virtual void SpawnBullet(Vector2 direction, float speed, Quaternion rotation, PlayerRef playerRef)
        {
            if (!HasStateAuthority)
            {
                return;
            }

            StartCoroutine(WaitBetweenShoot());
            GunBullet bullet = Runner.Spawn(_gunBullet, transform.position, rotation, playerRef);
            bullet.SetDirectionAndSpeed(direction, speed, rotation, _damage);
        }

        protected IEnumerator WaitBetweenShoot()
        {
            CurrentAmmo -= 1;
            yield return new WaitForSeconds(_timeToWaitBetweenShoot);
            // if (CurrentAmmo == 0 && AllAmmo == 0)
            // {
            //     _canShoot = false;
            // }
            // else
            // {
            //     _canShoot = true;
            // }
            _canShoot = !(CurrentAmmo == 0 && AllAmmo == 0);
        }

        #endregion

        #region INetworkGun

        public void Reload()
        {
            if (_canReload)
            {
                Debug.Log("Reload");
                StartCoroutine(WaitForReload());
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator WaitForReload()
        {
            _canReload = false;
            _canShoot = false;
            _gunEvent -= RPC_GunShoot;
            yield return new WaitForSeconds(_timeToReload);
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
            _canShoot = true;
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _gunName = gunConfig.GunName;
            _damage = gunConfig.Damage;
            _storageCapacity = gunConfig.StorageCapacity;
            _ammoCapacity = gunConfig.AmmoCapacity;
            _allAmmo = Mathf.FloorToInt(_ammoCapacity/2);
            _currentAmmo = _storageCapacity;
            _bulletSpeed = gunConfig.BulletSpeed;
            _gunBullet = gunConfig.GunBullet;
            _gunShootSound = gunConfig.GunShootSound;
            _gunImageUI = gunConfig.GunImageUI;
            _timeToWaitBetweenShoot = gunConfig.TimeToWaitBetweenShoot;
            _timeToReload = gunConfig.TimeToReload;
            GunType = gunConfig.GunType;
            _gunEvent += RPC_GunShoot;
            if (HasInputAuthority)
            {
                _gameUI = FindObjectOfType<GameUI>();
                _gameUI.SetAmmoUI(CurrentAmmo, AllAmmo);
            }

            if (HasStateAuthority) _canShoot = true;
        }

        #endregion

        #region Only for InputAuthority

        [Rpc]
        private void RPC_TakeUpdate(int currentAmmo, int allAmmo)
        {
            if (HasInputAuthority)
            {
                _gameUI.SetAmmoUI(currentAmmo, allAmmo);
            }
        }

        [Rpc]
        private void RPC_GunShoot(Vector2 direction, float speed, Quaternion rotation, PlayerRef playerRef)
        {
            if (HasStateAuthority)
            {
                SpawnBullet(direction, speed, rotation, playerRef);
            }
        }

        #endregion
    }
}