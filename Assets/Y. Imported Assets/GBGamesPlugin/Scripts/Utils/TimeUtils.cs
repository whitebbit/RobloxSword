using System;
using UnityEngine;

namespace GBGamesPlugin
{
    public static class TimeUtils
    {
        public static bool HasTimeElapsed(DateTime eventTime, int seconds)
        {
            var currentTime = DateTime.Now;
            var timeElapsed = currentTime - eventTime;
            return timeElapsed.TotalSeconds > seconds;
        }
    }
}