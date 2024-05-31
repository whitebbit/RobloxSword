using System.Collections.Generic;
using _3._Scripts.Characters;
using _3._Scripts.Config;
using _3._Scripts.UI.Scriptable.Shop;
using _3._Scripts.Wallet;
using GBGamesPlugin;

namespace _3._Scripts.UI.Panels
{
    public class UpgradesShop: ShopPanel<UpgradeItem>
    {
        protected override IEnumerable<UpgradeItem> ShopItems()
        {
            return new List<UpgradeItem>();
        }

        protected override bool ItemUnlocked(string id)
        {
            return GBGames.saves.upgradeSaves.Unlocked(id);
        }

        protected override bool IsSelected(string id)
        {
            return GBGames.saves.upgradeSaves.IsCurrent(id);
        }

        protected override bool Select(string id)
        {
            if (!ItemUnlocked(id)) return false;
            if (IsSelected(id)) return false;
            
            GBGames.saves.upgradeSaves.SetCurrent(id);
            GBGames.instance.Save();
            Player.Player.Instance.UpgradeHandler.SetUpgrade(id);
            SetSlotsState();
            return true;
        }

        protected override bool Buy(string id)
        {
            if (ItemUnlocked(id)) return false;

            var slot = GetSlot(id).Data;

            if (!WalletManager.TrySpend(slot.CurrencyType, slot.Price)) return false;
            
            GBGames.saves.upgradeSaves.Unlock(id);
            Select(id);
            return true;
        }
    }
}