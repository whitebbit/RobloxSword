using System;
using UnityEngine;

namespace _3._Scripts.Sounds
{
    [Serializable]
    public class SoundItem
    {
        [SerializeField] private string id;
        [SerializeField] private AudioClip sound;

        public string ID => id;

        public AudioClip Sound => sound;
    }
}