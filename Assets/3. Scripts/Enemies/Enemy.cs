using System;
using _3._Scripts.Interactive;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.Localization;
using _3._Scripts.Player;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts.Enemies
{
    public class Enemy : MonoBehaviour, IInteractive
    {
        [SerializeField] private EnemyData data;
        [SerializeField] private BattleArena arena;
        [SerializeField] private LocalizeStringEvent text;

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
            
            text.SetVariable("value", data.Strength.ToString());
        }

        public void TeleportToStart()
        {
            transform.position = _startPosition;
            transform.eulerAngles = _startRotation;
            text.gameObject.SetActive(true);
        }

        public void Interact()
        {
            arena.StartBattle(this);
            text.gameObject.SetActive(false);
        }

     

        public void StopInteract()
        {
        }
    }
}