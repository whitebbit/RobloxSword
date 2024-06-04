using System;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Action = Animator.StringToHash("Action");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int ActionSpeed = Animator.StringToHash("ActionSpeed");
        private static readonly int ActionID = Animator.StringToHash("ActionID");


        public event Action<string> Event;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetState(bool state) => _animator.enabled = state;

        public void SetBool(string id, bool state)
        {
            if (_animator == null) return;
            _animator.SetBool(Animator.StringToHash(id), state);
        }

        public bool GetBool(string id)
        {
            return _animator != null && _animator.GetBool(Animator.StringToHash(id));
        }

        public void SetAvatar(Avatar avatar) => _animator.avatar = avatar;

        public void SetSpeed(float speed)
        {
            if (_animator == null) return;
            _animator.SetFloat(Speed, speed);
        }

        public void DoAction(float actionSpeed, int actionID)
        {
            if (_animator == null) return;

            _animator.SetFloat(ActionSpeed, actionSpeed);
            _animator.SetFloat(ActionID, actionID);
            _animator.SetTrigger(Action);
        }

        public void DoJump()
        {
            if (_animator == null) return;

            _animator.SetTrigger(Jump);
            SetGrounded(false);
        }

        public void SetGrounded(bool grounded)
        {
            if (_animator == null) return;

            _animator.SetBool(IsGrounded, grounded);
        }

        public void SetTrigger(string id)
        {
            if (_animator == null) return;
            _animator.SetTrigger(Animator.StringToHash(id));
        }

        public void AnimationEvent(string id) => Event?.Invoke(id);
    }
}