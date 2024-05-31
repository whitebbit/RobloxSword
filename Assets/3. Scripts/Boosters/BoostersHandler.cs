using System;
using _3._Scripts.Singleton;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Boosters
{
    public class BoostersHandler : Singleton<BoostersHandler>
    {
        [SerializeField] private BoosterButton autoClickerButton;
        [SerializeField] private BoosterButton incomeButton;
        [SerializeField] private BoosterButton currencyButton;

        public bool UseAutoClicker { get; private set; }
        public bool X2Income { get; private set; }
        public bool X2Currency { get; private set; }

        private void Start()
        {
            autoClickerButton.onActivateBooster += () => UseAutoClicker = true;
            autoClickerButton.onDeactivateBooster += () => UseAutoClicker = false;
            incomeButton.onActivateBooster += () => X2Income = true;
            incomeButton.onDeactivateBooster += () => X2Income = false;
            currencyButton.onActivateBooster += () => X2Currency = true;
            currencyButton.onDeactivateBooster += () => X2Currency = false;
        }
    }
}