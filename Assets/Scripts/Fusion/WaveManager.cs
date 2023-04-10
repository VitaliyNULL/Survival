using Fusion;
using TMPro;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.Factory;

namespace VitaliyNULL.Fusion
{
    public class WaveManager : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private TMP_Text timeText;
        private NetworkEnemyFactory _networkEnemyFactory;
        private bool _isTimerWork;
        private GameTime _gameTime;
        private float _currentTime;
        private int _currentWave = 0;
        private readonly float _firstWaveTime = 10;
        private readonly float _secondWaveTime = 3 * 5;
        private readonly float _thirdWaveTime = 5 * 5;
        private readonly float _relaxTime = 5f;
        private bool _isRelaxing;
        private bool _canSpawn;
        private float _waveTime = 0;

        #endregion

        #region Private Properties

        private bool CanSpawn
        {
            get => _canSpawn;
            set
            {
                _canSpawn = value;
                _networkEnemyFactory.SetSpawn(_canSpawn);
            }
        }

        private int CurrentWave
        {
            get => _currentWave;
            set
            {
                _currentWave = Mathf.Clamp(value, 0, 3);
                switch (_currentWave)
                {
                    case 1:
                        _waveTime = _firstWaveTime;
                        break;
                    case 2:
                        _waveTime = _secondWaveTime;
                        break;
                    case 3:
                        _waveTime = _thirdWaveTime;
                        break;
                }
                _currentTime = _relaxTime;
                _isRelaxing = true;
                _networkEnemyFactory.SetWave(_currentWave);
                CanSpawn = false;
            }
        }

        #endregion

        #region NetworkBehaviour Callbacks

        public override void Spawned()
        {
            if (!HasInputAuthority)
            {
                Runner.Despawn(Object);
            }

            _networkEnemyFactory = FindObjectOfType<NetworkEnemyFactory>();
            _gameTime = new GameTime();
            CurrentWave = 1;
            _currentTime = _firstWaveTime;
            _gameTime.SetTime(Mathf.FloorToInt(_currentTime));
            _isTimerWork = true;
            _isRelaxing = false;
            CanSpawn = true;
            _networkEnemyFactory.StartSpawning();
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;
            if (_isTimerWork)
            {
                RPC_ChangeTime(Mathf.FloorToInt(_currentTime -= Time.deltaTime));
                if (_currentTime <= 0 && _isRelaxing)
                {
                    CanSpawn = true;
                    _currentTime = _waveTime;
                    _isRelaxing = false;
                }
                else if (_currentTime <= 0)
                {
                    CurrentWave++;
                }
            }
        }

        #endregion

        #region RPC

        [Rpc]
        private void RPC_ChangeTime(int seconds)
        {
            _gameTime.SetTime(seconds);
            timeText.text = _gameTime.ToString();
        }

        #endregion
    }
}