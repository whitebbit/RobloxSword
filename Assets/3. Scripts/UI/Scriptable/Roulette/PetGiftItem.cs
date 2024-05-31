using _3._Scripts.Pets.Scriptables;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Scriptable.Roulette
{
    [CreateAssetMenu(fileName = "PetGiftItem", menuName = "Roulette Item/Pet Gift Item", order = 0)]
    public class PetGiftItem: GiftItem
    {
        [SerializeField] private PetData data;
        public override Sprite Icon()
        {
            return data.Icon;
        }


        public override string Title()
        {
            return "";
        }

        public override void OnReward()
        {
            GBGames.saves.petSaves.Unlock(data.ID);
        }
    }
}