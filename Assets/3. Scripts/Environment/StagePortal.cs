using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Environment;
using _3._Scripts.Localization;
using _3._Scripts.Stages.Enums;
using _3._Scripts.Swords.Scriptable;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Components;
using VInspector;

namespace _3._Scripts.Stages
{
    public class StagePortal : MonoBehaviour
    {
        [SerializeField] private TeleportType type;
        [SerializeField] private LocalizeStringEvent text;
        [Tab("Next Values")] [SerializeField] private SwordData swordToUnlock;
        [SerializeField] private float cupsToUnlock;

        private async void Start()
        {
            switch (type)
            {
                case TeleportType.Next:
                    var swordName = await swordToUnlock.EnemyData.LocalizeID.GetTranslate();

                    text.SetReference("teleport_price");
                    text.SetVariable("cups", WalletManager.ConvertToWallet((decimal) cupsToUnlock));
                    text.SetVariable("sword", swordName);

                    break;
                case TeleportType.Previous:
                    text.SetReference("teleport_return");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CanTeleportToNextStage()
        {
            var swordStr = swordToUnlock.EnemyData.CurrentStrength;
            var currentSword =
                Configuration.Instance.SwordData.FirstOrDefault(s => s.ID == GBGames.saves.swordSaves.current);
            var currentStr = currentSword == null ? 1 : currentSword.EnemyData.CurrentStrength;
            return WalletManager.SecondCurrency >= cupsToUnlock &&
                   (GBGames.saves.swordSaves.IsCurrent(swordToUnlock.ID) || currentStr >= swordStr);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;

            switch (type)
            {
                case TeleportType.Next:
                    if (!CanTeleportToNextStage()) return;

                    StageController.Instance.TeleportToNextStage();
                    break;
                case TeleportType.Previous:
                    StageController.Instance.TeleportToPreviousStage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}