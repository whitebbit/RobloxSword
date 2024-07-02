using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.UI.Elements
{
    public class AchievementNotification : MonoBehaviour
    {
        [SerializeField] private Transform notification;

        private void OnEnable()
        {
            GBGames.saves.achievementSaves.OnAchievementComplete += SetState;
        }

        private void OnDisable()
        {
            GBGames.saves.achievementSaves.OnAchievementComplete -= SetState;
        }

        private void SetState()
        {
            notification.gameObject.SetActive(true);
        }
    }
}