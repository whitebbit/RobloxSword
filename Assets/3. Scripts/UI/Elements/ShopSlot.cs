using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _3._Scripts.Currency.Scriptable;
using _3._Scripts.Localization;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.UI.Structs;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using VInspector;

namespace _3._Scripts.UI.Elements
{
    public class ShopSlot : MonoBehaviour
    {
        [Tab("Rarity Tables")] [SerializeField]
        private List<RarityTable> rarityTables = new();

        [Tab("UI")] [SerializeField] private TMP_Text title;
        [SerializeField] private Image glow;
        [SerializeField] private Image icon;
        [SerializeField] private Image table;
        [SerializeField] private Image backGlow;
        [Tab("Currency")] [SerializeField] private Image currencyIcon;
        [SerializeField] private TMP_Text price;
        [Tab("Localization")] [SerializeField] private string selectKey;
        [SerializeField] private string selectedKey;
        
        public ShopItem Data { get; private set; }
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void SetView(ShopItem item, CurrencyData currencyData)
        {
            var rarity = rarityTables.FirstOrDefault(r => r.Rarity == item.Rarity);
            table.sprite = rarity.Table;
            glow.color = rarity.MainColor;
            backGlow.color = rarity.AdditionalColor;
            icon.sprite = item.Icon;
            currencyIcon.sprite = currencyData.Icon;
            title.text = item.Title();
            Data = item;
        }

        public void SetAction(Action action) => _button.onClick.AddListener(() => action?.Invoke());

        public async void Select()
        {
            var localizedString = await selectedKey.GetTranslate();
            
            price.text = localizedString;
            currencyIcon.gameObject.SetActive(false);
        }

        public async void Unselect()
        {
            var localizedString = await selectKey.GetTranslate();
            price.text = localizedString;
            currencyIcon.gameObject.SetActive(false);
        }
        
        public void Lock()
        {
            price.text = Data.Price.ToString();
            currencyIcon.gameObject.SetActive(true);
        }

        public void SetIconColor(Color color) => icon.color = color;
    }
}