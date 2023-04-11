using Cinemachine;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.GameSceneUI;
using VitaliyNULL.NetworkWeapon;
using VitaliyNULL.StateMachine;
using VitaliyNULL.Supply;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerController : NetworkBehaviour, IPlayerLeft, IDamageable
    {
        #region Private Fields

        [SerializeField] private StateMachine.StateMachine stateMachine;
        [SerializeField] private WeaponController weaponController;
        private CinemachineVirtualCamera _camera;
        private GameUI _gameUI;
        private Vector3 _deathPosition;
        private readonly int _maxHealth = 15;
        private int _currentHealth;

        private int _damageCount = 0;
        private int _kills = 0;

        #endregion

        #region Public Fields

        [HideInInspector] public bool isDead = false;

        #endregion

        #region Public Methods

        public static PlayerController FindKiller(PlayerRef playerRef)
        {

            var playerController = FindObjectsOfType<PlayerController>();
            foreach (var controller in playerController)
            {
                if (controller.Object.InputAuthority.PlayerId.Equals(Mathf.Abs(playerRef.PlayerId)))
                {
                    return controller;
                }
            }

            return null;
        }

        #endregion

        #region Private Properties

        private int Health
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                RPC_TakeHealthUpdate(_currentHealth, _maxHealth);
                if (_currentHealth == 0)
                {
                    RPC_Death();
                }
            }
        }

        private int Kills
        {
            get => _kills;
            set
            {
                _kills = value;
                if (HasInputAuthority)
                    _gameUI.SetKillsUI(_kills);
            }
        }

        #endregion

        #region Public Properties

        public GameUI GameUI => _gameUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Supply"))
            {
                col.GetComponent<NetworkSupply>().PickUp(this);
            }
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            _currentHealth = _maxHealth;
            if (Object.HasInputAuthority)
            {
                _camera = FindObjectOfType<CinemachineVirtualCamera>();
                _camera.Follow = transform;
                _gameUI = FindObjectOfType<GameUI>();
                _gameUI.SetHpUI(_currentHealth, _maxHealth);
                _gameUI.SetKillsUI(_kills);
                // _gameUI.SetAmmoUI(weaponController.currentGun.CurrentAmmo, weaponController.currentGun.AllAmmo);
            }
        }

        private void FixedUpdate()
        {
            if (isDead)
            {
                transform.position = _deathPosition;
            }
        }

        #endregion

        #region Public Methods

        public void SetDamage()
        {
            if (HasInputAuthority)
            {
                _damageCount += weaponController.currentGun.Damage;
            }
        }

        public void SetKill()
        {
            Kills++;
        }

        public void AddHealth()
        {
            Health += 4;
        }

        public void AddAmmo()
        {
            weaponController.currentGun.AddAmmo();
        }

        #endregion

        #region IPlayerLeft

        public void PlayerLeft(PlayerRef player)
        {
            if (HasInputAuthority) return;
            Runner.Despawn(Object);
            Debug.Log("Despawn Object");
        }

        #endregion

        #region IDamageable

        public void TakeDamage(int damage, PlayerRef playerRef)
        {
            Health -= damage;
            Debug.Log($"Player health is {Health}");
        }

        #endregion

        #region RPC

        [Rpc]
        private void RPC_TakeHealthUpdate(int currentHealth, int maxHealth)
        {
            if (HasInputAuthority)
            {
                _gameUI.SetHpUI(currentHealth, maxHealth);
            }
        }

        [Rpc]
        private void RPC_Death()
        {
            isDead = true;
            _deathPosition = transform.position;
            gameObject.layer = 0;
            tag = "Untagged";
            stateMachine.SwitchState<DeadState>();
            weaponController.gameObject.SetActive(false);
            Destroy(GetComponentInChildren<Collider2D>());
            Destroy(GetComponent<NetworkRigidbody2D>());
            Destroy(GetComponent<Rigidbody2D>());
            Debug.Log("Game Over");
        }
        #endregion
    }
}