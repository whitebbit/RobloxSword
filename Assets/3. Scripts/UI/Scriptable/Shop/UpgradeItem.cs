using _3._Scripts.Characters;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Scriptable.Shop
{
    [CreateAssetMenu(fileName = "UpgradeItem", menuName = "Shop Items/Upgrade Item", order = 0)]
    public class UpgradeItem : ShopItem
    {
        [SerializeField] private float booster;
        [Tab("Prefab")] [SerializeField] private GameObject prefab;

        public float Booster => booster;

        public GameObject Prefab => prefab;

        public override string Title()
        {
            return $"{(1 - booster + 1) * 100}%";
        }
    }
}