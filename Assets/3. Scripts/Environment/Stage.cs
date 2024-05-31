using _3._Scripts.Swords.Scriptable;
using UnityEngine;

namespace _3._Scripts.Environment
{
    public class Stage: MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        public Transform SpawnPoint => spawnPoint;
                
    }
}