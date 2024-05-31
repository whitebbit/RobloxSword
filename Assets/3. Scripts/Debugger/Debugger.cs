using System;
using System.Linq;
using _3._Scripts.Config;
using _3._Scripts.Wallet;
using DG.Tweening;
using GBGamesPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using Random = UnityEngine.Random;

namespace _3._Scripts.Debugger
{
    public class Debugger : MonoBehaviour
    {
        [Tab("Enable Button")] [SerializeField]
        private Button enableButton;

        [SerializeField] private Transform enableArrow;
        [Tab("Panel")] [SerializeField] private Transform panel;

        [Tab("Pet")] [SerializeField] private TMP_InputField petInputField;
        [Tab("Trail")] [SerializeField] private TMP_InputField trailInputField;
        [Tab("FPS")] [SerializeField] private TMP_Text fpsText;


        private void Awake()
        {
            SetPanelState(false);
            enableButton.onClick.AddListener(() => SetPanelState(!panel.gameObject.activeSelf));
        }

        private void Update()
        {
            UpdateFPS();
        }

        private void SetPanelState(bool state)
        {
            var rotation = state ? Vector3.zero : new Vector3(0, 0, 180);
            panel.gameObject.SetActive(state);
            enableArrow.transform.eulerAngles = rotation;
        }

        public void Save() => GBGames.instance.Save();
        public void DeleteSaves() => GBGames.Delete();
        public void UnlockPet() => GBGames.saves.petSaves.Unlock(petInputField.text);
        public void UnlockTrail() => GBGames.saves.trailSaves.Unlock(trailInputField.text);
        public void Add1000FirstCurrency() => WalletManager.FirstCurrency += 1000;
        public void Add1000SecondCurrency() => WalletManager.SecondCurrency += 1000;

        public void UnlockRandomTrail()
        {
            var trails = Configuration.Instance.AllTrails.Where(t => !GBGames.saves.trailSaves.Unlocked(t.ID)).ToList();
            if(trails.Count <= 0) return;
            
            var rand = Random.Range(0, trails.Count);
            GBGames.saves.trailSaves.Unlock(trails[rand].ID);
        }
        
        public void UnlockRandomPet()
        {
            var pets = Configuration.Instance.AllPets.Where(t => !GBGames.saves.petSaves.Unlocked(t.ID)).ToList();
            if(pets.Count <= 0) return;

            var rand = Random.Range(0, pets.Count);
            GBGames.saves.petSaves.Unlock(pets[rand].ID);
        }

        private float _deltaTime;

        private void UpdateFPS()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            var fps = 1.0f / _deltaTime;
            fpsText.text = $"{fps:0.} FPS";
        }
    }
}