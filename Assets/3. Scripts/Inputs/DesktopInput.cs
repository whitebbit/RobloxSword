using _3._Scripts.Inputs.Interfaces;
using UnityEngine;

namespace _3._Scripts.Inputs
{
    public class DesktopInput : IInput
    {
        public Vector2 GetMovementAxis()
        {
            return new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }
        
        public Vector2 GetLookAxis()
        {
            return new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")).normalized;
        }
        
        public bool GetAction()
        {
            return Input.GetMouseButtonDown(0);
        }

        public bool GetJump()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        public bool CanLook()
        {
            return Input.GetMouseButton(1);
        }

        public void CursorState()
        {
            Cursor.visible = !CanLook();
            Cursor.lockState = CanLook() ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}