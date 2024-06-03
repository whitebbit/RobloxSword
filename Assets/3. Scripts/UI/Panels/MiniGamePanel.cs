using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Enemies;
using _3._Scripts.Localization;
using _3._Scripts.UI.Panels.Base;
using _3._Scripts.Wallet;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public class MiniGamePanel : SimplePanel
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text timer;
        [Tab("Player")] [SerializeField] private Image playerIcon;
        [SerializeField] private TMP_Text playerStrengthText;
        [Tab("Enemy")] [SerializeField] private Image enemyIcon;
        [SerializeField] private TMP_Text enemyStrengthText;
        [SerializeField] private LocalizeStringEvent enemyName;

        public event Action OnLose;
        public event Action OnWin;

        private bool _started;
        private float _playerStrength;
        private float _enemyStrength;
        private float _fillAmount = 0.5f;

        private const float FillRate = 0.5f;
        private const float WinThreshold = 0.95f;

        public void StartGame(float playerStrength, EnemyData enemyData)
        {
            _fillAmount = 0.5f;
            _playerStrength = playerStrength;
            _enemyStrength = enemyData.Strength;
            slider.value = _fillAmount;

            DoCounter();
            UpdatePlayerData();
            UpdateEnemyData(enemyData);
        }


        private void Update()
        {
            if (!_started) return;

            HandleInput();
            UpdateFillAmount();
            slider.value = _fillAmount;
            CheckGameEnd();
        }

        private void DoCounter()
        {
            timer.DOCounter(3, 1, 3).OnComplete(() => _started = true);
            timer.DOFade(1, 0f).OnComplete(() =>
            {
                timer.DOFade(0, 0.5f).SetLoops(4, LoopType.Yoyo)
                    .OnComplete(() => timer.DOFade(0, 0.5f));
            });
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _fillAmount += GetPlayerFillRate();
            }
        }

        private void UpdateFillAmount()
        {
            _fillAmount -= GetEnemyDecreaseRate() * Time.deltaTime;
            _fillAmount = Mathf.Clamp(_fillAmount, 0f, 1f);
        }

        private void CheckGameEnd()
        {
            switch (_fillAmount)
            {
                case <= 0:
                    EndGame(OnLose);
                    break;
                case >= WinThreshold:
                    EndGame(OnWin);
                    break;
            }
        }

        private void EndGame(Action gameEndEvent)
        {
            gameEndEvent?.Invoke();
            _started = false;
            OnLose = null;
            OnWin = null;
        }

        private void UpdateEnemyData(EnemyData enemyData)
        {
            enemyIcon.sprite = enemyData.Icon;
            enemyStrengthText.text = WalletManager.ConvertToWallet((int) Mathf.Ceil(_enemyStrength));
            enemyName.SetReference(enemyData.LocalizeID);
        }

        private void UpdatePlayerData()
        {
            playerIcon.sprite = Configuration.Instance.AllCharacters
                .FirstOrDefault(c => c.ID == GBGames.saves.characterSaves.current)
                ?.Icon;
            playerStrengthText.text = WalletManager.ConvertToWallet((int) _playerStrength);
        }

        private float GetPlayerFillRate() => FillRate / 4 * (_playerStrength / (_playerStrength + _enemyStrength));

        private float GetEnemyDecreaseRate() => FillRate * (_enemyStrength / (_playerStrength + _enemyStrength));
    }
}