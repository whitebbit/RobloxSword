using System;
using _3._Scripts.Config;
using Cinemachine;
using GBGamesPlugin;
using UnityEngine;
using DeviceType = InstantGamesBridge.Modules.Device.DeviceType;

namespace _3._Scripts
{
    public class CameraSettingsProvider : MonoBehaviour
    {
        [SerializeField] private RemoteConfig<float> mobileXMaxSpeed;
        [SerializeField] private RemoteConfig<float> mobileYMaxSpeed;
        
        private CinemachineFreeLook _camera;
        
        private void Awake()
        {
            _camera = GetComponent<CinemachineFreeLook>();
        }

        private void Start()
        {
            switch (GBGames.deviceType)
            {
                case DeviceType.Mobile:
                    
                    _camera.m_XAxis.m_MaxSpeed = 300;
                    _camera.m_YAxis.m_MaxSpeed = 2;
                    break;
                case DeviceType.Desktop:
                    _camera.m_XAxis.m_MaxSpeed = 400;
                    _camera.m_YAxis.m_MaxSpeed = 2.5f;
                    break;
                default:
                    _camera.m_XAxis.m_MaxSpeed = 400;
                    _camera.m_YAxis.m_MaxSpeed = 2.5f;
                    break;
            }
        }
    }
}