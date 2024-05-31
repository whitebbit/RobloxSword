using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency.Scriptable;
using _3._Scripts.Sounds;
using _3._Scripts.UI.Elements;
using _3._Scripts.UI.Panels.Base;
using _3._Scripts.UI.Scriptable.Shop;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Panels
{
    public abstract class ShopPanel<T> : SimplePanel where T : ShopItem
    {
        [Tab("Components")] [SerializeField] private Transform container;
        [SerializeField] private ShopSlot prefab;

        private readonly List<ShopSlot> _shopSlots = new();
        protected abstract IEnumerable<T> ShopItems();

        public override void Initialize()
        {
            InTransition = transition;
            OutTransition = transition;
            SpawnItems();
            SetSlotsState();
        }

        protected override void OnOpen()
        {
            SetSlotsState();
        }

       
        protected virtual void OnSpawnItems(ShopSlot slot, T data)
        {
        }

        private void SpawnItems()
        {
            var items = ShopItems().OrderBy(obj => obj.Price).ToList();
            foreach (var item in items)
            {
                var obj = Instantiate(prefab, container);
                var currency = Configuration.Instance.GetCurrency(item.CurrencyType);
                obj.SetView(item, currency);
                obj.SetAction(() => OnClick(item.ID));
                OnSpawnItems(obj, item);
                _shopSlots.Add(obj);
            }
        }
        

        protected void SetSlotsState()
        {
            foreach (var slot in _shopSlots)
            {
                if (!ItemUnlocked(slot.Data.ID))
                    slot.Lock();
                else
                {
                    if (IsSelected(slot.Data.ID))
                        slot.Select();
                    else
                        slot.Unselect();
                }
            }
        }

        private void OnClick(string id)
        {
            if (ItemUnlocked(id))
            {
                if(Select(id))
                    SoundManager.Instance.PlayOneShot("select");
            }
            else
            {
                if(Buy(id))
                    SoundManager.Instance.PlayOneShot("unlock");
            }
        }

        protected ShopSlot GetSlot(string id) => _shopSlots.FirstOrDefault(s => s.Data.ID == id);
        protected abstract bool ItemUnlocked(string id);
        protected abstract bool IsSelected(string id);
        protected abstract bool Select(string id);
        protected abstract bool Buy(string id);
    }
}