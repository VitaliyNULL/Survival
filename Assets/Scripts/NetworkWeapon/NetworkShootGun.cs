using UnityEngine;

namespace VitaliyNULL.NetworkWeapon
{
    public class NetworkShootGun : NetworkGun
    {
        private int _countOfBullets = 10;
        private float _range = 3f;

        protected override void SpawnBullet(Vector2 direction, float speed, Quaternion rotation)
        {
            if (!HasStateAuthority) return;
            float step = _range / 10;
            for (int i = 0; i < _countOfBullets; i++)
            {
                Vector2 directionNormalized = direction.normalized;
                directionNormalized.x += Random.Range(-step, step);
                directionNormalized.y -= Random.Range(-step, step);
                GunBullet bullet = Runner.Spawn(_gunBullet, transform.position, rotation);
                bullet.SetDirectionAndSpeed(directionNormalized, speed, rotation);
                Debug.Log("Shoot");
            }
        }
    }
}