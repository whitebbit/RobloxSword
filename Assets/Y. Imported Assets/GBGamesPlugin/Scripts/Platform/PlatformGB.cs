using System;
using InstantGamesBridge;
using InstantGamesBridge.Common;
using InstantGamesBridge.Modules.Platform;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        /// <summary>
        /// Возвращает идентификатор платформы, на которой в данный момент запущена игра. Возможные значения: VK, Yandex, CrazyGames, AbsoluteGames, GameDistribution, VKPlay, Mock.
        /// </summary>
        public static string platform => Bridge.platform.id;

        /// <summary>
        /// Если платформа предоставляет данные об языке пользователя — то это будет язык, который установлен у пользователя на платформе. Если не предоставляет — это будет язык браузера пользователя. Формат: ISO 639-1. Пример: ru, en.
        /// </summary>
        public static string language => Bridge.platform.language;

        /// <summary>
        /// С помощью данного параметра можно в ссылку на игру встраивать какую-либо вспомогательную информацию.
        /// </summary>
        public static string payload => Bridge.platform.payload;

        /// <summary>
        /// Информация о домене.
        /// </summary>
        public static string domain
        {
            get
            {
                return platform switch
                {
                    "playgama" => "com",
                    "vk" => "com",
                    "yandex" => Bridge.platform.tld,
                    "crazy_games" => "com",
                    "absolute_games" => "com",
                    "game_distribution" => "com",
                    "vk_play" => "ru",
                    "mock" => "ru",
                    _ => ""
                };
            }
        }

        /// <summary>
        /// Игра загрузилась, все загрузочные экраны прошли, игрок может взаимодействовать с игрой. Yandex
        /// </summary>
        public static void GameReady()
        {
            Bridge.platform.SendMessage(PlatformMessage.GameReady);
            Message($"GameReady");
        }

        /// <summary>
        /// Началась какая-либо загрузка внутри игры. Например, когда идёт загрузка уровня. CrazyGames
        /// </summary>
        public static void InGameLoadingStarted()
        {
            Bridge.platform.SendMessage(PlatformMessage.InGameLoadingStarted);
            Message($"InGameLoadingStarted");
        }

        /// <summary>
        /// Загрузка внутри игры окончена. CrazyGames
        /// </summary>
        public static void InGameLoadingStopped()
        {
            Bridge.platform.SendMessage(PlatformMessage.InGameLoadingStopped);
            Message($"InGameLoadingStopped");
        }

        /// <summary>
        /// Начался геймплей. Например, игрок зашёл в уровень с главного меню. CrazyGames
        /// </summary>
        public static void GameplayStarted()
        {
            Bridge.platform.SendMessage(PlatformMessage.GameplayStarted);
            Message($"GameplayStarted");
        }

        /// <summary>
        /// Геймплей закончился/приостановился. Например, при выходе с уровня в главное меню, открытии меню паузы и т.д. CrazyGames
        /// </summary>
        public static void GameplayStopped()
        {
            Bridge.platform.SendMessage(PlatformMessage.GameplayStarted);
            Message($"GameplayStopped");
        }

        /// <summary>
        /// Игрок достиг особенного момента. Например, победил босса, установил рекорд и т.д. CrazyGames
        /// </summary>
        public static void PlayerGotAchievement()
        {
            Bridge.platform.SendMessage(PlatformMessage.PlayerGotAchievement);
            Message($"PlayerGotAchievement");
        }
    }
}