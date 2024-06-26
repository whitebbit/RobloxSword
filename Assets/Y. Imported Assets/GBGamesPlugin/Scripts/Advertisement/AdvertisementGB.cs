using System;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Advertisement;
using InstantGamesBridge.Modules.Device;
using UnityEngine;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        
        public static bool NowAdsShow =>
            interstitialState == InterstitialState.Opened || rewardedState == RewardedState.Opened;
        
        #region Banner

        /// <summary>
        /// Поддерживается ли баннер.
        /// </summary>
        public static bool isBannerSupported => Bridge.advertisement.isBannerSupported;

        /// <summary>
        /// Текущее состояние баннера. Возможные значения: Loading, Shown, Hidden, Failed.
        /// </summary>
        public static BannerState bannerState => Bridge.advertisement.bannerState;

        /// <summary>
        /// Показать баннер.
        /// </summary>
        public static void ShowBanner()
        {
            if (isBannerSupported) Bridge.advertisement.ShowBanner();
        }

        /// <summary>
        /// Скрыть баннер.
        /// </summary>
        public static void HideBanner()
        {
            if (isBannerSupported) Bridge.advertisement.HideBanner();
        }

        public static event Action BannerLoadingCallback;
        public static event Action BannerShownCallback;
        public static event Action BannerHiddenCallback;
        public static event Action BannerFailedCallback;

        private void OnBannerStateChanged(BannerState state)
        {
            switch (state)
            {
                case BannerState.Loading:
                    BannerLoadingCallback?.Invoke();
                    Message("Banner state - loading");
                    break;
                case BannerState.Shown:
                    BannerShownCallback?.Invoke();

                    Message("Banner state - shown");
                    break;
                case BannerState.Hidden:
                    BannerHiddenCallback?.Invoke();

                    Message("Banner state - hidden");
                    break;
                case BannerState.Failed:
                    BannerFailedCallback?.Invoke();

                    Message("Banner state - failed", LoggerState.error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        #endregion

        #region Interstitial

        /// <summary>
        /// Текущее состояние рекламы. Возможные значения: Loading, Opened, Closed, Failed.
        /// </summary>
        public static InterstitialState interstitialState => Bridge.advertisement.interstitialState;

        /// <summary>
        /// Минимальный интервал между показами межстраничной рекламы.
        /// </summary>
        public static int minimumDelayBetweenInterstitial
        {
            get => Bridge.advertisement.minimumDelayBetweenInterstitial;
            set => Bridge.advertisement.SetMinimumDelayBetweenInterstitial(value);
        }

        /// <summary>
        /// Показать межстраничную рекламу.
        /// </summary>
        public static void ShowInterstitial()
        {
            Bridge.advertisement.ShowInterstitial();
        }

        public static event Action InterstitialLoadingCallback;
        public static event Action InterstitialOpenedCallback;
        public static event Action InterstitialClosedCallback;
        public static event Action InterstitialFailedCallback;

        private void OnInterstitialStateChanged(InterstitialState state)
        {
            switch (state)
            {
                case InterstitialState.Loading:
                    InterstitialLoadingCallback?.Invoke();
                    Message("Interstitial state - loading");
                    break;
                case InterstitialState.Opened:
                    InterstitialOpenedCallback?.Invoke();
                    Message("Interstitial state - opened");
                    if (_inGame)
                        PauseController.Pause(true);
                    break;
                case InterstitialState.Closed:
                    PauseController.Pause(false);
                    
                    InterstitialClosedCallback?.Invoke();
                    Message("Interstitial state - closed");
                    break;
                case InterstitialState.Failed:
                    InterstitialFailedCallback?.Invoke();
                    Message("Interstitial state - failed");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        #endregion

        #region Rewarded

        /// <summary>
        /// Текущее состояние рекламы. Возможные значения: Loading, Opened, Closed, Rewarded, Failed.
        /// </summary>
        public static RewardedState rewardedState => Bridge.advertisement.rewardedState;

        /// <summary>
        /// Показать рекламу за вознаграждение.
        /// </summary>
        public static void ShowRewarded(Action onRewarded)
        {
            RewardedCallback = onRewarded;
            Bridge.advertisement.ShowRewarded();
        }

        private static event Action RewardedCallback;

        public static event Action RewardedLoadingCallback;
        public static event Action RewardedOpenedCallback;
        public static event Action RewardedClosedCallback;
        public static event Action RewardedFailedCallback;

        private void OnRewardedStateChanged(RewardedState state)
        {
            switch (state)
            {
                case RewardedState.Loading:
                    RewardedLoadingCallback?.Invoke();
                    Message("Rewarded state - loading");
                    break;
                case RewardedState.Opened:
                    RewardedOpenedCallback?.Invoke();
                    Message("Rewarded state - opened");
                    PauseController.Pause(true);
                    break;
                case RewardedState.Rewarded:
                    RewardedCallback?.Invoke();
                    RewardedCallback = null;
                    break;
                case RewardedState.Closed:
                    PauseController.Pause(false);
                    RewardedClosedCallback?.Invoke();
                    Message("Rewarded state - closed");
                    break;
                case RewardedState.Failed:
                    RewardedFailedCallback?.Invoke();
                    Message("Rewarded state - failed");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        #endregion
        
    }
}