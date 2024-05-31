using _3._Scripts.Characters;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Scriptable.Shop
{
    [CreateAssetMenu(fileName = "CharacterShopItem", menuName = "Shop Items/Character Item", order = 0)]
    public class CharacterItem : ShopItem
    {
        [SerializeField] private float booster;
        [Tab("Prefab")] 
        [SerializeField] private Character prefab;

        public Character Prefab => prefab;

        public float Booster => booster;
        public override string Title()
        {
            return $"+{booster}";
        }
    }
}