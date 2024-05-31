using GBGamesPlugin.Editor;
using UnityEditor;
using UnityEngine;

namespace GBGamesPlugin
{
    [CustomEditor(typeof(GBGamesSettings))]
    public class GBGamesSettingsEditor: UnityEditor.Editor
    {
        private bool _savesIsActive;

        private const string NewtonsoftUrl = "com.unity.nuget.newtonsoft-json";

        private const string DefineSaves = "GB_NEWTONSOFT_FOR_SAVES";
        
        private void OnEnable()
        {
            _savesIsActive = DefineSymbols.CheckDefine(DefineSaves);
        }
        
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);
            base.OnInspectorGUI();
            GUILayout.Space(10);

            if (_savesIsActive) return;
            if (!GUILayout.Button("Activate Newtonsoft for saves")) return;
            if (!PackageDownloader.IsPackageImported(NewtonsoftUrl))
            {
                PackageDownloader.DownloadPackage(NewtonsoftUrl);
            }

            DefineSymbols.AddDefine(DefineSaves);
        }
    }
}