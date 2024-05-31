using InstantGamesBridge;
using InstantGamesBridge.Modules.Device;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        /// <summary>
        /// Возвращает тип девайса, с которого пользователь запустил игру. Возможные значения: Mobile, Tablet, Desktop, TV.
        /// </summary>
        public static DeviceType deviceType => Bridge.device.type;
    }
}