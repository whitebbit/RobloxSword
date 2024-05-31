using System;
using _3._Scripts.Player;
using UnityEngine;

namespace _3._Scripts.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyData data;

        public EnemyData Data => data;
        public PlayerAnimator Animator => _animator;
        
        private PlayerAnimator _animator;
        private Vector3 _startPosition;
        private Vector3 _startRotation;


        private void Awake()
        {
            _animator = GetComponent<PlayerAnimator>();
        }

        private void Start()
        {
            _startPosition = transform.position;
            _startRotation = transform.eulerAngles;
            _animator.SetGrounded(true);
            _animator.SetSpeed(0);
        }

        public void TeleportToStart()
        {
            transform.position = _startPosition;
            transform.eulerAngles = _startRotation;
        }
        
    }
}