using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkEnemy
{
    [CreateAssetMenu(menuName = "NetworkEnemyConfig", fileName = "EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        #region Private Fields

        [SerializeField] private float speed;
        [SerializeField] private int health;
        [SerializeField] private int damage;
        [SerializeField] private float radiusOfAttack;
        [SerializeField] private float attackRate;
        [SerializeField] private AudioClip meleeSound;
        [SerializeField] private AudioClip hitSound;
        [SerializeField] private AudioClip deathSound;
        [SerializeField] private EnemyType enemyType;

        #endregion

        #region Public Properties

        public float Speed => speed;
        public int Damage => damage;

        public AudioClip DeathSound => deathSound;
        public EnemyType EnemyType => enemyType;

        public int Health => health;

        public float RadiusOfAttack => radiusOfAttack;
        public float AttackRate => attackRate;
        public AudioClip MeleeSound => meleeSound;
        public AudioClip HitSound => hitSound;

        #endregion
    }
}