using System;
using _3._Scripts.Currency.Enums;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Wallet
{
    public static class WalletManager
    {
        public static event Action<float, float> OnFirstCurrencyChange;

        public static float FirstCurrency
        {
            get => GBGames.saves.walletSave.firstCurrency;
            set
            {
                GBGames.saves.walletSave.firstCurrency = value;
                OnFirstCurrencyChange?.Invoke((float) FirstCurrency, (float) value);
            }
        }

        public static event Action<float, float> OnSecondCurrencyChange;

        public static float SecondCurrency
        {
            get => GBGames.saves.walletSave.secondCurrency;
            set
            {
                GBGames.saves.walletSave.secondCurrency = value;
                OnSecondCurrencyChange?.Invoke((float) SecondCurrency, (float) value);
            }
        }
        
        private static void SpendByType(CurrencyType currencyType, float count)
        {
            switch (currencyType)
            {
                case CurrencyType.First:
                    FirstCurrency -= count;
                    break;
                case CurrencyType.Second:
                    SecondCurrency -= count;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null);
            }
        }

        public static bool TrySpend(CurrencyType currencyType, float count)
        {
            var canSpend = currencyType switch
            {
                CurrencyType.First => FirstCurrency >= count,
                CurrencyType.Second => SecondCurrency >= count,
                _ => throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null)
            };

            if (canSpend)
                SpendByType(currencyType, count);

            return canSpend;
        }

        public static string ConvertToWallet(decimal number)
        {
            return number switch
            {
                < 1_000 => number.ToString("0.#"),
                < 1_000_000 => (number / 1_000m).ToString("0.#") + "K",
                < 1_000_000_000 => (number / 1_000_000m).ToString("0.#") + "M",
                < 1_000_000_000_000 => (number / 1_000_000_000m).ToString("0.#") + "B",
                < 1_000_000_000_000_000 => (number / 1_000_000_000_000m).ToString("0.#") + "T",
                < 1_000_000_000_000_000_000 => (number / 1_000_000_000_000_000m).ToString("0.#") + "Qa",
                < 1000_000_000_000_000_000_000m => (number / 1_000_000_000_000_000_000m).ToString("0.#") + "Qi",
                _ => (number / 1_000_000_000_000_000_000_000m).ToString("0.#") + "Sx"
            };
        }

    }
}