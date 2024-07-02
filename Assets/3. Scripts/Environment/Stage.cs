using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Bots;
using _3._Scripts.Swords.Scriptable;
using UnityEngine;
using Random = System.Random;

namespace _3._Scripts.Environment
{
    public class Stage : MonoBehaviour
    {
        [SerializeField] private int id;

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float petsBoosterMultiplier = 1;
        [SerializeField] private float giftsBoosterMultiplier = 1;


        [SerializeField] private List<Bot> bots = new();
        private readonly List<Bot> _currentBots = new();
        public Transform SpawnPoint => spawnPoint;

        public int ID => id;
        public float PetsBoosterMultiplier => petsBoosterMultiplier;

        public float GiftsBoosterMultiplier => giftsBoosterMultiplier;

        private void OnEnable()
        {
            foreach (var obj in bots.Select(bot => Instantiate(bot, transform)))
            {
                obj.transform.position += Vector3.left * UnityEngine.Random.Range(-7.5f, 7.5f) +
                                          Vector3.forward * UnityEngine.Random.Range(-7.5f, 7.5f);
                _currentBots.Add(obj);
            }
        }

        private void OnDisable()
        {
            foreach (var bot in _currentBots)
            {
                Destroy(bot.gameObject);
            }

            _currentBots.Clear();
        }
    }
}