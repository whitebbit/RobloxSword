using System;
using System.Linq;
using _3._Scripts.Boosters;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.UI;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Interactive
{
    public class TrainingInteractive : MonoBehaviour, IInteractive
    {
        [SerializeField] private CurrencyCounterEffect effect;

        public void Interact()
        {
            DoIncome();
        }

        public void StopInteract()
        {
        }

        private void DoIncome()
        {
            var obj = EffectPanel.Instance.SpawnEffect(effect);
            var income = Player.Player.Instance.TrainingStrength();
            var position = transform.localPosition;
            obj.Initialize(CurrencyType.First, income);
            transform.DOShakePosition(0.25f, 0.25f, 50).OnComplete(() => transform.localPosition = position);
            WalletManager.FirstCurrency += income;
        }

    }
}