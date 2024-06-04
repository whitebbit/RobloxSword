using System;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.UI.Enums;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Scriptable.Shop
{
    public abstract class ShopItem : ScriptableObject
    {
        [SerializeField] private string id;
        [Tab("UI")] [SerializeField] private Sprite icon;
        [SerializeField] private Rarity rarity;
        [Tab("Currency")] [SerializeField] private CurrencyType currencyType;
        [SerializeField] private float price;


        public abstract string Title();

        public string ID => id;
        public Sprite Icon => icon;
        public Rarity Rarity => rarity;

        public CurrencyType CurrencyType => currencyType;
        public float Price => price * RemoteConfiguration.ShopPriceMultiplier;
    }
}