using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Pets.Scriptables;
using _3._Scripts.UI.Enums;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using Random = UnityEngine.Random;

namespace _3._Scripts.Pets
{
    public class PetUnlocker : MonoBehaviour
    {
        [SerializeField] private List<PetData> data = new();
        [Tab("Components")] [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Image currencyIcon;

        private void Awake()
        {
            canvas.worldCamera = Camera.main;
        }

        private void Start()
        {
            UpdatePrice();
        }

        private int Price()
        {
            return GBGames.saves.petSaves.unlocked.Count switch
            {
                <= 0 => 1,
                >0 and <10 => 5,
                >=10 and <15 => 10,
                >= 15 => 20
            };
        }

        private void UnlockRandom()
        {
            if (!WalletManager.TrySpend(CurrencyType.Second, Price())) return;

            var list = data.Where(p => !GBGames.saves.petSaves.Unlocked(p.ID)).ToList();
            if (list.Count <= 0) return;

            var rand = GetRandomItem(list);
            if (rand == null) return;

            GBGames.saves.petSaves.Unlock(rand.ID);
            GBGames.instance.Save();
            WalletManager.SecondCurrency -= Price();
            UpdatePrice();
        }

        private PetData GetRandomItem(IEnumerable<PetData> items)
        {
            var itemWeights = new Dictionary<PetData, int>();

            const int rareWeight = 75;
            const int mythicalWeight = 20;
            const int legendaryWeight = 5;

            foreach (var item in items)
            {
                itemWeights[item] = item.Rarity switch
                {
                    Rarity.Rare => rareWeight,
                    Rarity.Mythical => mythicalWeight,
                    Rarity.Legendary => legendaryWeight,
                    _ => itemWeights[item]
                };
            }

            var totalWeight = itemWeights.Values.Sum();

            var randomWeightPoint = Random.Range(0, totalWeight);

            foreach (var (key, value) in itemWeights)
            {
                if (randomWeightPoint < value)
                {
                    return key;
                }

                randomWeightPoint -= value;
            }

            return null;
        }

        private void UpdatePrice()
        {
            var image = Configuration.Instance.GetCurrency(CurrencyType.Second).Icon;
            currencyIcon.sprite = image;
            priceText.text = Price().ToString();
        }

        private void OnMouseDown()
        {
            UnlockRandom();
        }
    }
}