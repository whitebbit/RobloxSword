using System;
using System.Linq;
using _3._Scripts.Boosters;
using _3._Scripts.Characters;
using _3._Scripts.Config;
using _3._Scripts.Environment;
using _3._Scripts.Pets;
using _3._Scripts.Singleton;
using _3._Scripts.Swords;
using _3._Scripts.Trails;
using _3._Scripts.Upgrades;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Player
{
    public class Player : Singleton<Player>
    {
        [SerializeField] private TrailRenderer trail;

        public PetsHandler PetsHandler { get; private set; }
        public TrailHandler TrailHandler { get; private set; }
        public CharacterHandler CharacterHandler { get; private set; }
        public UpgradeHandler UpgradeHandler { get; private set; }
        public PlayerAnimator Animator { get; private set; }

        public SwordHandler SwordHandler { get; private set; }
        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            Animator = GetComponent<PlayerAnimator>();
            PetsHandler = new PetsHandler();
            CharacterHandler = new CharacterHandler();
            UpgradeHandler = new UpgradeHandler();
            TrailHandler = new TrailHandler(GetComponent<PlayerMovement>(), trail);
        }

        private void Start()
        {
            InitializeCharacter();
            InitializeTrail();
            InitializePets();
            InitializeSword();

            Teleport(StageController.Instance.CurrentStage.SpawnPoint.position);
        }

        public float Strength()
        {
            var x2 = BoostersHandler.Instance.X2Currency ? 2 : 1;

            var strength = WalletManager.FirstCurrency;
            var result = strength;

            return result * x2;
        }

        public float TrainingStrength()
        {
            var pets = Configuration.Instance.AllPets.Where(p => GBGames.saves.petSaves.IsCurrent(p.ID)).ToList();
            var sword = Configuration.Instance.SwordData.FirstOrDefault(s => s.ID == GBGames.saves.swordSaves.current);
            var swordStrength = sword == null ? 0 : sword.StrengthBooster;
            var character =
                Configuration.Instance.AllCharacters.FirstOrDefault(c => GBGames.saves.characterSaves.IsCurrent(c.ID));
            var booster = BoostersHandler.Instance.X2Income ? 2 : 1;

            return (character.Booster + swordStrength + (character.Booster * pets.Sum(pet => pet.Booster) / 100)) *
                   booster;
        }


        public void SetSwordPoint(SwordHandler handler) => SwordHandler = handler;

        public void Teleport(Vector3 position)
        {
            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
        }

        private void InitializeSword()
        {
            SwordHandler.CreateCurrentSword();
        }

        private void InitializeCharacter()
        {
            var id = GBGames.saves.characterSaves.current;
            CharacterHandler.SetCharacter(id, transform);
        }

        private void InitializeTrail()
        {
            var id = GBGames.saves.trailSaves.current;
            TrailHandler.SetTrail(id);
        }

        private void InitializePets()
        {
            var player = transform;
            var position = player.position + player.right * 2;
            foreach (var data in GBGames.saves.petSaves.current.Select(id =>
                Configuration.Instance.AllPets.FirstOrDefault(p => p.ID == id)))
            {
                PetsHandler.CreatePet(data, position);
            }
        }
    }
}