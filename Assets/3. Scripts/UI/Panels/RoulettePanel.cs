using System.Collections;
using System.Collections.Generic;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Extensions;
using _3._Scripts.UI.Panels.Base;
using _3._Scripts.UI.Scriptable.Roulette;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public class RoulettePanel : SimplePanel
    {
        [Tab("Roulette")] [SerializeField] private RectTransform roulette;
        [SerializeField] private RectTransform detectObject;
        [SerializeField] private List<RouletteSlot> slots = new();
        [Tab("Rewards")] [SerializeField] private List<GiftItem> items = new();
        [Tab("Buttons")] 
        [SerializeField] private Button spinButton;
        [SerializeField] private Button backButton;
        
        [Tab("Timer")] [SerializeField] private float timerDuration = 60;


        [SerializeField] private List<Timer> timers = new();
        [Tab("Effects")] [SerializeField] private Ease ease;

        private RouletteSlot _currentReward;

        private bool _rotating;
        private bool _rewardGot;

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
            spinButton.onClick.AddListener(Rotate);
            foreach (var timer in timers)
            {
                timer.StartTimer(timerDuration);
                timer.OnTimerEnd += () => spinButton.interactable = timer.TimerEnd();
                timer.OnTimerStart += () => spinButton.interactable = false;
            }
        }

        protected override void OnOpen()
        {
            AddRandomItems();
            spinButton.interactable = timers[0].TimerEnd();
        }

        private void Rotate()
        {
            if (_rotating) return;

            var rand = Random.Range(0, slots.Count);
            var z = -45 * rand;
            var vector = new Vector3(0, 0, z - 720 - 720 - 720);
            _rotating = true;
            backButton.interactable = !_rotating;
            roulette.DORotate(vector, 2.5f, RotateMode.FastBeyond360)
                .SetRelative(true)
                .SetEase(ease)
                .SetDelay(0.1f)
                .SetLink(gameObject)
                .OnComplete(() =>
                {
                    _rotating = false;
                    backButton.interactable = !_rotating;
                    GetReward();
                });
            
            foreach (var timer in timers) timer.StartTimer(timerDuration);
        }

        private void GetReward()
        {
            _currentReward = UIRaycast.FindObject<RouletteSlot>(detectObject.position);
            _currentReward.GetReward();
        }

        private void AddRandomItems()
        {
            foreach (var slot in slots)
            {
                var rand = Random.Range(0, items.Count);
                slot.Initialize(items[rand]);
            }
        }
    }
}