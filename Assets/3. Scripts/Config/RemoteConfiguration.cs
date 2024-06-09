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
        [SerializeField] private RemoteConfig<float> enemyStrengthMultiplier;
        [SerializeField] private RemoteConfig<float> enemyCupsMultiplier;
        [SerializeField] private RemoteConfig<float> petBoosterAdditionalPercent;
        [SerializeField] private RemoteConfig<float> characterBoosterAdditionalPercent;
        [SerializeField] private RemoteConfig<float> swordAdditionalBooster;

        public static float ShopPriceMultiplier => Instance.shopPriceMultiplier.Value;
        public static float EnemyStrengthMultiplier => Instance.enemyStrengthMultiplier.Value;
        public static float EnemyCupsMultiplier => Instance.enemyCupsMultiplier.Value;
        public static float BoostTime => Instance.boostTime.Value;
        public static float InterstitialTimer => Instance.interstitialTimer.Value;
        public static float PetBoosterAdditionalPercent => Instance.petBoosterAdditionalPercent.Value;
        public static float CharacterBoosterAdditionalPercent => Instance.characterBoosterAdditionalPercent.Value;
        public static float SwordAdditionalBooster=> Instance.swordAdditionalBooster.Value;
    }
}