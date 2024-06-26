using System;
using _3._Scripts.Detectors;
using _3._Scripts.Inputs;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.UI;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class PlayerInteraction: MonoBehaviour
    {
        [SerializeField] private BaseDetector<IInteractive> detector;
        private IInteractive _interactive;
        private IInput _input;

        private void Start()
        {
            detector.OnFound += SetInteractive;
            _input = InputHandler.Instance.Input;
        }
        
        private void Update()
        {
            if (_interactive != null && _input.GetInteract() && !UIManager.Instance.Active) // ПКМ
            {
                _interactive.Interact();
            }
        }

        private void SetInteractive(IInteractive newInteractive)
        {
            if (_interactive == newInteractive) return;

            _interactive?.StopInteract();

            _interactive = newInteractive;

            _interactive?.StartInteract();
        }
        
    }
}