using UnityEngine;

namespace _3._Scripts.Inputs.Interfaces
{
    public interface IInput
    {
        public Vector2 GetMovementAxis();
        public Vector2 GetLookAxis();

        public bool GetAction();
        public bool GetJump();
        public bool GetInteract();

        public bool CanLook();

        public void CursorState();
    }
}
