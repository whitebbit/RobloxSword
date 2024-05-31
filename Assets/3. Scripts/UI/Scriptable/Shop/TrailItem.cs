using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Scriptable.Shop
{
    [CreateAssetMenu(fileName = "TrailItem", menuName = "Shop Items/Trail Item", order = 0)]
    public class TrailItem : ShopItem
    {
        [Tab("Trail")] [SerializeField]
        private Color color;

        [SerializeField] private float boost;

        
        
        public Color Color => color;
        public float Boost => boost;
        public override string Title()
        {
            return $"{boost * 100}%";
        }
    }
}