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
        public static void ShowLeaderboardNativePopup(Action onShowNativePopupSuccess = null,
            Action onShowNativePopupFailed = null,
            params ShowNativePopupPlatformDependedOptions[] platformDependedOptions)
        {
            if (!leaderboardIsNativePopupSupported) return;

            Bridge.leaderboard.ShowNativePopup((success) =>
            {
                if (success) onShowNativePopupSuccess?.Invoke();
                else onShowNativePopupFailed?.Invoke();
            }, platformDependedOptions);
        }

        /// <summary>
        /// Поддерживается ли запись очков игрока с клиента.
        /// </summary>
        public static bool leaderboardIsSetScoreSupported => Bridge.leaderboard.isSetScoreSupported;

        /// <summary>
        /// Записать очки игрока.
        /// </summary>
        public static void SetLeaderboardScore(Action onSetLeaderboardScoreSuccess = null,
            Action onSetLeaderboardScoreFailed = null, params SetScorePlatformDependedOptions[] platformDependedOptions)
        {
            if (!leaderboardIsSetScoreSupported) return;
            Bridge.leaderboard.SetScore((success) =>
            {
                if (success) onSetLeaderboardScoreSuccess?.Invoke();
                else onSetLeaderboardScoreFailed?.Invoke();
            }, platformDependedOptions);
        }

        /// <summary>
        /// Поддерживается ли чтение очков игрока.
        /// </summary>
        public static bool leaderboardIsGetScoreSupported => Bridge.leaderboard.isGetScoreSupported;

        /// <summary>
        /// Получение очков игрока.
        /// </summary>
        public static int GetLeaderboardScore(Action onGetLeaderboardScoreSuccess = null,
            Action onGetLeaderboardScoreFailed = null, params GetScorePlatformDependedOptions[] platformDependedOptions)
        {
            if (!leaderboardIsGetScoreSupported) return 0;

            var score = 0;

            Bridge.leaderboard.GetScore((success, s) =>
            {
                if (success)
                {
                    score = s;
                    onGetLeaderboardScoreSuccess?.Invoke();
                }
                else onGetLeaderboardScoreFailed?.Invoke();
            }, platformDependedOptions);

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
            params GetEntriesPlatformDependedOptions[] platformDependedOptions)
        {
            if (!leaderboardIsGetEntriesSupported) return new List<LeaderboardEntry>();

            var entries = new List<LeaderboardEntry>();

            Bridge.leaderboard.GetEntries((success, e) =>
            {
                if (success)
                {
                    entries = e;
                    onGetLeaderboardEntriesSuccess?.Invoke();
                }
                else onGetLeaderboardEntriesFailed?.Invoke();
            }, platformDependedOptions);

            return entries;
        }
    }
}