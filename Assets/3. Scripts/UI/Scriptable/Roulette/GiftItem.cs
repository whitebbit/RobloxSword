using _3._Scripts.UI.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

namespace _3._Scripts.UI.Scriptable.Roulette
{
    public abstract class GiftItem: ScriptableObject
    {
        public abstract Sprite Icon();
        public abstract string Title();
        
        public abstract void OnReward();
    }
}