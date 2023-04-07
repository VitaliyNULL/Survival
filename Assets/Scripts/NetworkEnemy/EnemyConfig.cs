using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.NetworkEnemy
{
    [CreateAssetMenu(menuName = "NetworkEnemyConfig", fileName = "EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField] private float speed;
        [SerializeField] private int health;
        [SerializeField] private int damage;
        [SerializeField] private float radiusOfAttack;
        [SerializeField] private float attackRate;
        [SerializeField] private AudioClip meleeSound;
        [SerializeField] private AudioClip hitSound;
        [SerializeField] private EnemyType enemyType;
        public float Speed => speed;
        public int Damage => damage;

        public EnemyType EnemyType => enemyType;

        public int Health => health;

        public float RadiusOfAttack => radiusOfAttack;
        public float AttackRate => attackRate;
        public AudioClip MeleeSound => meleeSound;
        public AudioClip HitSound => hitSound;
    }
}