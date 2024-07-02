using _3._Scripts.Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _3._Scripts.Inputs.Utils
{
    public class FixedTouchField : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RemoteConfig<float> minMovementThreshold;
        private int _pointerId;
        private Vector2 _startTouchPosition;
        private Vector2 _currentTouchPosition;
        private Vector2 _previousTouchPosition;

        public Vector2 Axis { get; private set; }

        public bool Pressed { get; private set; }

        void Update()
        {
            if (Pressed)
            {
                var distance = Vector2.Distance(_currentTouchPosition, _previousTouchPosition);
            
                if (distance >= 0.75)
                {
                    Axis = _currentTouchPosition - _startTouchPosition;
                }
                else
                {
                    Axis = Vector2.zero;
                    _startTouchPosition = _currentTouchPosition;
                }

                _previousTouchPosition = _currentTouchPosition;
            }
            else
            {
                Axis = Vector2.zero;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Pressed) return;
            Pressed = true;
            _pointerId = eventData.pointerId;
            _startTouchPosition = eventData.position;
            _currentTouchPosition = _startTouchPosition;
            _previousTouchPosition = _startTouchPosition;
            Axis = Vector2.zero;  
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Pressed && eventData.pointerId == _pointerId)
            {
                _currentTouchPosition = eventData.position;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!Pressed || eventData.pointerId != _pointerId) return;
            Pressed = false;
            Axis = Vector2.zero;
        }
    }
}