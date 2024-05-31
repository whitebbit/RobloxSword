using System;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Device;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        /// <summary>
        /// Добавить на рабочий стол. Поддерживается ли функционал на платформе.
        /// </summary>
        public static bool isAddToHomeScreenSupported => Bridge.social.isAddToHomeScreenSupported;

        /// <summary>
        /// Добавить на рабочий стол.
        /// </summary>
        public static void AddToHomeScreen(Action onAddToHomeScreenSuccess = null,
            Action onAddToHomeScreenFailed = null)
        {
            if (!isAddToHomeScreenSupported) return;

            Bridge.social.AddToHomeScreen((success) =>
            {
                if (success) onAddToHomeScreenSuccess?.Invoke();
                else onAddToHomeScreenFailed?.Invoke();
            });
        }
        
        /// <summary>
        /// Оценить игру. Поддерживается ли функционал на платформе.
        /// </summary>
        public static bool isRateSupported => Bridge.social.isRateSupported;

        /// <summary>
        /// Оценить игру.
        /// </summary>
        public static void Rate(Action onRateSuccess = null,
            Action onRateFailed = null)
        {
            if(!isRateSupported) return;
            
            Bridge.social.Rate((success) =>
            {
                if(success) onRateSuccess?.Invoke();
                else onRateFailed?.Invoke();
            });
        }
    }
}