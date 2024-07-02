using System;
using System.Collections;
using System.Collections.Generic;
using _3._Scripts.Singleton;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Leaderboard
{
    public class Leaderboard : Singleton<Leaderboard>
    {
        [SerializeField] private string leaderboardName;
        [Space] [SerializeField] private List<LeaderboardEntryView> leaderboardEntryViews = new();

        private void Awake()
        {
            if (!GBGames.leaderboardIsSupported)
            {
                gameObject.SetActive(false);
                return;
            }

            GBGames.instance.LoadLeaderboardScore(leaderboardName);
            StartCoroutine(InitializeLeaderboard());
        }


        public void UpdateScore(float score)
        {
            if (Math.Abs(GBGames.GetLeaderboardScore(leaderboardName) - score) > 0.1f)
                GBGames.SetLeaderboardScore(leaderboardName, score);
        }

        private IEnumerator InitializeLeaderboard()
        {
            yield return GBGames.instance.LoadLeaderboardEntries(leaderboardName);

            var index = 0;
            foreach (var entry in GBGames.leaderboardEntries)
            {
                leaderboardEntryViews[index].Initialize(entry);
                index++;
            }
        }
    }
}