using System;
using _3._Scripts.FSM.Base;
using UnityEngine;

namespace _3._Scripts.Bots
{
    public class IdleState: State
    {
        private UnitNavMeshAgent _navMesh;
        
        public void SetNavMeshAgent(UnitNavMeshAgent navMesh)
        {
            _navMesh = navMesh;
        } 

        public override void OnEnter()
        {
            base.OnEnter();
            _navMesh.StopMoving();
        }

        
    }
}