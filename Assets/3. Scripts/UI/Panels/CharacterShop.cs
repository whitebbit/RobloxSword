using System.Collections.Generic;
using _3._Scripts.Config;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.Wallet;
using GBGamesPlugin;

namespace _3._Scripts.UI.Panels
{
    public class CharacterShop : ShopPanel<CharacterItem>
    {
        protected override IEnumerable<CharacterItem> ShopItems()
        {
            return Configuration.Instance.AllCharacters;
        }

        protected override bool ItemUnlocked(string id)
        {
            return GBGames.saves.characterSaves.Unlocked(id);
        }

        protected override bool IsSelected(string id)
        {
            return GBGames.saves.characterSaves.IsCurrent(id);
        }

        protected override bool Select(string id)
        {
            if (!ItemUnlocked(id)) return false;
            if (IsSelected(id)) return false;

            var player = Player.Player.Instance;
            player.CharacterHandler.SetCharacter(id, player.transform);
            GBGames.saves.characterSaves.SetCurrent(id);
            SetSlotsState();
            GBGames.instance.Save();

            return true;
        }

        protected override bool Buy(string id)
        { 
            if (ItemUnlocked(id)) return false;

            var slot = GetSlot(id).Data;

            if (!WalletManager.TrySpend(slot.CurrencyType, slot.Price)) return false;
            
            GBGames.saves.characterSaves.Unlock(id);
            Select(id);
            return true;
        }
    }
}