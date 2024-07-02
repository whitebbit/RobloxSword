using _3._Scripts.UI.Scriptable.Roulette;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Achievements.Scriptables
{
    [CreateAssetMenu(fileName = "Achievement", menuName = "ScriptableObjects/Achievement", order = 1)]
    public class AchievementData : ScriptableObject
    {    
        [Tab("Main")]
        [SerializeField] private string id;
        [SerializeField] private int goal;
        [SerializeField] private GiftItem gift;
        
        [Tab("UI")]
        [SerializeField] private string titleID;
        [SerializeField] private Sprite icon;

        public string ID => id;
        public float Goal => goal;
        public string TitleID => titleID;
        public Sprite Icon => icon;

        public GiftItem Gift => gift;
    }
}