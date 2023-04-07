using System.Linq;
using Cinemachine;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.GameSceneUI;
using VitaliyNULL.NetworkWeapon;
using VitaliyNULL.StateMachine;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerController : NetworkBehaviour, IPlayerLeft, IDamageable
    {
        [SerializeField] private StateMachine.StateMachine stateMachine;
        [SerializeField] private WeaponController weaponController;
        private CinemachineVirtualCamera _camera;
        private GameUI _gameUI;
        [HideInInspector] public bool isDead = false;
        private readonly int _maxHealth = 15;
        private int _currentHealth;

        private int _damageCount = 0;
        private int _kills = 0;

        public static PlayerController FindKiller(PlayerRef playerRef)
        {
            return FindObjectsOfType<PlayerController>()
                .SingleOrDefault(x => x.Object.StateAuthority.Equals(playerRef));
        }
        private int Health
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                RPC_TakeHealthUpdate(_currentHealth, _maxHealth);
                if (_currentHealth == 0)
                {
                    isDead = true;
                    gameObject.layer = 0;
                    tag = "Untagged";
                    stateMachine.SwitchState<DeadState>();
                    weaponController.gameObject.SetActive(false);
                    GetComponentInChildren<Collider2D>().isTrigger = true;
                    Debug.Log("Game Over");
                }
            }
        }

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

        public void SetDamage()
        {
            _damageCount += weaponController.currentGun.Damage;
        }

        public void SetKill()
        {
            _kills++;
        }

        public GameUI GameUI => _gameUI;

        #region IPlayerLeft

        public void PlayerLeft(PlayerRef player)
        {
            if (HasInputAuthority) return;
            Runner.Despawn(Object);
            Debug.Log("Despawn Object");
        }

        #endregion

        public void TakeDamage(int damage)
        {
            Health -= damage;
            Debug.Log($"Player health is {Health}");
        }

        [Rpc]
        private void RPC_TakeHealthUpdate(int currentHealth, int maxHealth)
        {
            if (HasInputAuthority)
            {
                _gameUI.SetHpUI(currentHealth, maxHealth);
            }
        }
    }
}