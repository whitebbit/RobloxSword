using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Ads;
using _3._Scripts.Config;
using _3._Scripts.Enemies;
using _3._Scripts.Inputs;
using _3._Scripts.Localization;
using _3._Scripts.UI.Extensions;
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
        [SerializeField] private CountdownTimer timer;
        [SerializeField] private List<Transform> deactivateObjects = new();

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
        private const float WinThreshold = 0.9f;

        public void StartGame(float playerStrength, EnemyData enemyData)
        {
            _fillAmount = 0.5f;
            _playerStrength = playerStrength;
            _enemyStrength = enemyData.CurrentStrength;
            slider.value = _fillAmount;
            DoCounter();
            UpdatePlayerData();
            UpdateEnemyData(enemyData);
        }

        protected override void OnOpen()
        {
            InterstitialsTimer.Instance.Blocked = true;
            base.OnOpen();

            foreach (var obj in deactivateObjects)
            {
                obj.gameObject.SetActive(false);
            }
            InputHandler.Instance.SetState(false);
        }

        protected override void OnClose()
        {
            InterstitialsTimer.Instance.Blocked = false;
            base.OnClose();
            
            foreach (var obj in deactivateObjects)
            {
                obj.gameObject.SetActive(true);
            }
            InputHandler.Instance.SetState(true);
        }

        private void Update()
        {
            if (!_started) return;
            if (InterstitialsTimer.Instance.Active) return;

            HandleInput();
            UpdateFillAmount();
            slider.value = _fillAmount;
            CheckGameEnd();
        }

        private void DoCounter()
        {
            timer.StartCountdown(() => _started = true);
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
            InterstitialsTimer.Instance.Blocked = false;

            OnLose = null;
            OnWin = null;
        }

        private void UpdateEnemyData(EnemyData enemyData)
        {
            enemyIcon.sprite = enemyData.Icon;
            enemyStrengthText.text = WalletManager.ConvertToWallet((decimal) _enemyStrength);
            enemyName.SetReference(enemyData.LocalizeID);
        }

        private void UpdatePlayerData()
        {
            playerIcon.sprite = Configuration.Instance.AllCharacters
                .FirstOrDefault(c => c.ID == GBGames.saves.characterSaves.current)
                ?.Icon;
            playerStrengthText.text = WalletManager.ConvertToWallet((decimal) _playerStrength);
        }

        private float GetPlayerFillRate() => FillRate / 4 * (_playerStrength / (_playerStrength + _enemyStrength));

        private float GetEnemyDecreaseRate() => FillRate * (_enemyStrength / (_playerStrength + _enemyStrength));
    }
}