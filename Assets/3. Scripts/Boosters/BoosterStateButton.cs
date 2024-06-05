using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.Boosters
{
    public class BoosterStateButton: MonoBehaviour
    {
        [Tab("View")] 
        [SerializeField] private Image activeImage;

        public Action onActivateBooster;
        public Action onDeactivateBooster;
        private Button _button;
        private bool _state;
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            Deactivate();
            _button.onClick.AddListener(OnCLick);
        }
        
        private void OnCLick()
        {
            if (_state) Deactivate();
            else Activate();
        }

        private void Deactivate()
        {
            _state = false;
            onDeactivateBooster?.Invoke();
            activeImage.DOFade(0, 0.15f).OnComplete(() =>
            {
                activeImage.gameObject.SetActive(false);
            });
        }
        private void Activate()
        {
            _state = true;
            onActivateBooster?.Invoke();
            activeImage.gameObject.SetActive(true);
            activeImage.DOFade(1, 0.15f);
        }

    }
}