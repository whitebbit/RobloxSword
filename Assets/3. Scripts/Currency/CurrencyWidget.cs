using System;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Wallet;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Currency
{
    public class CurrencyWidget : MonoBehaviour
    {
        [Tab("Components")]
        [SerializeField] private CurrencyType type;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image icon;
        [SerializeField] private Image table;


        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            var currency = Configuration.Instance.GetCurrency(type);
            icon.sprite = currency.Icon;
            table.color = currency.DarkColor;
            switch (type)
            {
                case CurrencyType.First:
                    OnChange((float)WalletManager.FirstCurrency, (float)WalletManager.FirstCurrency);
                    break;
                case CurrencyType.Second:
                    OnChange((float)WalletManager.SecondCurrency, (float)WalletManager.SecondCurrency);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnEnable()
        {
            switch (type)
            {
                case CurrencyType.First:
                    WalletManager.OnFirstCurrencyChange += OnChange;
                    OnChange((float)WalletManager.FirstCurrency, (float)WalletManager.FirstCurrency);
                    break;
                case CurrencyType.Second:
                    WalletManager.OnSecondCurrencyChange += OnChange;
                    OnChange((float)WalletManager.SecondCurrency, (float)WalletManager.SecondCurrency);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDisable()
        {
            switch (type)
            {
                case CurrencyType.First:
                    WalletManager.OnFirstCurrencyChange -= OnChange;
                    break;
                case CurrencyType.Second:
                    WalletManager.OnSecondCurrencyChange -= OnChange;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnChange(float oldValue, float newValue)
        {
            text.text = WalletManager.ConvertToWallet((decimal) newValue);
        }
    }
}