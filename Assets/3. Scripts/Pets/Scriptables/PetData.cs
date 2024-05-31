using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Enums;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Pets.Scriptables
{
    [CreateAssetMenu(fileName = "PetData", menuName = "PetData", order = 0)]
    public class PetData : ScriptableObject
    {
        [Tab("Main")] [SerializeField] private string id;
        [SerializeField] private Pet prefab;
        [Tab("Booster")] [SerializeField] private CurrencyType currencyType;
        [SerializeField] private float booster;
        [Tab("UI")] [SerializeField] private Sprite icon;
        [SerializeField] private Rarity rarity;

        public Sprite Icon => icon;
        public Rarity Rarity => rarity;
        public Pet Prefab => prefab;
        public CurrencyType CurrencyType => currencyType;
        public float Booster => booster;
        public string ID => id;

        public static void Activate()
        {
        }

        public static void Deactivate()
        {
        }
    }
}