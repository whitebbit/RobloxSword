using System;
using System.Collections.Generic;
using _3._Scripts.Saves;

namespace GBGamesPlugin
{
    [Serializable]
    public class SavesGB
    {
        // Технические сохранения.(Не удалять)
        public int saveID;

        // Ваши сохранения, если вы привыкли пользоваться сохранением через объекты. Можно задать полям значения по умолчанию     
        public bool defaultLoaded;
        
        public SaveHandler<string> characterSaves = new();
        public SaveHandler<string> trailSaves = new();
        public SaveHandler<string> upgradeSaves = new();
        public SaveHandler<string> swordSaves = new();
        public SaveManyHandler<string> petSaves = new();
        public WalletSave walletSave = new();

        public int stageID;
        public bool sound = true;
    }
}