using System.Collections;
using _3._Scripts.Config;
using _3._Scripts.UI;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace _3._Scripts.Ads
{
    public class InterstitialsTimer : MonoBehaviour
    {
        [SerializeField] private GameObject secondsPanelObject;
        [SerializeField] private LocalizeStringEvent localizedText;
        
        private int _adCountdown = 3;
        private bool _isAdShowing;
        private Coroutine _checkTimerCoroutine;
        private Coroutine _adShowCoroutine;
        private Coroutine _backupTimerCoroutine;

        private void Start()
        {
            secondsPanelObject.SetActive(false);
            StartCheckTimerCoroutine();
        }

        private void OnEnable()
        {
            GBGames.InterstitialClosedCallback += OnInterstitialClosed;
            GBGames.InterstitialOpenedCallback += OnInterstitialOpened;
            GBGames.InterstitialFailedCallback += OnInterstitialFailed;
        }

        private void OnDisable()
        {
            GBGames.InterstitialClosedCallback -= OnInterstitialClosed;
            GBGames.InterstitialOpenedCallback -= OnInterstitialOpened;
            GBGames.InterstitialFailedCallback -= OnInterstitialFailed;
            
            StopAllCoroutines();
        }

        private void OnInterstitialFailed()
        {
            ResetTimer();
            StartBackupTimerCoroutine();
        }

        private void OnInterstitialOpened()
        {
            secondsPanelObject.SetActive(false);
            _isAdShowing = true;

            if (_backupTimerCoroutine == null) return;
            
            StopCoroutine(_backupTimerCoroutine);
            _backupTimerCoroutine = null;
        }

        private void OnInterstitialClosed()
        {
            ResetTimer();
            _isAdShowing = false;
            StartCheckTimerCoroutine();
        }

        private void ResetTimer()
        {
            _adCountdown = 0;
            UIManager.Instance.Active = false;
            secondsPanelObject.SetActive(false);
            PauseController.Pause(false);
        }

        private void StartCheckTimerCoroutine()
        {
            if (_checkTimerCoroutine != null)
            {
                StopCoroutine(_checkTimerCoroutine);
            }
            _checkTimerCoroutine = StartCoroutine(CheckTimerCoroutine());
        }

        private IEnumerator CheckTimerCoroutine()
        {
            yield return new WaitForSeconds(RemoteConfiguration.InterstitialTimer);

            while (GBGames.NowAdsShow && UIManager.Instance.Active)
            {
                yield return new WaitForSeconds(1);
            }

            _adCountdown = 3;
            secondsPanelObject.SetActive(true);
            UIManager.Instance.Active = true;
            _adShowCoroutine = StartCoroutine(AdShowCoroutine());
        }

        private IEnumerator AdShowCoroutine()
        {
            while (_adCountdown > 0)
            {
                UpdateLocalizedText(_adCountdown);
                _adCountdown--;
                yield return new WaitForSeconds(1.0f);
            }

            StartBackupTimerCoroutine();
            GBGames.ShowInterstitial();
        }

        private void StartBackupTimerCoroutine()
        {
            if (_backupTimerCoroutine != null)
            {
                StopCoroutine(_backupTimerCoroutine);
            }
            _backupTimerCoroutine = StartCoroutine(BackupTimerCoroutine());
        }

        private IEnumerator BackupTimerCoroutine()
        {
            yield return new WaitForSeconds(5f);

            if (_isAdShowing) yield break;
            ResetTimer();
            StartCheckTimerCoroutine();
        }

        private void UpdateLocalizedText(int value)
        {
            var stringReference = localizedText.StringReference;
            if (stringReference["value"] is IntVariable variable)
            {
                variable.Value = value;
            }

            localizedText.RefreshString();
        }
    }
}
