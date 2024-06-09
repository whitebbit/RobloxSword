using UnityEngine;

namespace GBGamesPlugin
{
    public static class PauseController
    {
        private static bool _audioPause;
        private static float _audioVolume;
        private static float _timeScale = 1;
        private static CursorLockMode _cursorLockMode = CursorLockMode.None;
        private static bool _cursorVisible = true;

        public static void Pause(bool state)
        {
            if (state)
            {
                _audioPause = AudioListener.pause;
                _timeScale = Time.timeScale;
                _cursorLockMode = Cursor.lockState;
                _cursorVisible = Cursor.visible;
                _audioVolume = AudioListener.volume;
                
                AudioListener.volume = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = _timeScale;
                AudioListener.volume = _audioVolume;
                Cursor.lockState = _cursorLockMode;
                Cursor.visible = _cursorVisible;
            }
        }
    }
}