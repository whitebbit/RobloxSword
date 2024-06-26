using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

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
                    var touch = Input.touches[_pointerId];
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        Axis = touch.position - _pointerOld;
                        _pointerOld = touch.position;
                    }
                    else
                    {
                        Axis = Vector2.zero;
                    }
                }
                else
                {
                    Axis = (Vector2) Input.mousePosition - _pointerOld;
                    _pointerOld = Input.mousePosition;
                }
            }
            else
            {
                Axis = Vector2.zero;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Pressed = true;
            _pointerId = eventData.pointerId;
            _pointerOld = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Pressed = false;
            _pointerId = -1;
        }
    }
}