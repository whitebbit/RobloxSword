using System;
using System.Collections;
using System.Collections.Generic;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Loader : MonoBehaviour
{
    private void OnEnable()
    {
        GBGames.SaveLoadedCallback += Load;
    }

    private void OnDisable()
    {
        GBGames.SaveLoadedCallback -= Load;
    }

    private void Load()
    {
        StartCoroutine(InitializeLocalizationAndLoadScene());
    }
    private IEnumerator InitializeLocalizationAndLoadScene()
    {
        yield return LocalizationSettings.InitializationOperation;

        SetLanguage(GBGames.language);
        SceneLoader.LoadScene(1);
    }

    private void SetLanguage(string languageCode)
    {
        var locale = LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == languageCode);
        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
        }
        else
        {
            Debug.LogWarning($"Locale for code {languageCode} not found.");
        }
    }
}