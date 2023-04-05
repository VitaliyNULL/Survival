using System;
using Cinemachine;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.NetworkWeapon;
using VitaliyNULL.StateMachine;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerController : NetworkBehaviour, IPlayerLeft, IDamageable
    {
        [SerializeField] private StateMachine.StateMachine stateMachine;
        [SerializeField] private GameObject weaponController;
        private CinemachineVirtualCamera _camera;
        [HideInInspector] public bool isDead = false;
        private readonly int _maxHealth = 15;
        private int _currentHealth;

        private int Health
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                if (_currentHealth == 0)
                {
                    isDead = true;
                    gameObject.layer = 0;
                    tag = String.Empty;
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
            }
        }

        #region IPlayerLeft

        public void PlayerLeft(PlayerRef player)
        {
            if(HasInputAuthority) return;
            Runner.Despawn(Object);
            Debug.Log("Despawn Object");
        }

        #endregion

        public void TakeDamage(int damage)
        {
            Health -= damage;
            Debug.Log($"Player health is {Health}");
        }
    }
}