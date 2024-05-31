#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace GBGamesPlugin
{
    public class CallingAnEvent : MonoBehaviour
    {
        public IEnumerator CallingAd(float duration)
        {
            DrawScreen(new Color(0, 1, 0, 0.5f));
            yield return new WaitForSecondsRealtime(duration);
            Destroy(gameObject);
        }
        
        private void DrawScreen(Color color)
        {
            var obj = gameObject;
            var canvas = obj.AddComponent<Canvas>();
            canvas.sortingOrder = 32767;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            obj.AddComponent<GraphicRaycaster>();
            obj.AddComponent<RawImage>().color = color;
        }
    }
}
#endif