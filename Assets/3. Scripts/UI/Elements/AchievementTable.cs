using _3._Scripts.Achievements;
using _3._Scripts.Achievements.Scriptables;
using _3._Scripts.Localization;
using _3._Scripts.UI.Extensions;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace _3._Scripts.UI.Elements
{
    public class AchievementTable : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private LocalizeStringEvent title;
        [SerializeField] private TMP_Text goal;
        [SerializeField] private Slider progress;
        [SerializeField] private Button getReward;
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TMP_Text rewardText;

        private AchievementData _data;

        public void Initialize(AchievementData data)
        {
            _data = data;
            icon.sprite = data.Icon;
            icon.ScaleImage();
            title.SetReference(data.TitleID);
            getReward.onClick.AddListener(GetReward);
            UpdateView();
        }

        public void UpdateView()
        {
            var save = GBGames.saves.achievementSaves.Get(_data.ID);

            if (!save.completed)
            {
                getReward.interactable = false;
            }
            else
            {
                if (save.giftReceived) getReward.gameObject.SetActive(false);
                else getReward.interactable = true;
            }
            
            goal.text = WalletManager.ConvertToWallet((decimal) _data.Goal);
            progress.value = save.progress / _data.Goal;
            rewardIcon.sprite = _data.Gift.Icon();
            rewardText.text = _data.Gift.Title();
        }

        private void GetReward()
        {
            var save = GBGames.saves.achievementSaves.Get(_data.ID);
            save.giftReceived = true;
            _data.Gift.OnReward();
            getReward.gameObject.SetActive(false);
        }
    }
}