using System;
using _3._Scripts.Localization;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts
{
    
    [RequireComponent(typeof(LocalizeStringEvent))]
    public class ComplexityText: MonoBehaviour
    {
        
        private LocalizeStringEvent _event;

        private void Awake()
        {
            _event = GetComponent<LocalizeStringEvent>();
        }

        public void SetComplexity(ComplexityType type)
        {
            _event.TextToComplexity(type);
        }
        
        
    }

    public static class ComplexityExtensions
    {
        public static void TextToComplexity(this LocalizeStringEvent @event, ComplexityType type)
        {
            switch (type)
            {
                case ComplexityType.Starter:
                    @event.SetReference("starter");
                    break;
                case ComplexityType.Average:
                    @event.SetReference("average");

                    break;
                case ComplexityType.Hard:
                    @event.SetReference("hard");

                    break;
                case ComplexityType.Extreme:
                    @event.SetReference("extreme");

                    break;
                case ComplexityType.FinalBoss:
                    @event.SetReference("final_boss");

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
    
    public enum ComplexityType
    {
        Starter,
        Average,
        Hard,
        Extreme,
        FinalBoss
    }
}