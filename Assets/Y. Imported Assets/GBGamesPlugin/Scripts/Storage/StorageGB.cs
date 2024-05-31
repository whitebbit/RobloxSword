using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstantGamesBridge;
using InstantGamesBridge.Common;
using InstantGamesBridge.Modules.Platform;
using InstantGamesBridge.Modules.Storage;
#if GB_NEWTONSOFT_FOR_SAVES
using Newtonsoft.Json;
#endif
using UnityEngine;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static SavesGB saves = new();
        public static event Action SaveLoadedCallback;

        private static SavesGB _platformInternalSaves;
        private static SavesGB _localStorageSaves;
        private const string SavesID = "saves";
        private static DateTime _lastCloudSaveTime;


        /// <summary>
        /// Тип хранилища по умолчанию. Используется автоматически, если при работе с данными не указывать конкретный тип.
        /// </summary>
        public static StorageType storageDefaultType => Bridge.storage.defaultType;

        /// <summary>
        /// Проверка на поддержку.
        /// </summary>
        public static bool IsSupportedStorageType(StorageType storageType) =>
            Bridge.storage.IsSupported(storageType);

        /// <summary>
        /// Проверка на доступность.
        /// </summary>
        public static bool IsAvailableStorageType(StorageType storageType) =>
            Bridge.storage.IsAvailable(storageType);


        #region Load

        ///<summary>
        /// Загрузка данных.
        /// </summary>
        private void Load()
        {
            StartCoroutine(FetchData());
        }

        private IEnumerator FetchData()
        {
            if (CanLoadFromStorage(StorageType.PlatformInternal))
                yield return LoadByStorageType(StorageType.PlatformInternal);

            if (CanLoadFromStorage(StorageType.LocalStorage))
                yield return LoadByStorageType(StorageType.LocalStorage);

            saves = _platformInternalSaves != null && _localStorageSaves != null &&
                    _platformInternalSaves.saveID > _localStorageSaves.saveID
                ? _platformInternalSaves
                : _localStorageSaves ?? _platformInternalSaves;

            SaveLoadedCallback?.Invoke();
        }

        private IEnumerator LoadByStorageType(StorageType storageType)
        {
            var loader = 0;
            var result = new SavesGB();

            Bridge.storage.Get(SavesID, (success, data) =>
            {
                if (success)
                {
#if GB_NEWTONSOFT_FOR_SAVES
                    try
                    {
                        result = string.IsNullOrEmpty(data)
                            ? new SavesGB()
                            : JsonConvert.DeserializeObject<SavesGB>(data);
                    }
                    catch (Exception e)
                    {
                        Message(e.Message, LoggerState.error);
                        result = new SavesGB();
                        throw;
                    }
#else
                    result = new SavesGB();
#endif
                    Message($"{storageType} loading saves success. Data - {data}");
                }
                else
                {
                    Message($"{storageType} loading failed", LoggerState.error);
                }

                loader += 1;
            }, storageType);

            yield return new WaitUntil(() => loader >= 1);

            switch (storageType)
            {
                case StorageType.LocalStorage:
                    _localStorageSaves = result;
                    break;
                case StorageType.PlatformInternal:
                    _platformInternalSaves = result;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageType), storageType, null);
            }
        }

        private static bool CanLoadFromStorage(StorageType type)
        {
            if (!IsSupportedStorageType(type) || !IsAvailableStorageType(type)) return false;

            return type switch
            {
                StorageType.LocalStorage => true,
                StorageType.PlatformInternal => instance.settings.useCloudSaves,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        #endregion

        #region Save

        ///<summary>
        /// Сохранение данных.
        /// </summary>
        public void Save()
        {
            saves.saveID++;
            StartCoroutine(SaveData());
        }

        private static IEnumerator SaveData()
        {
            if (CanSaveToStorage(StorageType.PlatformInternal))
                yield return SaveByStorageType(StorageType.PlatformInternal);

            if (CanSaveToStorage(StorageType.LocalStorage))
                yield return SaveByStorageType(StorageType.LocalStorage);
        }

        private static IEnumerator SaveByStorageType(StorageType storageType)
        {
#if GB_NEWTONSOFT_FOR_SAVES
            var data = JsonConvert.SerializeObject(saves);
            var saver = 0;
#else
            var data = "";
#endif
            Bridge.storage.Set(SavesID, data, (success) =>
            {
                if (success)
                {
                    Message($"{storageType} save data - {data} success ");
                }
                else
                {
                    Message($"{storageType} save data - {data} failed", LoggerState.error);
                }

                saver += 1;
            }, storageType);

            yield return new WaitUntil(() => saver >= 1);
        }


        private IEnumerator IntervalSave()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(60 * instance.settings.saveInterval);
                Save();
            }
        }

        private static bool CanSaveToStorage(StorageType type)
        {
            if (!IsSupportedStorageType(type) || !IsAvailableStorageType(type))
                return false;

            switch (type)
            {
                case StorageType.LocalStorage:
                    return true;
                case StorageType.PlatformInternal when !instance.settings.useCloudSaves:
                    return false;
                case StorageType.PlatformInternal when !instance.settings.useCloudSaveInterval:
                    return true;
                case StorageType.PlatformInternal
                    when !TimeUtils.HasTimeElapsed(_lastCloudSaveTime, instance.settings.cloudSaveInterval * 60):
                    return false;
                case StorageType.PlatformInternal:
                    _lastCloudSaveTime = DateTime.Now;
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion

        #region Delete

        public static void Delete()
        {
            var id = saves.saveID;
            saves = new SavesGB {saveID = id};
            instance.Save();
        }

        #endregion
    }
}