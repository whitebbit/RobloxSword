using System;
using System.Collections;
using _3._Scripts.FSM.Base;
using UnityEngine;
using VInspector;
using Random = UnityEngine.Random;

namespace _3._Scripts.Bots
{
    public class Bot : MonoBehaviour
    {
        [SerializeField] private UnitNavMeshAgent navMesh;
        [Tab("States")] [SerializeField] private RunState runState;
        [SerializeField] private TrainingState trainingState;

        private FSMHandler _fsmHandler;
        private IdleState _idleState;

        private bool _running;
        private bool _training;

        private void Awake()
        {
            _fsmHandler = new FSMHandler();
            _idleState = new IdleState();
            
            _idleState.SetNavMeshAgent(navMesh);
            runState.SetNavMeshAgent(navMesh);
            trainingState.SetNavMeshAgent(navMesh);

            _fsmHandler.AddTransition(_idleState, new FuncPredicate(() => !_running && !_training));
            _fsmHandler.AddTransition(runState, new FuncPredicate(() => _running && !_training));
            _fsmHandler.AddTransition(trainingState, new FuncPredicate(() => !_running && _training));

            _fsmHandler.StateMachine.SetState(_idleState);
        }

        private void Start()
        {
            StartCoroutine(ChangeState());
        }

        private void Update()
        {
            _fsmHandler.StateMachine.Update();
        }

        private IEnumerator ChangeState()
        {
            var time = 0;
            while (true)
            {
                var rand = Random.Range(0, 3);
                switch (rand)
                {
                    case 0:
                        _running = false;
                        _training = false;
                        time = Random.Range(2, 3);
                        break;
                    
                    case 1:
                        _running = true;
                        _training = false;
                        time = Random.Range(5, 10);
                        break;

                    case 2:
                        _running = false;
                        _training = true;
                        time = Random.Range(5, 10);
                        break;
                }
                yield return new WaitForSeconds(time);
            }
        }
    }
}