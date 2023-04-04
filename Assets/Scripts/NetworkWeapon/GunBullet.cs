using Fusion;
using UnityEngine;

namespace VitaliyNULL.NetworkWeapon
{
    public class GunBullet : NetworkBehaviour
    {
        private Vector2 _direction;
        private bool _hasDirection = false;
        private float _speed;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D ??= GetComponent<Rigidbody2D>();
        }

        public void SetDirectionAndSpeed(Vector2 direction, float speed,Quaternion quaternion)
        {
            Debug.Log("Setting direction");
            _direction = direction;
            transform.rotation = quaternion;
            _hasDirection = true;
            _speed = speed;
        }

        public override void Spawned()
        {
            _rigidbody2D ??= GetComponent<Rigidbody2D>();
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

        private void OnDisable()
        {
            _hasDirection = false;
            _direction = Vector2.zero;
        }
    }
}