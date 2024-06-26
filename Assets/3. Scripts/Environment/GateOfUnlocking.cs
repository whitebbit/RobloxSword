using System;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.Localization;
using _3._Scripts.Swords.Scriptable;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;

namespace _3._Scripts.Environment
{
    public class GateOfUnlocking : MonoBehaviour
    {
        [SerializeField] private SwordData swordToUnlock;
        [SerializeField] private int cupsToUnlock;
        [Space] [SerializeField] private LocalizeStringEvent text;

        private async void Start()
        {
            var swordName = await swordToUnlock.EnemyData.LocalizeID.GetTranslate();
            text.SetVariable("cups", cupsToUnlock);
            text.SetVariable("sword", swordName);
        }

        private bool CanTeleportToNextStage()
        {
            return (WalletManager.SecondCurrency >= cupsToUnlock &&
                    GBGames.saves.swordSaves.IsCurrent(swordToUnlock.ID));

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player.Player _)) return;

            if (!CanTeleportToNextStage()) return;

            GBGames.saves.stageID += 1;
            GBGames.instance.Save();

            SceneManager.LoadSceneAsync("MainScene");
        }
    }
}