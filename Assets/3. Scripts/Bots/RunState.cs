using System;
using _3._Scripts.FSM.Base;
using _3._Scripts.Player;
using UnityEngine;

namespace _3._Scripts.Bots
{
    [Serializable]
    public class RunState : State
    {
        [SerializeField] private float speed;
        [SerializeField] private float minRange;
        [SerializeField] private float maxRange;
        
        private UnitNavMeshAgent _navMesh;


        public void SetNavMeshAgent(UnitNavMeshAgent navMesh)
        {
            _navMesh = navMesh;
        } 
        public override void OnEnter()
        {
            base.OnEnter();
            _navMesh.Agent.speed = speed;
            _navMesh.StartMoving(_navMesh.RandomPointOnNavMesh(minRange, maxRange));
        }

        public override void Update()
        {
            OnPointChecking();
        }

        private void OnPointChecking()
        {
            if (!_navMesh.OnPoint()) return;
            _navMesh.StartMoving(_navMesh.RandomPointOnNavMesh(minRange, maxRange));
        }

        public override void OnExit()
        {
            base.OnExit();
            _navMesh.StopMoving();
            _navMesh.ResetOnStopMoving();
        }
    }
}