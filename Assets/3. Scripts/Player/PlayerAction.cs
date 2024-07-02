using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Ads;
using _3._Scripts.Boosters;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Detectors;
using _3._Scripts.Inputs;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.Sounds;
using _3._Scripts.UI;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Panels;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.Wallet;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;
using VInspector;
using Random = System.Random;

namespace _3._Scripts.Player
{
    public class PlayerAction : MonoBehaviour
    {
        [SerializeField] private List<AnimationClip> actionAnimations = new();
        [Tab("UI")] [SerializeField] private CurrencyCounterEffect effect;


        private IInput _input;
        private bool _isOnCooldown;
        private PlayerAnimator _animator;

        private IInteractive _interactive;

        private void Awake()
        {
            _animator = GetComponent<PlayerAnimator>();
        }

        private void Start()
        {
            _input = InputHandler.Instance.Input;
            _animator.Event += AnimatorAction;
        }

        private void Update()
        {
            if (UIManager.Instance.Active || InterstitialsTimer.Instance.Active) return;
            if (_input.GetAction() || BoostersHandler.Instance.UseAutoClicker) DoAction();
        }

        private void DoAction()
        {
            if (_isOnCooldown) return;

            var rand = UnityEngine.Random.Range(0, actionAnimations.Count);

            _isOnCooldown = true;
            _animator.DoAction(3, rand);
        }

        private void AnimatorAction(string id)
        {
            switch (id)
            {
                case "Action":
                    SoundManager.Instance.PlayOneShot("action");
            
                    DoIncome();
                    break;
                
                case "ActionEnd":
                    _isOnCooldown = false;
                    break;
            }

        }
        
        private void DoIncome()
        {
            var obj = EffectPanel.Instance.SpawnEffect(effect);
            var income = Player.Instance.TrainingStrength();
            obj.Initialize(CurrencyType.First, income);
            WalletManager.FirstCurrency += income;
        }
    }
}