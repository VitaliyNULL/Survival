using System;
using Fusion;
using UnityEngine;

namespace VitaliyNULL.NetworkWeapon
{
    public class GunBullet : NetworkBehaviour
    {
        private Vector2 _direction;
        private bool _hasDirection = false;

        public void SetDirection(Vector2 direction)
        {
            // TODO: Implement code here
            _direction = direction;
            _hasDirection = true;
        }


        private void Update()
        {
            if (_hasDirection)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(transform.position.x + _direction.x, transform.position.y + _direction.y,
                        transform.position.z),
                    20 * Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            _hasDirection = false;
            _direction = Vector2.zero;
        }
    }
}