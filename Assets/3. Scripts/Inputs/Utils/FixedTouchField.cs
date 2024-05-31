using UnityEngine;
using UnityEngine.EventSystems;

namespace _3._Scripts.Inputs.Utils
{
    public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private int _pointerId = -1;
        private Vector2 _pointerOld;
        public Vector2 Axis { get; private set; }
        public bool Pressed { get; private set; }

        private void Update()
        {
            if (Pressed)
            {
                if (_pointerId >= 0 && _pointerId < Input.touches.Length)
                {
                    Axis = Input.touches[_pointerId].position - _pointerOld;
                    _pointerOld = Input.touches[_pointerId].position;
                }
                else
                {
                    Axis = Vector2.zero;
                }
            }
            else
            {
                Axis = Vector2.zero;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_pointerId != -1) return;
        
            Pressed = true;
            _pointerId = eventData.pointerId;
            _pointerOld = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId != _pointerId) return;
        
            Pressed = false;
            _pointerId = -1;
        }
    }
}