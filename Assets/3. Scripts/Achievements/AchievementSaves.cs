using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Config;
using UnityEngine.SocialPlatforms.Impl;

namespace _3._Scripts.Achievements
{
    [Serializable]
    public class AchievementSaves
    {
        public List<Achievement> achievements = new();
        public event Action OnAchievementComplete;
        public Achievement Get(string achievementID)
        {
            return GetOrCreateAchievement(achievementID);
        }

        public void Update(string achievementID, float progress)
        {
            var data = Configuration.Instance.AchievementData.FirstOrDefault(a => a.ID == achievementID);
            var achievement = Get(achievementID);

            achievement.progress += progress;
            if (data is not null && achievement.progress < data.Goal) return;
            
            achievement.completed = true;
            OnAchievementComplete?.Invoke();
        }
        
        private Achievement GetOrCreateAchievement(string achievementID)
        {
            var get = GetAchievement(achievementID);
            return get ?? CreateAchievement(achievementID);
        }

        private Achievement GetAchievement(string achievementID)
        {
            return achievements.FirstOrDefault(a => a.id == achievementID);
        }
        
        private Achievement CreateAchievement(string achievementID)
        {
            var newAchievement = new Achievement
            {
                id = achievementID,
                completed = false,
                progress = 0
            };

            achievements.Add(newAchievement);

            return newAchievement;
        }
    }
}