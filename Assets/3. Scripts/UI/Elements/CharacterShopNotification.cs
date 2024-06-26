using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Elements
{
    public class CharacterShopNotification : MonoBehaviour
    {
        [SerializeField] private Transform notification;

        private void OnEnable()
        {
            OnChange(WalletManager.SecondCurrency, WalletManager.SecondCurrency);
            WalletManager.OnSecondCurrencyChange += OnChange;
        }

        private void OnDisable()
        {
            WalletManager.OnSecondCurrencyChange -= OnChange;
        }

        private void OnChange(float _, float newValue)
        {
            var current =
                Configuration.Instance.AllCharacters.FirstOrDefault(c => c.ID == GBGames.saves.characterSaves.current);

            var character = Configuration.Instance.AllCharacters.FirstOrDefault(c =>
                current is not null && c.Price <= newValue && !GBGames.saves.characterSaves.Unlocked(c.ID) &&
                c.Price > current.Price);

            notification.gameObject.SetActive(character != null);
        }
    }
}