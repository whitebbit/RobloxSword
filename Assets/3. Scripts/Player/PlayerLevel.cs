using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
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
            if (newValue <= 0)
            {
                _text.SetVariable("value", 0.ToString());
                Leaderboard.Leaderboard.Instance.UpdateScore(0);
                return;
            }
            var normalizedPower = Math.Log10(newValue);
            var swords = Configuration.Instance.SwordData.OrderBy(obj => obj.StrengthBooster).ToList();
            var characters = Configuration.Instance.AllCharacters.OrderBy(obj => obj.Booster).ToList();

            var currentSword =
                Configuration.Instance.SwordData.FirstOrDefault(s => GBGames.saves.swordSaves.IsCurrent(s.ID));
            var currentCharacter =
                Configuration.Instance.AllCharacters.FirstOrDefault(s => GBGames.saves.characterSaves.IsCurrent(s.ID));
            
            double totalBooster = 1 + swords.IndexOf(currentSword) + characters.IndexOf(currentCharacter);
            
            var playerLevel = (int)(normalizedPower * totalBooster);
            _text.SetVariable("value", playerLevel.ToString());

            Leaderboard.Leaderboard.Instance.UpdateScore(playerLevel);
        }
    }
}