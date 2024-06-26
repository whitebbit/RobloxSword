using System;
using InstantGamesBridge;
using InstantGamesBridge.Common;
using InstantGamesBridge.Modules.Game;
using InstantGamesBridge.Modules.Platform;
using UnityEngine;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        /// <summary>
        /// Возвращает текущее состояние видимости игры (вкладки с игрой). Возможные значения: visible, hidden.
        /// </summary>
        public static VisibilityState visibilityState => Bridge.game.visibilityState;

        public static event Action GameVisibleStateCallback;
        public static event Action GameHiddenStateCallback;
        
        private static void OnGameVisibilityStateChanged(VisibilityState state)
        {
            switch (state)
            {
                case VisibilityState.Visible:
                    Message("Visibility state - Visible");
                    GameVisibleStateCallback?.Invoke();
                    GameplayStarted();
                    break;
                case VisibilityState.Hidden:
                    Message("Visibility state - Hidden");
                    GameHiddenStateCallback?.Invoke();
                    GameplayStopped();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}