using System;
using System.Collections;
using _3._Scripts.Config;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Boosters
{
    [RequireComponent(typeof(Button))]
    public class BoosterButton : MonoBehaviour
    {
        [Tab("View")] [SerializeField] private Image cooldownImage;

        [SerializeField] private Image adImage;


        public Action onActivateBooster;
        public Action onDeactivateBooster;
        private Button _button;
        private bool _used;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(OnCLick);
        }

        private void OnCLick()
        {
            if (_used) return;
            GBGames.ShowRewarded(Activate);
        }

        private void Activate()
        {
            _used = true;
            onActivateBooster?.Invoke();
            cooldownImage.fillAmount = 1;
            cooldownImage.DOFillAmount(0, RemoteConfiguration.BoostTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                onDeactivateBooster?.Invoke();
                adImage.gameObject.SetActive(true);
                cooldownImage.fillAmount = 0;
                _used = false;
            });
            adImage.gameObject.SetActive(false); 
            
        }
    }
}