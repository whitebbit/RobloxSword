using System;
using System.Collections.Generic;
using _3._Scripts.Ads;
using _3._Scripts.Boosters;
using _3._Scripts.FSM.Base;
using _3._Scripts.Inputs;
using _3._Scripts.Player;
using _3._Scripts.Sounds;
using _3._Scripts.UI;
using UnityEngine;

namespace _3._Scripts.Bots
{
    [Serializable]
    public class TrainingState: State
    {
        [SerializeField] private List<AnimationClip> actionAnimations = new();
        [SerializeField] private PlayerAnimator animator;
        
        private UnitNavMeshAgent _navMesh;
        private bool _isOnCooldown;

        public void SetNavMeshAgent(UnitNavMeshAgent navMesh)
        {
            _navMesh = navMesh;
        } 

        public override void OnEnter()
        {
            base.OnEnter();
            _isOnCooldown = false;
            animator.Event += AnimatorAction;
            _navMesh.StopMoving();
        }

        public override void Update()
        {
            base.Update();
            DoAction();
        }

        private void DoAction()
        {
            if (_isOnCooldown) return;

            var rand = UnityEngine.Random.Range(0, actionAnimations.Count);

            _isOnCooldown = true;
            animator.DoAction(3, rand);
        }

        private void AnimatorAction(string id)
        {
            _isOnCooldown = id switch
            {
                "ActionEnd" => false,
                _ => _isOnCooldown
            };
        }

        public override void OnExit()
        {
            base.OnExit();
            _isOnCooldown = false;
            animator.Event -= AnimatorAction;
        }
    }
}