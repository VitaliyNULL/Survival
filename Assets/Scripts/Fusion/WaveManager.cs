using Fusion;
using TMPro;
using UnityEngine;
using VitaliyNULL.Core;
using VitaliyNULL.Factory;
using VitaliyNULL.NetworkPlayer;

namespace VitaliyNULL.Fusion
{
    public class WaveManager : NetworkBehaviour
    {
        #region Private Fields

        [SerializeField] private TMP_Text timeText;
        private NetworkEnemyFactory _networkEnemyFactory;
        private NetworkSupplyFactory _networkSupplyFactory;
        private bool _isTimerWork;
        private GameTime _gameTime;
        private float _currentTime;
        private int _currentWave = 0;
        private bool _isGameOver = false;
        private readonly float _firstWaveTime = 60;
        private readonly float _secondWaveTime = 60 * 3;
        private readonly float _thirdWaveTime = 60 * 5;
        private readonly float _relaxTime = 30f;
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
                _networkSupplyFactory.SetSpawn(_canSpawn);
            }
        }

        private int CurrentWave
        {
            get => _currentWave;
            set
            {
                _currentWave = Mathf.Clamp(value, 0, 4);
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
                    case 4:
                        RPC_GameOver();
                        break;
                }

                _currentTime = _relaxTime;
                _isRelaxing = true;
                _networkEnemyFactory.SetWave(_currentWave);
                _networkSupplyFactory.SetWave(_currentWave);
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
            _networkSupplyFactory = FindObjectOfType<NetworkSupplyFactory>();
            _gameTime = new GameTime();
            CurrentWave = 1;
            _currentTime = _firstWaveTime;
            _gameTime.SetTime(Mathf.FloorToInt(_currentTime));
            _isTimerWork = true;
            _isRelaxing = false;
            CanSpawn = true;
            _networkEnemyFactory.StartSpawning();
            _networkSupplyFactory.StartSpawning();
        }

        public override void FixedUpdateNetwork()
        {
            if (_isGameOver) return;
            if (!HasStateAuthority) return;
            if (_isTimerWork)
            {
                RPC_ChangeTime(Mathf.FloorToInt(_currentTime -= Runner.DeltaTime));
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

        #region Public Methods

        public void SetGameOver()
        {
            RPC_GameOver();
        }
        

        #endregion
        #region RPC

        [Rpc]
        private void RPC_ChangeTime(int seconds)
        {
            _gameTime.SetTime(seconds);
            timeText.text = _gameTime.ToString();
        }

        [Rpc]
        private void RPC_GameOver()
        {
            Debug.Log("Winner");
            _isGameOver = true;
            _isTimerWork = false;
            _networkEnemyFactory.SetSpawn(false);
            _networkSupplyFactory.SetSpawn(false);
            timeText.gameObject.SetActive(false);
            _networkEnemyFactory.SetGameOver();
            _networkSupplyFactory.SetGameOver();
            Debug.LogError($"Current player is {Runner.LocalPlayer.PlayerId}");
            foreach (var playerRef in Runner.ActivePlayers)
            {
                PlayerController playerController = PlayerController.FindPlayer(playerRef);

                playerController.SpawnLeaderboardContainer(out GameObject disconnectButton);
                disconnectButton.SetActive(true);
                playerController.GameOver();
            }
        }
        #endregion
    }
}