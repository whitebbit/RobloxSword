using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Device;
using InstantGamesBridge.Modules.Leaderboard;
using UnityEngine;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static List<LeaderboardEntry> leaderboardEntries = new();
        private static readonly Dictionary<string, float> LeaderboardsScore = new();

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
        public static void SetLeaderboardScore(string name, float score,
            Action onSetLeaderboardScoreSuccess = null,
            Action onSetLeaderboardScoreFailed = null)
        {
            if (!leaderboardIsSetScoreSupported) return;

            SetLeaderboardScore(new Dictionary<string, object>
            {
                {"leaderboardName", name}, {"score", score}
            }, onSetLeaderboardScoreSuccess, onSetLeaderboardScoreFailed);

            LeaderboardsScore[name] = score;
        }

        private static void SetLeaderboardScore(Dictionary<string, object> options = default,
            Action onSetLeaderboardScoreSuccess = null,
            Action onSetLeaderboardScoreFailed = null)
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
        public static float GetLeaderboardScore(string name)
        {
            return !leaderboardIsGetScoreSupported ? 0 : LeaderboardsScore.GetValueOrDefault(name, 0);
        }

        public void LoadLeaderboardScore(string leaderboardName,
            Action onGetLeaderboardScoreSuccess = null,
            Action onGetLeaderboardScoreFailed = null)
        {
            if (!leaderboardIsGetScoreSupported) return;

            var options = new Dictionary<string, object>
            {
                {"leaderboardName", leaderboardName}
            };

            StartCoroutine(FetchLeaderboardScore(options, onGetLeaderboardScoreSuccess, onGetLeaderboardScoreFailed));
        }

        private IEnumerator FetchLeaderboardScore(
            Dictionary<string, object> options,
            Action onGetLeaderboardScoreSuccess = null,
            Action onGetLeaderboardScoreFailed = null)
        {
            var loader = 0;
            var score = 0;
            Bridge.leaderboard.GetScore(options, (success, s) =>
            {
                if (success)
                {
                    score = s;
                    onGetLeaderboardScoreSuccess?.Invoke();
                }
                else onGetLeaderboardScoreFailed?.Invoke();

                loader += 1;
            });

            yield return new WaitUntil(() => loader >= 1);

            LeaderboardsScore.Add(options["leaderboardName"].ToString(), score);
        }

        /// <summary>
        /// Поддерживается ли чтение полной таблицы.
        /// </summary>
        public static bool leaderboardIsGetEntriesSupported => Bridge.leaderboard.isGetEntriesSupported;

        /// <summary>
        /// Получение записей из таблицы;
        /// </summary>
        public IEnumerator LoadLeaderboardEntries(string leaderboardName, bool includeUser = true,
            int quantityAround = 10, int quantityTop = 10,
            Action onGetLeaderboardEntriesSuccess = null,
            Action onGetLeaderboardEntriesFailed = null)
        {
            if (!leaderboardIsGetEntriesSupported) yield break;
            var options = new Dictionary<string, object>
            {
                {"leaderboardName", leaderboardName},
                {"includeUser", includeUser},
                {"quantityAround", quantityAround},
                {"quantityTop", quantityTop},
            };

            yield return StartCoroutine(FetchLeaderboardEntries(options, onGetLeaderboardEntriesSuccess,
                onGetLeaderboardEntriesFailed));
        }

        private IEnumerator FetchLeaderboardEntries(
            Dictionary<string, object> options,
            Action onGetLeaderboardEntriesSuccess = null,
            Action onGetLeaderboardEntriesFailed = null)
        {
            var loader = 0;
            var entries = new List<LeaderboardEntry>();
            Bridge.leaderboard.GetEntries(options, (success, e) =>
            {
                if (success)
                {
                    entries = e;
                    onGetLeaderboardEntriesSuccess?.Invoke();
                }
                else onGetLeaderboardEntriesFailed?.Invoke();

                loader += 1;
            });

            yield return new WaitUntil(() => loader >= 1);

            leaderboardEntries = entries;
        }
    }
}