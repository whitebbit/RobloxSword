using System;
using _3._Scripts.Swords;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3._Scripts.Characters
{
    public class Character : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField] private SwordHandler swordHandler;

        private void OnValidate()
        {
            swordHandler = GetComponentInChildren<SwordHandler>();
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            Player.Player.Instance.Animator.SetAvatar(_animator.avatar);
            Player.Player.Instance.SetSwordPoint(swordHandler);

        }
        
    }
}