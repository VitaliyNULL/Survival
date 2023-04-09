using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkWeapon
{
    [CreateAssetMenu(menuName = "NetworkGunConfig", fileName = "GunConfig")]
    public class GunConfig : ScriptableObject
    {
        #region Private Fields

        [Header("Info about gun")] [SerializeField]
        private string gunName;

        [SerializeField] private int damage;
        [SerializeField] private int storageCapacity;
        [SerializeField] private int ammoCapacity;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float timeToWaitBetweenShoot;
        [SerializeField] private float timeToReload;

        [Header("Objects that gun has")] [SerializeField]
        private GunBullet gunBullet;

        [SerializeField] private AudioClip gunShootSound;
        [SerializeField] private Sprite gunImageUI;
        [SerializeField] private GunType gunType;

        #endregion

        #region Public Properties

        public AudioClip GunShootSound => gunShootSound;
        public int Damage => damage;
        public int StorageCapacity => storageCapacity;
        public int AmmoCapacity => ammoCapacity;
        public string GunName => gunName;
        public Sprite GunImageUI => gunImageUI;
        public float BulletSpeed => bulletSpeed;
        public GunBullet GunBullet => gunBullet;
        public float TimeToWaitBetweenShoot => timeToWaitBetweenShoot;
        public float TimeToReload => timeToReload;

        public GunType GunType => gunType;

        #endregion
    }
}