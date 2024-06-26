using System;
using System.Collections.Generic;
using System.Linq;
using InstantGamesBridge;
using InstantGamesBridge.Modules.RemoteConfig;
using UnityEngine;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static bool remoteConfigIsSupported => Bridge.remoteConfig.isSupported;
        private static List<RemoteConfigValue> _data = new();

        public static T GetRemoteValue<T>(string id, T defaultValue = default)
        {
            var configValue = _data.FirstOrDefault(d => d.name == id);

            if (configValue == null || !remoteConfigIsSupported)
            {
                return defaultValue;
            }

            try
            {
                if (typeof(T) == typeof(string))
                {
                    return (T) (object) configValue.value;
                }

                if (typeof(T) == typeof(int))
                {
                    return (T) (object) int.Parse(configValue.value);
                }

                if (typeof(T) == typeof(float))
                {
                    return (T) (object) float.Parse(configValue.value);
                }

                if (typeof(T) == typeof(bool))
                {
                    return (T) (object) bool.Parse(configValue.value);
                }

                throw new NotSupportedException($"Type {typeof(T)} is not supported");
            }
            catch
            {
                return defaultValue;
            }
        }

        private static void LoadRemoteConfig(Dictionary<string, object> options = default)
        {
            if(!remoteConfigIsSupported) return;
            Bridge.remoteConfig.Get(options, OnRemoteConfigGetCompleted);
        }

        private static void OnRemoteConfigGetCompleted(bool success, List<RemoteConfigValue> data)
        {
            if (success)
            {
                _data = data;
                Message("Load Remote Config Success");
            }
            else
            {
                _data = new List<RemoteConfigValue>();
                Message("Load Remote Config Failed", LoggerState.error);
            }
        }
    }
}