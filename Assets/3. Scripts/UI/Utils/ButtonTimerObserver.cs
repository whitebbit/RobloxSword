using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Utils
{
    public class ButtonTimerObserver : MonoBehaviour
    {
        [SerializeField] private float minimumCheckTime = 60;

        [SerializeField] private List<Timer> timers = new();
        [SerializeField] private Image notificationImage;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            notificationImage.gameObject.SetActive(false);
            _button.onClick.AddListener(() => notificationImage.gameObject.SetActive(false));
            StartCoroutine(CheckTimerState());
        }

        private IEnumerator CheckTimerState()
        {
            while (true)
            {
                yield return new WaitForSeconds(minimumCheckTime + 1);
                if (!timers.Any(t => t.TimerEnd() && !t.TimerStopped)) continue;
                NotificationAnimation();
            }
        }

        private void NotificationAnimation()
        {
            var rect = (RectTransform) notificationImage.transform;
            var startSize = rect.sizeDelta;

            notificationImage.gameObject.SetActive(true);
            rect.DOSizeDelta(startSize * 1.25f, 0.25f).OnComplete(() => { rect.DOSizeDelta(startSize, 0.1f); })
                .SetEase(Ease.InOutBounce).SetLink(notificationImage.gameObject);
        }
    }
}