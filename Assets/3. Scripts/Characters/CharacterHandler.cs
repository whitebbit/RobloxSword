using System.Linq;
using _3._Scripts.Config;
using UnityEngine;

namespace _3._Scripts.Characters
{
    public class CharacterHandler
    {
        private Character _current;

        public void SetCharacter(string id, Transform parent)
        {
            DeleteCharacter();
            SpawnCharacter(id, parent);
        }

        private void SpawnCharacter(string id, Transform parent)
        {
            var character = Configuration.Instance.AllCharacters.FirstOrDefault(c => c.ID == id)?.Prefab;
            _current = Object.Instantiate(character, parent);
            _current.transform.localPosition = -Vector3.up;
        }

        private void DeleteCharacter()
        {
            if (_current == null) return;
            Object.Destroy(_current.gameObject);
        }
    }
}