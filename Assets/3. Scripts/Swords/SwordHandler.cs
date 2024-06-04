using System.Linq;
using _3._Scripts.Config;
using DG.Tweening;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Swords
{
    public class SwordHandler : MonoBehaviour
    {
        private Sword _currentSword;
        public void SetGetState(Sword sword)
        {
            var swordTransform = sword.transform;
            var st = sword.Data.GetState;

            swordTransform.SetParent(transform);
            swordTransform.localPosition = st.position;
            swordTransform.localEulerAngles = st.rotation;
            swordTransform.localScale = st.scale;

            _currentSword = sword;
        }
        
        public void SetHandState(Sword sword, float duration = 0.25f)
        {
            var swordTransform = sword.transform;
            var st = sword.Data.HandState;

            swordTransform.SetParent(transform);
            swordTransform.DOLocalMove(st.position, duration);
            swordTransform.DOLocalRotate(st.rotation, duration);
            swordTransform.DOScale(st.scale, duration);
            
            _currentSword = sword;
        }


        public void DestroyCurrentSword()
        {
            if(_currentSword == null) return;
            Destroy(_currentSword.gameObject);
            _currentSword = null;
        }

        public void CreateCurrentSword()
        {
            var swordID = GBGames.saves.swordSaves.current;
            
            if(string.IsNullOrEmpty(swordID)) return;

            var data = Configuration.Instance.SwordData.FirstOrDefault(s => s.ID == swordID);
            if (data is null) return;
            var sword = Instantiate(data.Sword, transform);
            SetHandState(sword, 0);
        }
    }
}