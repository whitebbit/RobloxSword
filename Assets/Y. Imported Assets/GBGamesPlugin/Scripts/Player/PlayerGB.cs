using System;
using System.Collections.Generic;
using InstantGamesBridge;
using InstantGamesBridge.Common;
using InstantGamesBridge.Modules.Device;
using InstantGamesBridge.Modules.Player;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        private static readonly Dictionary<string, string> PlayerWordTranslate = new()
        {
            {"ru", "Игрок"}, {"en", "Player"}, {"fr", "Joueur"}, {"it", "Giocatore"}, {"de", "Spieler"},
            {"es", "Jugador"},
            {"zh", "玩家 "}, {"pt", "Jogador"}, {"ko", "플레이어"}, {"ja", "プレイヤー"}, {"tr", "oyuncu"}, {"ar", "لاعب "},
            {"hi", "खिलाड़ी "}, {"id", "Pemain"},
        };

        /// <summary>
        /// Поддержка авторизации.
        /// </summary>
        public static bool isAuthorizationSupported => Bridge.player.isAuthorizationSupported;

        /// <summary>
        /// Авторизован ли игрок в данный момент.
        /// </summary>
        public static bool isAuthorized => Bridge.player.isAuthorized;

        /// <summary>
        /// ID игрока. Если авторизация поддерживается на платформе и игрок авторизован в данный момент – возвращает его ID на платформе, иначе — null.
        /// </summary>
        public static string playerID => string.IsNullOrEmpty(Bridge.player.id) ? "playerID" : Bridge.player.id;

        /// <summary>
        /// Имя игрока.
        /// </summary>
        public static string playerName =>
            string.IsNullOrEmpty(Bridge.player.name)
                ? PlayerWordTranslate.GetValueOrDefault(language, PlayerWordTranslate["en"])
                : Bridge.player.name;

        public static List<string> playerPhoto => Bridge.player.photos;

        /// <summary>
        /// Авторизация игрока.
        /// </summary>
        public static void AuthorizePlayer(Action onAuthorizePlayerSuccess = null,
            Action onAuthorizePlayerFailed = null, Dictionary<string, object> options = default)
        {
            Message("Authorize Player");
            Bridge.player.Authorize(options, (success) =>
            {
                if (success)
                {
                    onAuthorizePlayerSuccess?.Invoke();
                    Message("Authorize Player Success");
                }
                else
                {
                    onAuthorizePlayerFailed?.Invoke();
                    Message("Authorize Player Failed", LoggerState.error);
                }
            });
        }
    }
}