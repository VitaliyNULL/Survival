using System.Collections;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkEnemy
{
    public class NetworkEnemy : NetworkBehaviour, IDamageable
    {
        [SerializeField] private EnemyConfig enemyConfig;
        private float _speed;
        private int _currentHealth;
        private int _maxHealth;

        private int Health
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
                if (_currentHealth == 0)
                {
                    Runner?.Despawn(Object);
                }
            }
        }

        private int _damage;
        private float _radiusOfAttack;
        private float _attackRate;
        private AudioClip _meleeSound;
        private AudioClip _hitSound;
        private Collider2D _player;
        private NetworkRigidbody2D _networkRigidbody2D;
        private bool _hasPlayer = false;
        private bool _isAttacked = false;

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
            if (other.CompareTag("Player"))
            {
                _player = other;
                _hasPlayer = true;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _player = other;
                _hasPlayer = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
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
            Health -= damage;
            Debug.Log($"Enemy health is {Health}");
        }
    }
}