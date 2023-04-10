using Fusion;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkWeapon
{
    public class GunBullet : NetworkBehaviour
    {
        #region Private Fields

        private Vector2 _direction;
        private bool _hasDirection = false;
        private float _speed;
        private int _damage;
        private Rigidbody2D _rigidbody2D;
        private IDamageable _damageable;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _rigidbody2D ??= GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                _damageable = col.gameObject.GetComponent<IDamageable>();
                RPC_ForTarget();
                Runner?.Despawn(Object);
            }
        }

        private void OnDisable()
        {
            _hasDirection = false;
            _direction = Vector2.zero;
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            _rigidbody2D ??= GetComponent<Rigidbody2D>();
            Debug.LogError("Spawned bullet by: " + Object.InputAuthority.PlayerId);
        }

        public override void FixedUpdateNetwork()
        {
            if (_hasDirection)
            {
                Vector2 toMove = _rigidbody2D.transform.position;
                if (_speed <= 0)
                {
                    Destroy(gameObject);
                }

                _speed -= 1f;
                toMove.x += _direction.x * _speed * Runner.DeltaTime;
                toMove.y += _direction.y * _speed * Runner.DeltaTime;
                _rigidbody2D.MovePosition(toMove);
            }

            if (_rigidbody2D.position.y < -20f || _rigidbody2D.position.y > 20f || _rigidbody2D.position.x > 25f ||
                _rigidbody2D.position.x < -25f)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Public Methods

        public void SetDirectionAndSpeed(Vector2 direction, float speed, Quaternion quaternion, int damage)
        {
            _direction = direction;
            _damage = damage;
            transform.rotation = quaternion;
            _hasDirection = true;
            _speed = speed;
        }

        #endregion

        #region RPC

        [Rpc]
        private void RPC_ForTarget(RpcInfo info = default)
        {
            _damageable.TakeDamage(_damage, Object.InputAuthority);
        }

        #endregion
    }
}