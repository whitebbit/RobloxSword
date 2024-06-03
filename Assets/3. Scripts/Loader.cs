using System.Collections;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _3._Scripts
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;

        private void OnEnable()
        {
            GBGames.SaveLoadedCallback += LoadGame;
        }

        private void OnDisable()
        {
            GBGames.SaveLoadedCallback -= LoadGame;
        }

        private void LoadGame()
        {
            StartCoroutine(InitializeLocalizationAndLoadScene());
        }

        private IEnumerator InitializeLocalizationAndLoadScene()
        {
            yield return LocalizationSettings.InitializationOperation;
            SetLanguage(GBGames.language);

            // Проверка свободной памяти перед загрузкой
            if (!IsMemorySufficient())
            {
                Debug.Log("Insufficient memory, attempting to free up memory");
                yield return FreeUpMemory();
            }

            yield return LoadGameSceneAsync();
        }

        private void SetLanguage(string languageCode)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == languageCode);
            if (locale != null)
            {
                LocalizationSettings.SelectedLocale = locale;
            }
        }

        private IEnumerator LoadGameSceneAsync()
        {
            var asyncOperation = SceneManager.LoadSceneAsync(1);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
               // progressBar.value = Mathf.Clamp01(asyncOperation.progress / 0.9f);

                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        private bool IsMemorySufficient()
        {
            // Проверьте текущее использование памяти
            var totalMemory = System.GC.GetTotalMemory(false);
            var allocatedMemory = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
            
            // Установите порог, после которого будет попытка освободить память (например, 500MB)
            const long memoryThreshold = 500 * 1024 * 1024; // 500 MB

            Debug.Log($"Total Managed Memory: {totalMemory}, Total Allocated Memory: {allocatedMemory}");

            return allocatedMemory < memoryThreshold;
        }

        private IEnumerator FreeUpMemory()
        {
            // Выгружаем неиспользуемые ресурсы
            yield return Resources.UnloadUnusedAssets();
            // Запускаем сборщик мусора
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            // Подождите некоторое время, чтобы убедиться, что все очистки завершены
            yield return new WaitForSeconds(0.5f);

            // Повторно проверяем использование памяти
            var allocatedMemory = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
            Debug.Log($"Memory after cleanup: {allocatedMemory}");
        }
    }
}
