using System;
using _3._Scripts.Config;
using _3._Scripts.Localization;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace _3._Scripts
{
    public class AdTimerText: MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<LocalizeStringEvent>().SetVariable("time", RemoteConfiguration.BoostTime / 60f);
        }
    }
}