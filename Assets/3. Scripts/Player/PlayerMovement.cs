using System;
using _3._Scripts.Boosters;
using _3._Scripts.Inputs;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Sounds;
using _3._Scripts.UI;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

namespace _3._Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private const float Gravity = -9.81f;
        private const float TurnSmoothTime = 0.1f;

        [Tab("Setting")] [SerializeField] private float speed;
        [SerializeField] private float jumpHeight;
        [Tab("Components")] [SerializeField] private CinemachineFreeLook freeLookCamera;
        [Tab("Ground")] [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance;
        [SerializeField] private LayerMask groundMask;

        private CharacterController _characterController;
        private Transform _camera;
        private IInput _input;
        private Vector3 _velocity;
        private float _turnSmoothVelocity;
        private PlayerAnimator _animator;
        public float SpeedMultiplier { get; set; } = 1;

        private void Awake()
        {
            _animator = GetComponent<PlayerAnimator>();
            _characterController = GetComponent<CharacterController>();
            if (Camera.main is not null) _camera = Camera.main.transform;
        }

        private void Start()
        {
            _input = InputHandler.Instance.Input;
        }

        private void Update()
        {
            ResetVelocity();
            if (UIManager.Instance.Active)
            {
                _animator.SetSpeed(0);
                _input.CursorState();
                SetCameraInputAxis(Vector2.zero);
                _animator.SetGrounded(true);
                return;
            }

            Move();
            Jump();
            Fall();
            Look();
        }

        private void Move()
        {
            var direction = _input.GetMovementAxis();
            _animator.SetSpeed(direction.magnitude);

            if (!(direction.magnitude >= 0.1f)) return;

            var targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                TurnSmoothTime);
            var moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            transform.rotation = Quaternion.Euler(0, angle, 0);
            _characterController.Move(moveDirection * speed * SpeedMultiplier * Time.deltaTime);
            PlayFootstepSound();
        }

        private void Look()
        {
            _input.CursorState();

            if (!_input.CanLook())
            {
                SetCameraInputAxis();
                return;
            }

            SetCameraInputAxis(_input.GetLookAxis());
        }

        private void Jump()
        {
            if (_input.GetJump() && IsGrounded())
            {
                EventSystem.current.SetSelectedGameObject(null);
                _velocity.y = Mathf.Sqrt(jumpHeight * -2 * Gravity);
                _animator.DoJump();
                SoundManager.Instance.PlayOneShot("jump");
            }

            _animator.SetGrounded(IsGrounded());
        }

        private void Fall()
        {
            _velocity.y += Gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void SetCameraInputAxis(Vector2 direction = default)
        {
            freeLookCamera.m_XAxis.m_InputAxisValue = direction.x;
            freeLookCamera.m_YAxis.m_InputAxisValue = direction.y;
        }

        private void ResetVelocity()
        {
            if (IsGrounded() && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
        }

        private bool IsGrounded()
        {
            return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }

        private float _footstepTimer;
        private float FootstepInterval => 0.4f;

        private void PlayFootstepSound()
        {
            if (!IsGrounded()) return;
            _footstepTimer -= Time.deltaTime;
            if (!(_footstepTimer <= 0f)) return;
            SoundManager.Instance.PlayOneShot("step");
            _footstepTimer = FootstepInterval;
        }
    }
}