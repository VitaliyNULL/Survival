using System;
using System.Collections;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.StateMachine;

namespace VitaliyNULL.NetworkEnemy
{
    public class NetworkEnemy : NetworkBehaviour, IDamageable
    {
        [SerializeField] private EnemyConfig enemyConfig;
        [SerializeField] private StateMachine.StateMachine stateMachine;
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
        private int Health
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                if (_currentHealth == 0)
                {
                    _isDead = true;
                    _deathPos = transform.position;
                    StartCoroutine(WaitForDespawnDeadEnemy());
                }
            }
        }

        IEnumerator WaitForDespawnDeadEnemy()
        {
            stateMachine.SwitchState<DeadState>();
            tag = String.Empty;
            gameObject.layer = 0;
            var colliders = GetComponents<Collider2D>();
            foreach (var collider in colliders)
            {
                collider.isTrigger = true;
            }
            yield return new WaitForSeconds(5f);
            Runner.Despawn(Object);
        }
 

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
            if(_isDead) return;
            if (other.CompareTag("Player"))
            {
                _player = other;
                _hasPlayer = true;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if(_isDead) return;

            if (other.CompareTag("Player"))
            {
                _player = other;
                _hasPlayer = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(_isDead) return;

            if (other.CompareTag("Player"))
            {
                _hasPlayer = false;
                _player = null;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if(_isDead) return;

            if (!_isAttacked)
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    Damage(col.gameObject.GetComponent<IDamageable>());
                }
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if(_isDead) return;

            if (!_isAttacked)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    Damage(collision.gameObject.GetComponent<IDamageable>());
                }
            }
        }

        void Damage(IDamageable damageable)
        {
            StartCoroutine(WaitForAttackRate(damageable));
        }

        IEnumerator WaitForAttackRate(IDamageable damageable)
        {
            _isAttacked = true;
            damageable.TakeDamage(_damage);
            yield return new WaitForSeconds(_attackRate);
            _isAttacked = false;
        }


        public override void FixedUpdateNetwork()
        {
            if (_isDead)
            {
                transform.position = _deathPos;
                return;
            }
            if(_isTakedDamage) return;
            Vector2 direction = Vector2.zero;
            if (_hasPlayer)
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
        }

        public void TakeDamage(int damage)
        {
            StartCoroutine(WaitForTakeDamage());
            stateMachine.SwitchState<HitState>();
            Health -= damage;
            Debug.Log($"Enemy health is {Health}");
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
    }
}