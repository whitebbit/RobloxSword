using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Interactive.Interfaces;
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
    public class PetUnlocker : MonoBehaviour, IInteractive
    {
        [SerializeField] private Transform useTutorial;
        
        [Tab("Settings")] [SerializeField] private int price;
        [SerializeField] private float priceMultiplier;

        [SerializeField] private List<PetData> data = new();

        [Tab("Components")] [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Image currencyIcon;

        private float Price()
        {
            var pets = GBGames.saves.petSaves.unlocked.Count;
            var multiplier = pets > 0 ? pets * priceMultiplier : 1;
            return price * multiplier;
        }

        private void Awake()
        {
            canvas.worldCamera = Camera.main;
        }

        private void Start()
        {
            StopInteract();
            UpdatePrice();
        }

        private void OnEnable()
        {
            WalletManager.OnSecondCurrencyChange += WalletManagerOnOnSecondCurrencyChange;
        }

        private void OnDisable()
        {
            WalletManager.OnSecondCurrencyChange -= WalletManagerOnOnSecondCurrencyChange;
        }

        private void WalletManagerOnOnSecondCurrencyChange(float arg1, float arg2)
        {
            UpdatePrice();
        }

        private void UnlockRandom()
        {
            var list = data.Where(p => !GBGames.saves.petSaves.Unlocked(p.ID)).ToList();
            if (list.Count <= 0) return;

            var rand = GetRandomItem(list);
            if (rand == null) return;

            if (!WalletManager.TrySpend(CurrencyType.Second, Price())) return;

            GBGames.saves.petSaves.Unlock(rand.ID);
            GBGames.instance.Save();
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
            priceText.text = WalletManager.ConvertToWallet((decimal) Price());
        }

        public void StartInteract()
        {
            useTutorial.gameObject.SetActive(true);
        }

        public void Interact()
        {
            UnlockRandom();
        }

        public void StopInteract()
        {
            useTutorial.gameObject.SetActive(false);
        }
    }
}