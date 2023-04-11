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
        private bool _isGameOver = false;

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
            _spawnRate = 15f;
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
                        countOfSpawners = 2;
                        for (int i = 0; i < countOfSpawners; i++)
                        {
                            Runner.Spawn(_networkSupplies[(SupplyType)Random.Range(0, 2)],
                                spawnPoints.GetRandomPoint().position, Quaternion.identity);
                        }

                        break;
                    case 3:
                        countOfSpawners = Random.Range(2, 4);
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
            switch (_currentWave)
            {
                case 1:
                    _spawnRate = 15f;
                    break;
                case 2:
                    _spawnRate = 10f;
                    break;
                case 3:
                    _spawnRate = 7f;
                    break;
            }
        }

        public void StartSpawning()
        {
            StartCoroutine(WaitForSpawn());
        }

        public void SetGameOver()
        {
            _isGameOver = true;
        }

        #endregion

        #region Private Methods

        private IEnumerator WaitForSpawn()
        {
            while (!_isGameOver)
            {
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