using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VitaliyNULL.Factory
{
    public class NetworkEnemyFactory : NetworkBehaviour
    {
        [SerializeField] private List<NetworkEnemy.NetworkEnemy> enemies;
        [SerializeField] private SpawnPoints spawnPoints;

        private void Start()
        {
            InvokeRepeating("Spawn",1,5);
        }

        private void Spawn()
        {
            Runner.Spawn(enemies[Random.Range(0, enemies.Count)], spawnPoints.GetRandomPoint().position,
                Quaternion.identity);
        }
    }
}
