﻿using _3._Scripts.Swords.Scriptable;
using UnityEngine;

namespace _3._Scripts.Environment
{
    public class Stage: MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float petsBoosterMultiplier = 1;
        [SerializeField] private float giftsBoosterMultiplier = 1;
        
        public Transform SpawnPoint => spawnPoint;

        public float PetsBoosterMultiplier => petsBoosterMultiplier;

        public float GiftsBoosterMultiplier => giftsBoosterMultiplier;
    }
}