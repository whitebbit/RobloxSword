#if UNITY_WEBGL
using System.Collections;
using InstantGamesBridge;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBGamesPlugin
{
    public partial class GBGames : MonoBehaviour
    {
        public static GBGames instance { get; private set; }
        public GBGamesSettings settings;
        private static bool _inGame;
        private void Awake()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            _inGame = true;
            Singleton();
            yield return new WaitUntil(() => Bridge.instance != null && Bridge.Initialized);
            Storage();
            RemoteConfig();
            Advertisement();
            Platform();
            Player();
            Game();
        }
        
        private void Singleton()
        {
            transform.SetParent(null);
            gameObject.name = "GBGames";

            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Advertisement()
        {
            Bridge.advertisement.bannerStateChanged += OnBannerStateChanged;
            Bridge.advertisement.interstitialStateChanged += OnInterstitialStateChanged;
            Bridge.advertisement.rewardedStateChanged += OnRewardedStateChanged;

            minimumDelayBetweenInterstitial = instance.settings.minimumDelayBetweenInterstitial;

            if (instance.settings.enableBannerAutomatically)
                ShowBanner();
        }

        private void Platform()
        {
            if (instance.settings.autoGameReadyAPI)
                GameReady();

            if (instance.settings.gameLoadingCallbacksOnSceneLoading)
            {
                SceneManager.sceneLoaded += (_, _) => { InGameLoadingStopped(); };
            }
        }

        private void Player()
        {
            if (instance.settings.authPlayerAutomatically)
                AuthorizePlayer();
        }

        private void Storage()
        {
            Load();
            if (instance.settings.autoSaveByInterval)
                StartCoroutine(IntervalSave());
        }

        private void Game()
        {
            Bridge.game.visibilityStateChanged += OnGameVisibilityStateChanged;
            if (instance.settings.saveOnChangeVisibilityState)
                GameHiddenStateCallback += Save;
        }

        private void RemoteConfig()
        {
            LoadRemoteConfig();
        }
        
    }
}
#endif