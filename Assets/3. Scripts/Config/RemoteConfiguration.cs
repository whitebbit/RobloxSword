using _3._Scripts.Singleton;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

namespace _3._Scripts.Config
{
    public class RemoteConfiguration : Singleton<RemoteConfiguration>
    {
        [SerializeField] private RemoteConfig<float> shopPriceMultiplier;
        [SerializeField] private RemoteConfig<float> boostTime;
        [SerializeField] private RemoteConfig<float> interstitialTimer;
        [SerializeField] private RemoteConfig<float> enemyStrength;

        public static float ShopPriceMultiplier => Instance.shopPriceMultiplier.Value;
        public static float EnemyStrength => Instance.enemyStrength.Value;
        public static float BoostTime => Instance.boostTime.Value;
        public static float InterstitialTimer => Instance.interstitialTimer.Value;
    }
}