using _3._Scripts.Singleton;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _3._Scripts.UI.Panels
{
    public class EffectPanel : Singleton<EffectPanel>
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public T SpawnEffect<T>(T prefab) where T : UIEffect
        {
            var obj = Instantiate(prefab, transform);
            if (obj.transform is not RectTransform objTransform) return obj;

            objTransform.anchoredPosition = GetRandomPosition();
            objTransform.anchorMin = new Vector2(0, 0);
            objTransform.anchorMax = new Vector2(0, 0);

            return obj;
        }

        private Vector2 GetRandomPosition()
        {
            var x = Random.Range(0, _rectTransform.rect.width);
            var y = Random.Range(0, _rectTransform.rect.height);
            return new Vector2(x, y);
        }
    }
}