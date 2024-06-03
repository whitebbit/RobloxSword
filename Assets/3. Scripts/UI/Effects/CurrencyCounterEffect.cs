using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Wallet;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Effects
{
    public class CurrencyCounterEffect : UIEffect
    {
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private TMP_Text counter;
        [SerializeField] private Image icon;


        public void Initialize(CurrencyType type, float count)
        {
            var image = Configuration.Instance.GetCurrency(type).Icon;
            var rect = transform as RectTransform;
            icon.sprite = image;
            counter.text = $"+{WalletManager.ConvertToWallet((decimal) count)}";

            canvasGroup.alpha = 0;
            if (rect is not null)
            {
                var rand = Random.Range(0, 2) == 0 ? Vector2.right : -Vector2.right;
                var size = rect.sizeDelta.y;
                var target = Vector2.up * size * 10 + rand * size * 2.5f;
                rect.DOAnchorPos(target, 1.5f)
                    .SetLink(gameObject)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => Destroy(gameObject));
            }
            
            canvasGroup.DOFade(1, 0.25f).SetLink(gameObject);
        }
    }
}