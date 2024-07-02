using System;
using _3._Scripts.FSM.Interfaces;
using UnityEngine;

namespace _3._Scripts.FSM.Base
{
    public abstract class State: IState
    {
        public event Action OnEnterAction; 
        public event Action OnExitAction; 
        public virtual void OnEnter()
        {
            //Debug.Log($"{GetType()} - OnEnter");
            OnEnterAction?.Invoke();
        }

        public virtual void Update()
        {
           
        }

        public virtual void FixedUpdate()
        {
           
        }

        public virtual void OnExit()
        {
            //Debug.Log($"{GetType()} - OnExit");
            OnExitAction?.Invoke();
        }
    }
}