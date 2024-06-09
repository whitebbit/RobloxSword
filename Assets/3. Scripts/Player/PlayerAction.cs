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
        [SerializeField] private float baseCooldownTime;
        [SerializeField] private List<AnimationClip> actionAnimations = new();
        [Tab("Detectors")] [SerializeField] private BaseDetector<IInteractive> detector;
        [Tab("UI")] [SerializeField] private CurrencyCounterEffect effect;


        private IInput _input;
        private bool _isOnCooldown;
        private float _cooldownTimer;
        private PlayerAnimator _animator;

        private IInteractive _interactive;

        private void Awake()
        {
            _animator = GetComponent<PlayerAnimator>();
        }

        private void Start()
        {
            _input = InputHandler.Instance.Input;
            detector.OnFound += TryGetInteractive;
            _animator.Event += AnimatorAction;
        }

        private void Update()
        {
            if (UIManager.Instance.Active || InterstitialsTimer.Instance.Active) return;
            //if(!_animator.GetBool("IsGrounded")) return;

            if (_input.GetAction() || BoostersHandler.Instance.UseAutoClicker) DoAction();
            Cooldown();
        }

        private void DoAction()
        {
            if (_isOnCooldown) return;

            var rand = UnityEngine.Random.Range(0, actionAnimations.Count);
            var randClip = actionAnimations[rand];

            _isOnCooldown = true;
            _cooldownTimer = GetCooldown();
            _animator.DoAction(GetActionSpeed(randClip), rand);
        }

        private void AnimatorAction(string id)
        {
            if (id != "Action") return;
            SoundManager.Instance.PlayOneShot("action");
                
            if (_interactive == null)
                DoIncome();
            else
                _interactive?.Interact();
        }

        private void TryGetInteractive(IInteractive interactive)
        {
            if (_interactive == interactive) return;

            _interactive?.StopInteract();

            _interactive = interactive;

            if (_interactive != null) return;

            _interactive?.StopInteract();
            _interactive = null;
        }

        private void Cooldown()
        {
            if (!_isOnCooldown) return;
            _cooldownTimer -= Time.deltaTime;
            if (!(_cooldownTimer <= 0f)) return;
            _isOnCooldown = false;
        }

        private float GetActionSpeed(AnimationClip clip)
        {
            return clip.length / GetCooldown();
        }

        private float GetCooldown()
        {
            /*var first = Configuration.Instance.AllUpgrades.FirstOrDefault(u =>
                GBGames.saves.upgradeSaves.IsCurrent(u.ID));*/

            // var booster = first == null ? 1 : first.Booster;
            return Mathf.Clamp(baseCooldownTime * 1, 0.25f, 10);
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