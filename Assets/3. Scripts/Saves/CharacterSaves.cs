using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class CharacterSaves
    {
        public string currentCharacter = "default_character";
        public List<string> unlockedCharacters = new();

        public bool IsCurrentCharacter(string id) => currentCharacter == id;
        public void SetCurrentCharacter(string id) => currentCharacter = id;
        
        public void UnlockCharacter(string id)
        {
            if (CharacterUnlocked(id)) return;
            
            unlockedCharacters.Add(id);
        }

        public bool CharacterUnlocked(string id) => unlockedCharacters.Contains(id);
    }
}