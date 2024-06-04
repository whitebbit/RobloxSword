using System;
using _3._Scripts.Config;
using UnityEngine;

namespace _3._Scripts.Enemies
{
    [Serializable]
    public class EnemyData
    {
        [field:Header("Properties")]
        [field:SerializeField] public float Strength { get; private set; }
        [field:SerializeField] public float Cups { get; private set; }
        [field:Header("UI")]
        [field:SerializeField] public Sprite Icon { get; private set; }
        [field:SerializeField] public string LocalizeID { get; private set; }

        public float CurrentStrength => Strength * RemoteConfiguration.EnemyStrength;
    }
}