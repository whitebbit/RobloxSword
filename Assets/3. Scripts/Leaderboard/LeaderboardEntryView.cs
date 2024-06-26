using InstantGamesBridge.Modules.Leaderboard;
using TMPro;
using UnityEngine;

namespace _3._Scripts.Leaderboard
{
    public class LeaderboardEntryView : MonoBehaviour
    {
        [SerializeField] private TMP_Text index;
        [SerializeField] private TMP_Text playerName;
        [SerializeField] private TMP_Text playerScore;

        public void Initialize(LeaderboardEntry entry)
        {
            index.text = entry.rank.ToString();
            playerName.text = entry.name;
            playerScore.text = entry.score.ToString();
        }
    }
}