using System;
using _3._Scripts.UI.Enums;
using UnityEngine;

namespace _3._Scripts.UI.Structs
{
    [Serializable]
    public struct RarityTable
    {
        [SerializeField] private Rarity rarity;
        [SerializeField] private Sprite table;
        [SerializeField] private Color mainColor;
        [SerializeField] private Color additionalColor;
        public Rarity Rarity => rarity;
        public Sprite Table => table;

        public Color MainColor => mainColor;
        public Color AdditionalColor => additionalColor;
    }
}