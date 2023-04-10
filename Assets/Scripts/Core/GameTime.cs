using System;
using UnityEngine;

namespace VitaliyNULL.Core
{
    public class GameTime
    {
        private int _allTime;
        private int _seconds;
        private int _minutes;

        public override string ToString()
        {
            _seconds = _allTime;
            if (_seconds > 60)
            {
                _minutes = _seconds / 60;
                _seconds %= 60;
            }
            else
            {
                _minutes = 0;
            }
            string seconds = _seconds.ToString("00");
            string minutes = _minutes.ToString("00");
            return String.Format($"{_minutes}:{_seconds}");
        }

        public void SetTime(int minutes, int seconds)
        {
            _allTime = seconds + minutes * 60;
        }

        public void SetTime(int seconds)
        {
            _allTime = seconds;
        }
    }
}