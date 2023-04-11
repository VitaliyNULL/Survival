using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.Supply;
using Random = UnityEngine.Random;

namespace VitaliyNULL.Factory
{
    public class NetworkSupplyFactory : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private NetworkSupply healSupply;
        [SerializeField] private NetworkSupply bombSupply;
        [SerializeField] private NetworkSupply ammoSupply;
        [SerializeField] private SpawnPoints spawnPoints;
        private float _spawnRate;
        private int _currentWave;

        private Dictionary<SupplyType, NetworkSupply> _networkSupplies =
            new Dictionary<SupplyType, NetworkSupply>();

        private bool _canSpawn;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _networkSupplies[SupplyType.AmmoSupply] = ammoSupply;
            _networkSupplies[SupplyType.HealthSupply] = healSupply;
            _networkSupplies[SupplyType.BombSupply] = bombSupply;
            _spawnRate = 3f;
        }

        #endregion
        
        #region Public Methods

        private void Spawn(int wave)
        {
            if (HasStateAuthority)
            {
                int countOfSpawners = 0;
                switch (wave)
                {
                    case 1:
                        countOfSpawners = 1;
                        for (int i = 0; i < countOfSpawners; i++)
                        {
                            Runner.Spawn(_networkSupplies[SupplyType.AmmoSupply], spawnPoints.GetRandomPoint().position,
                                Quaternion.identity);
                        }

                        break;
                    case 2:
                        countOfSpawners = Random.Range(1, 3);
                        for (int i = 0; i < countOfSpawners; i++)
                        {
                            Runner.Spawn(_networkSupplies[(SupplyType)Random.Range(0, 2)],
                                spawnPoints.GetRandomPoint().position, Quaternion.identity);
                        }

                        break;
                    case 3:
                        countOfSpawners = Random.Range(1, 4);
                        for (int i = 0; i < countOfSpawners; i++)
                        {
                            Runner.Spawn(_networkSupplies[(SupplyType)Random.Range(0, 3)],
                                spawnPoints.GetRandomPoint().position, Quaternion.identity);
                        }

                        break;
                    default:
                        Debug.LogError("Not set wave");
                        break;
                }
            }
        }

        public void SetSpawn(bool canSpawn)
        {
            _canSpawn = canSpawn;
        }

        public void SetWave(int wave)
        {
            _currentWave = wave;
        }

        public void StartSpawning()
        {
            StartCoroutine(WaitForSpawn());
        }

        #endregion

        #region Private Methods

        private IEnumerator WaitForSpawn()
        {
            while (true)
            {
                Debug.Log("Spawn");
                if (_canSpawn)
                {
                    Spawn(_currentWave);
                }

                yield return new WaitForSeconds(_spawnRate);
            }
        }

        #endregion
    }
}