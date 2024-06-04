using _3._Scripts.Config;
using _3._Scripts.Pets.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class PetBooster: MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text counter;

        public void SetBooster(PetData data)
        {
            var currency = Configuration.Instance.GetCurrency(data.CurrencyType);
            icon.sprite = currency.Icon;
            counter.text = $"+{data.Booster}%";
        }
    }
}