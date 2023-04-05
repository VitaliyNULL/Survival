using Cinemachine;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkPlayer
{
    public class PlayerController : NetworkBehaviour, IPlayerLeft, IDamageable
    {
        private CinemachineVirtualCamera _camera;
        private readonly int _maxHealth = 15;
        private int _currentHealth;

        private int Health
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                if (_maxHealth == 0)
                {
                    Runner.Despawn(Object);
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