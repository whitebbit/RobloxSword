using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Inputs.Utils;
using _3._Scripts.Singleton;
using UnityEngine;

namespace _3._Scripts.Inputs
{
    public class MobileInput: MonoBehaviour, IInput
    {
        [SerializeField] private Joystick joystick;
        [SerializeField] private FixedTouchField touchField;
        [SerializeField] private FixedButton jumpButton;
        [SerializeField] private FixedButton actionButton;
        
        public Vector2 GetMovementAxis()
        {
            return joystick.Direction.normalized;
        }

        public Vector2 GetLookAxis()
        {
            return touchField.Axis.normalized;
        }

        public bool GetAction()
        {
            return actionButton.ButtonDown;
        }

        public bool GetJump()
        {
            return jumpButton.ButtonDown;
        }

        public bool CanLook()
        {
            return touchField.Pressed;
        }

        public void CursorState()
        {
            
        }
    }
}