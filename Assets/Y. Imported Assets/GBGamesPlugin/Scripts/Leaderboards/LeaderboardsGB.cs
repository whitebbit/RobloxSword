using System;
using System.Collections.Generic;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Device;
using InstantGamesBridge.Modules.Leaderboard;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        /// <summary>
        /// Поддерживает ли платформа лидерборды.
        /// </summary>
        public static bool leaderboardIsSupported => Bridge.leaderboard.isSupported;

        /// <summary>
        /// Поддерживается ли нативный popup.
        /// </summary>
        public static bool leaderboardIsNativePopupSupported => Bridge.leaderboard.isNativePopupSupported;

        /// <summary>
        /// Показать нативный popup.
        /// </summary>
        public static void ShowLeaderboardNativePopup(
            Action onShowNativePopupSuccess = null,
            Action onShowNativePopupFailed = null,
            Dictionary<string, object> options = default)
        {
            if (!leaderboardIsNativePopupSupported) return;

            Bridge.leaderboard.ShowNativePopup(options, (success) =>
            {
                if (success) onShowNativePopupSuccess?.Invoke();
                else onShowNativePopupFailed?.Invoke();
            });
        }

        /// <summary>
        /// Поддерживается ли запись очков игрока с клиента.
        /// </summary>
        public static bool leaderboardIsSetScoreSupported => Bridge.leaderboard.isSetScoreSupported;

        /// <summary>
        /// Записать очки игрока.
        /// </summary>
        public static void SetLeaderboardScore(Action onSetLeaderboardScoreSuccess = null,
            Action onSetLeaderboardScoreFailed = null, Dictionary<string, object> options = default)
        {
            if (!leaderboardIsSetScoreSupported) return;
            Bridge.leaderboard.SetScore(options, (success) =>
            {
                if (success) onSetLeaderboardScoreSuccess?.Invoke();
                else onSetLeaderboardScoreFailed?.Invoke();
            });
        }

        /// <summary>
        /// Поддерживается ли чтение очков игрока.
        /// </summary>
        public static bool leaderboardIsGetScoreSupported => Bridge.leaderboard.isGetScoreSupported;

        /// <summary>
        /// Получение очков игрока.
        /// </summary>
        public static int GetLeaderboardScore(Action onGetLeaderboardScoreSuccess = null,
            Action onGetLeaderboardScoreFailed = null, Dictionary<string, object> options = default)
        {
            if (!leaderboardIsGetScoreSupported) return 0;

            var score = 0;

            Bridge.leaderboard.GetScore(options, (success, s) =>
            {
                if (success)
                {
                    score = s;
                    onGetLeaderboardScoreSuccess?.Invoke();
                }
                else onGetLeaderboardScoreFailed?.Invoke();
            });

            return score;
        }

        /// <summary>
        /// Поддерживается ли чтение полной таблицы.
        /// </summary>
        public static bool leaderboardIsGetEntriesSupported => Bridge.leaderboard.isGetEntriesSupported;

        /// <summary>
        /// Получение записей из таблицы;
        /// </summary>
        public static List<LeaderboardEntry> GetLeaderboardEntries(Action onGetLeaderboardEntriesSuccess = null,
            Action onGetLeaderboardEntriesFailed = null,
            Dictionary<string, object> options = default)
        {
            if (!leaderboardIsGetEntriesSupported) return new List<LeaderboardEntry>();

            var entries = new List<LeaderboardEntry>();

            Bridge.leaderboard.GetEntries(options,(success, e) =>
            {
                if (success)
                {
                    entries = e;
                    onGetLeaderboardEntriesSuccess?.Invoke();
                }
                else onGetLeaderboardEntriesFailed?.Invoke();
            });

            return entries;
        }
    }
}