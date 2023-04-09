using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VitaliyNULL.Factory
{
    public class NetworkEnemyFactory : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private List<NetworkEnemy.NetworkEnemy> enemies;
        [SerializeField] private SpawnPoints spawnPoints;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            InvokeRepeating("Spawn", 1, 5);
        }

        #endregion

        #region Private Methods

        private void Spawn()
        {
            Runner.Spawn(enemies[Random.Range(0, enemies.Count)], spawnPoints.GetRandomPoint().position,
                Quaternion.identity);
        }

        #endregion
    }
}