using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Config
{
    public class Tester : MonoBehaviour
    {
        [SerializeField] private string petID;
        [SerializeField] private string trailID;

        [Button]
        public void UnlockPet()
        {
            GBGames.saves.petSaves.Unlock(petID);
        }
        [Button]
        public void UnlockTrail()
        {
            GBGames.saves.trailSaves.Unlock(trailID);
        }
        [Button]
        public void Delete()
        {
            GBGames.Delete();
        }
        
        
    }
}