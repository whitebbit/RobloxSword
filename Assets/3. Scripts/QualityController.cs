using System;
using System.Linq;
using GBGamesPlugin;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VInspector;
using DeviceType = InstantGamesBridge.Modules.Device.DeviceType;

namespace _3._Scripts
{
    public class QualityController : MonoBehaviour
    {
        [Tab("Assets")] [SerializeField] private UniversalRenderPipelineAsset pc;
        [SerializeField] private UniversalRenderPipelineAsset mobile;
        [Tab("Components")] [SerializeField] private Light mainLight;
        [SerializeField] private Volume postProcessing;


        private void Start()
        {
            GraphicsSettings.renderPipelineAsset = GBGames.deviceType switch
            {
                DeviceType.Mobile => mobile,
                DeviceType.Desktop => pc,
                DeviceType.Tablet => pc,
                _ => mobile
            };
            mainLight.shadows = GBGames.deviceType switch
            {
                DeviceType.Mobile => LightShadows.None,
                DeviceType.Tablet => LightShadows.Soft,
                DeviceType.Desktop => LightShadows.Soft,
                _ => LightShadows.None
            };
            postProcessing.enabled = GBGames.deviceType switch
            {
                DeviceType.Mobile => false,
                DeviceType.Tablet => true,
                DeviceType.Desktop => true,
                _ => false
            };

            QualitySettings.SetQualityLevel(QualitySettings.names.ToList().IndexOf(GBGames.deviceType switch
            {
                DeviceType.Mobile => "Mobile",
                DeviceType.Tablet => "PC",
                DeviceType.Desktop => "PC",
                _ => "Mobile"
            }));
        }
    }
}