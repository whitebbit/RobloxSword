using System;
using TMPro;
using UnityEngine;

namespace _3._Scripts
{
    [RequireComponent(typeof(TMP_Text))]
    public class Timer : MonoBehaviour
    {
        private float _durationInSeconds;
        private DateTime _startTime;
        private bool _isRunning;

        public event Action OnTimerStart;
        public event Action OnTimerEnd;
        private TMP_Text _timerText;
        public bool TimerStopped { get; set; }
        private void Awake()
        {
            _timerText = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            if (!_isRunning) return;

            var remainingTime = _startTime.AddSeconds(_durationInSeconds) - DateTime.Now;
            _timerText.text = GetRemainingTimeFormatted();

            if (remainingTime > TimeSpan.Zero) return;
            _timerText.text = GetRemainingTimeFormatted();
            _isRunning = false;
            OnTimerEnd?.Invoke();
        }

        public void StartTimer(float timeInSeconds)
        {
            OnTimerStart?.Invoke();
            _durationInSeconds = timeInSeconds;
            _startTime = DateTime.Now;
            _isRunning = true;
        }

        public bool TimerEnd()
        {
            return _startTime.AddSeconds(_durationInSeconds) - DateTime.Now <= TimeSpan.Zero;
        }
        public TimeSpan TimeToEnd() => _startTime.AddSeconds(_durationInSeconds) - DateTime.Now;
        public void StopTimer()
        {
            _isRunning = false;
        }

        public void ResetTimer()
        {
            _startTime = DateTime.Now;
            _isRunning = false;
        }

        private string GetRemainingTimeFormatted()
        {
            var remainingTime = _startTime.AddSeconds(_durationInSeconds) - DateTime.Now;
            if (remainingTime <= TimeSpan.Zero)
                remainingTime = TimeSpan.Zero;
            return $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
        }
    }
}