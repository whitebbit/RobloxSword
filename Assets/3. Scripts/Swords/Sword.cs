using _3._Scripts.Swords.Scriptable;
using UnityEngine;

namespace _3._Scripts.Swords
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private SwordData data;

        public SwordData Data => data;
    }
}