using _3._Scripts.Config;
using _3._Scripts.Enemies;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

namespace _3._Scripts.Swords.Scriptable
{
    [CreateAssetMenu(fileName = "SwordData", menuName = "Sword Data", order = 0)]
    public class SwordData : ScriptableObject
    {
        [Tab("Main")]
        [SerializeField] private string id;
        [SerializeField] public float strengthBooster;
        [SerializeField] private ComplexityType type;
        [Space]
        [SerializeField] private EnemyData enemyData;
        [Space]
        [SerializeField] private Sword sword;

        [Tab("Transform")]
        [SerializeField] private SerializableTransform getState;
        [SerializeField] private SerializableTransform handState;

        public SerializableTransform GetState => getState;
        public SerializableTransform HandState => handState;

        public Sword Sword => sword;

        public string ID => id;

        public float StrengthBooster => strengthBooster + RemoteConfiguration.SwordAdditionalBooster;
        public EnemyData EnemyData => enemyData;

        public ComplexityType Type => type;
    }
}