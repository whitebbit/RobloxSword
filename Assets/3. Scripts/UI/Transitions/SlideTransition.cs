using System;
using _3._Scripts.UI.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace _3._Scripts.UI.Transitions
{
    [Serializable]
    public class SlideTransition : IUITransition
    {
        [SerializeField] private RectTransform transform;
        [Space] [SerializeField] private float duration;
        [SerializeField] private Vector2 direction;

        private Vector2 startPosition;

        public void SetStartPosition()
        {
            startPosition = transform.anchoredPosition;
        }
        public Tween AnimateIn()
        {
            return transform.DOAnchorPos(startPosition, duration).SetLink(transform.gameObject);
        }

        public Tween AnimateOut()
        {
            return transform.DOAnchorPos(startPosition - direction, duration).SetLink(transform.gameObject);
        }

        public void ForceIn()
        {
            transform.anchoredPosition = startPosition;
        }

        public void ForceOut()
        {
            transform.anchoredPosition = startPosition - direction;
        }
    }
}