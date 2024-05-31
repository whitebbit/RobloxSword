using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _3._Scripts.Sounds.Scriptable
{
    [CreateAssetMenu(fileName = "SoundDatabase", menuName = "Sound Database", order = 0)]
    public class SoundDatabase : ScriptableObject
    {
        [SerializeField] private List<SoundItem> items = new();

        public AudioClip GetSound(string id)
        {
            return items.FirstOrDefault(i=> i.ID == id)?.Sound;
        }
    }
}