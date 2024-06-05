using System;
using GBGamesPlugin;
using TMPro;
using UnityEngine;

namespace _3._Scripts
{
    [RequireComponent(typeof(TMP_Text))]
    public class Timer : MonoBehaviour
    {
        private float _durationInSeconds;
        private float _startTime;
        private bool _isRunning;
        private bool _isPaused;
        private TMP_Text _timerText;

        public event Action OnTimerStart;
        public event Action OnTimerEnd;

        private bool _timerStopped;
        public bool TimerStopped
        {
            get => _timerStopped;
            set
            {
                if (_timerStopped == value) return;
                _timerStopped = value;
                if (_timerStopped)
                {
                    StopTimer();
                }
                else
                {
                    StartTimer(_durationInSeconds - (Time.time - _startTime));
                }
            }
        }

        private void Awake()
        {
            _timerText = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            if (!_isRunning || _isPaused) return;

            var remainingTime = Mathf.Max(0, _durationInSeconds - (Time.time - _startTime));
            _timerText.text = FormatTime(remainingTime);

            if (remainingTime <= 0)
            {
                _isRunning = false;
                OnTimerEnd?.Invoke();
            }
        }

        public void StartTimer(float timeInSeconds)
        {
            OnTimerStart?.Invoke();
            _durationInSeconds = timeInSeconds;
            _startTime = Time.time;
            _isRunning = true;
            _isPaused = false;
            _timerStopped = false;
        }

        public bool TimerEnd()
        {
            return Time.time - _startTime >= _durationInSeconds;
        }

        public TimeSpan TimeToEnd()
        {
            float remainingTime = Mathf.Max(0, _durationInSeconds - (Time.time - _startTime));
            return TimeSpan.FromSeconds(remainingTime);
        }

        public void StopTimer()
        {
            _isRunning = false;
        }

        public void ResetTimer()
        {
            _startTime = Time.time;
            _isRunning = false;
            _timerStopped = true;
        }

        private string FormatTime(float timeInSeconds)
        {
            var ts = TimeSpan.FromSeconds(timeInSeconds);
            return $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
        }

        private void OnEnable()
        {
            GBGames.GameVisibleStateCallback += OnGameVisibleStateCallback;
            GBGames.GameHiddenStateCallback += OnGameHiddenStateCallback;
        }

        private void OnDisable()
        {
            GBGames.GameVisibleStateCallback -= OnGameVisibleStateCallback;
            GBGames.GameHiddenStateCallback -= OnGameHiddenStateCallback;
        }

        private void OnGameHiddenStateCallback()
        {
            _isPaused = true;
        }

        private void OnGameVisibleStateCallback()
        {
            _isPaused = false;
        }
    }
}
