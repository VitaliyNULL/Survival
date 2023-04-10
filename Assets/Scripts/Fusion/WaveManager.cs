using Fusion;
using TMPro;
using UnityEngine;
using VitaliyNULL.Core;

namespace VitaliyNULL.Fusion
{
    public class WaveManager : NetworkBehaviour
    {
        [SerializeField] private TMP_Text timeText;
        private bool _isTimerWork;
        private GameTime _gameTime;
        private float _currentTime;
        private int _currentWave = 0;
        private readonly float _firstWaveTime = 60;
        private readonly float _secondWaveTime = 3 * 60;
        private readonly float _thirdWaveTime = 5 * 60;
        private readonly float _relaxTime = 30f;
        private bool _isRelaxing;

        private int CurrentWave
        {
            get => _currentWave;
            set
            {
                _currentWave = Mathf.Clamp(value, 0, 3);
                _currentTime = _relaxTime;
                _isRelaxing = true;
            }
        }

        public override void Spawned()
        {
            _gameTime = new GameTime();
            _currentWave = 1;
            _currentTime = _firstWaveTime;
            _gameTime.SetTime(Mathf.FloorToInt(_currentTime));
            _isTimerWork = true;
            if (!HasInputAuthority)
            {
                Runner.Despawn(Object);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;
            if (_isTimerWork)
            {
                RPC_ChangeTime(Mathf.FloorToInt(_currentTime -= Time.deltaTime));
                if (_currentTime <= 0)
                {
                    CurrentWave++;
                }
            }
        }

        [Rpc]
        private void RPC_ChangeTime(int seconds)
        {
            _gameTime.SetTime(seconds);
            timeText.text = _gameTime.ToString();
        }
    }
}