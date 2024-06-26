using System;
using System.Collections.Generic;
using _3._Scripts.Localization;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts.Player
{
    public class PlayerLevel : MonoBehaviour
    {
        private LocalizeStringEvent _text;

        private void Awake()
        {
            _text = GetComponent<LocalizeStringEvent>();
        }

        private void OnEnable()
        {
            OnChange(WalletManager.FirstCurrency, WalletManager.FirstCurrency);
            WalletManager.OnFirstCurrencyChange += OnChange;
        }
        
        private void OnDisable()
        {
            WalletManager.OnFirstCurrencyChange -= OnChange;
        }

        private void OnChange(float _, float newValue)
        {
            var level = Math.Round(newValue / 1000);
            _text.SetVariable("value", level.ToString());

            Leaderboard.Leaderboard.Instance.UpdateScore((int) level);
        }
    }
}