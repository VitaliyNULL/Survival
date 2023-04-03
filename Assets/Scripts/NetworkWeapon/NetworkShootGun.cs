using System.Threading.Tasks;
using UnityEngine;

namespace VitaliyNULL.NetworkWeapon
{
    public class NetworkShootGun : NetworkGun
    {
        private int _countOfBullets = 10;
        private float _range = 3f;

        protected override void SpawnBullet()
        {
            if (HasInputAuthority)
            {
                float step = _range / 10;
                for (int i = 0; i < _countOfBullets; i++)
                {
                    Vector2 direction = _gunDirection.normalized;
                    direction.x += Random.Range(-step, step);
                    direction.y -= Random.Range(-step, step);
                    GunBullet bullet = Runner.Spawn(_gunBullet, transform.position, _gunRotation);
                    bullet.SetDirectionAndSpeed(direction, _bulletSpeed);
                    Debug.Log("Shoot");
                }
            }
        }

    }
}