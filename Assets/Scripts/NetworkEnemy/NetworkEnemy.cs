using System;
using System.Collections;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.NetworkPlayer;
using VitaliyNULL.StateMachine;

namespace VitaliyNULL.NetworkEnemy
{
    public class NetworkEnemy : NetworkBehaviour, IDamageable
    {
        #region Private Fields

        [SerializeField] private EnemyConfig enemyConfig;
        [SerializeField] private StateMachine.StateMachine stateMachine;
        [SerializeField] private LayerMask playerMask;
        private float _speed;
        private int _currentHealth;
        private int _maxHealth;
        private int _damage;
        private float _radiusOfAttack;
        private float _attackRate;
        private AudioClip _meleeSound;
        private AudioClip _hitSound;
        private Collider2D _player;
        private NetworkRigidbody2D _networkRigidbody2D;
        private bool _hasPlayer = false;
        private bool _isAttacked = false;
        private bool _isTakedDamage = false;
        private bool _isDead = false;
        private Vector2 _deathPos;
        private PlayerRef _killer;

        #endregion

        #region Private Properties

        private int Health
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                if (_currentHealth == 0)
                {
                    RPC_Death();
                }
            }
        }

        #endregion

        #region Public Methods

        public void DeathImmediately()
        {
           RPC_ImmediatelyDeath();
        }

        #endregion

        #region Private Methods

        private void Death()
        {
            _isDead = true;
            _deathPos = transform.position;
            StartCoroutine(WaitForDespawnDeadEnemy());
            PlayerController.FindKiller(_killer.PlayerId).SetKill();
        }

        IEnumerator WaitForDespawnDeadEnemy()
        {
            stateMachine.SwitchState<DeadState>();
            tag = "Untagged";
            gameObject.layer = 0;
            var colliders = GetComponents<Collider2D>();
            foreach (var collider in colliders)
            {
                collider.isTrigger = true;
            }

            yield return new WaitForSeconds(5f);
            Runner?.Despawn(Object);
        }

        void Damage(IDamageable damageable, PlayerRef playerRef)
        {
            StartCoroutine(WaitForAttackRate(damageable, playerRef));
        }

        IEnumerator WaitForAttackRate(IDamageable damageable, PlayerRef playerRef)
        {
            _isAttacked = true;
            damageable.TakeDamage(_damage, playerRef);
            yield return new WaitForSeconds(_attackRate);
            _isAttacked = false;
        }

        IEnumerator WaitForTakeDamage()
        {
            _isTakedDamage = true;
            yield return new WaitForSeconds(0.2f);
            _isTakedDamage = false;
            if (!_isDead)
            {
                stateMachine.SwitchState<RunState>();
            }
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _speed = enemyConfig.Speed;
            _damage = enemyConfig.Damage;
            _radiusOfAttack = enemyConfig.RadiusOfAttack;
            _attackRate = enemyConfig.AttackRate;
            _meleeSound = enemyConfig.MeleeSound;
            _hitSound = enemyConfig.HitSound;
            _maxHealth = enemyConfig.Health;
            _currentHealth = _maxHealth;
            _networkRigidbody2D = GetComponent<NetworkRigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isDead) return;
            if (other.CompareTag("Player"))
            {
                _player = other;
                _hasPlayer = true;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (_isDead) return;

            if (other.CompareTag("Player"))
            {
                _player = other;
                _hasPlayer = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_isDead) return;

            if (other.CompareTag("Player"))
            {
                _hasPlayer = false;
                _player = null;
            }
        }

        #endregion
        
        #region NetworkBehaviour Callbacks

        public override void FixedUpdateNetwork()
        {
            if (_isDead)
            {
                transform.position = _deathPos;
                return;
            }

            if (_isTakedDamage) return;
            Vector2 direction = Vector2.zero;
            if (_hasPlayer && _player != null)
            {
                direction = _player.transform.position;
            }

            Vector2 toMove = direction - _networkRigidbody2D.Rigidbody.position;
            toMove.Normalize();
            Vector2 movePos = _networkRigidbody2D.Rigidbody.position;
            movePos.x += _speed * toMove.x * Runner.DeltaTime;
            movePos.y += _speed * toMove.y * Runner.DeltaTime;
            _networkRigidbody2D.Rigidbody.MovePosition(movePos);
            float flip = 0;
            if (toMove.x < 0)
            {
                flip = 180;
            }

            transform.rotation =
                new Quaternion(transform.rotation.x, flip, transform.rotation.z, transform.rotation.w);


            if (!_isAttacked)
            {
                Collider2D collider2D = Physics2D.OverlapCircle(transform.position, _radiusOfAttack, playerMask);
                if (collider2D)
                {
                    Damage(collider2D.gameObject.GetComponent<IDamageable>(), _killer);
                }
            }
        }

        #endregion

        #region IDamageable

        public void TakeDamage(int damage, PlayerRef playerRef)
        {
            RPC_TakeDamage(damage, playerRef);
        }

        #endregion

        #region RPC

        [Rpc]
        private void RPC_TakeDamage(int damage, PlayerRef playerRef)
        {
            _killer = playerRef;
            _deathPos = transform.position;
            StartCoroutine(WaitForTakeDamage());
            stateMachine.SwitchState<HitState>();
            Health -= damage;
            try
            {
                var obj = PlayerController.FindKiller(playerRef);
                PlayerController.FindKiller(playerRef).SetDamage();
            }
            catch (NullReferenceException e)
            {
                Debug.LogError($"NullReference Exception {playerRef.PlayerId} was shot in enemy");
            }
        }

        [Rpc]
        private void RPC_Death()
        {
            Death();
        }

        [Rpc]
        private void RPC_ImmediatelyDeath()
        {
            _isDead = true;
            _deathPos = transform.position;
            StartCoroutine(WaitForDespawnDeadEnemy());
        }

        #endregion
    }
}