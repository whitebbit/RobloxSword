using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using _3._Scripts.Config;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.Localization;
using _3._Scripts.Swords.Scriptable;
using _3._Scripts.UI;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using VInspector;

namespace _3._Scripts.Interactive
{
    public class StoneWithSword : MonoBehaviour, IInteractive
    {
        [Tab("Player")] [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Transform playerPoint;

        [Tab("Sword")] [SerializeField] private SwordData swordData;
        [SerializeField] private SerializableTransform modelTransform;
        [Tab("UI")] [SerializeField]
        private Transform information;

        
        [SerializeField] private LocalizeStringEvent swordName;
        [SerializeField] private LocalizeStringEvent recommendationText;
        [SerializeField] private ComplexityText complexityText;
        
        [SerializeField] private CurrencyCounterEffect counterEffect;
        private Transform _swordModel;
        private Swords.Sword _currentSword;

        private void Awake()
        {
            InitializeSword();
            recommendationText.SetVariable("value",
                WalletManager.ConvertToWallet((int) Mathf.Ceil(swordData.EnemyData.Strength * 1.5f)));
        }

        private void Start()
        {
            complexityText.SetComplexity(swordData.Type);

        }

        private void InitializeSword()
        {
            _swordModel = Instantiate(swordData.Sword, transform).transform;

            _swordModel.localPosition = modelTransform.position;
            _swordModel.localEulerAngles = modelTransform.rotation;
            _swordModel.localScale = modelTransform.scale;

            swordName.SetReference(swordData.EnemyData.LocalizeID);
        }

        public void Interact()
        {
            _currentSword = Instantiate(swordData.Sword, transform);
            _swordModel.gameObject.SetActive(false);
            information.gameObject.SetActive(false);
          

            InitializePlayer();
            InitializeUI();

            CameraController.Instance.SwapTo(virtualCamera);
        }

        public void StopInteract()
        {
            // Add any necessary cleanup logic here
        }

        private void InitializeUI()
        {
            var panel = UIManager.Instance.GetPanel<MiniGamePanel>();
            var buttonsPanel = UIManager.Instance.GetPanel<ButtonsPanel>();

            buttonsPanel.Enabled = false;
            panel.Enabled = true;

            panel.OnLose += OnLose;
            panel.OnWin += OnWin;

            panel.StartGame(Player.Player.Instance.Strength(), swordData.EnemyData);
        }

        private void InitializePlayer()
        {
            var player = Player.Player.Instance;

            player.Teleport(playerPoint.position);
            player.Animator.SetTrigger("TakeSword");
            player.transform.DOLookAt(transform.position, 0, AxisConstraint.Y);

            player.SwordHandler.DestroyCurrentSword();
            player.SwordHandler.SetGetState(_currentSword);
        }

        private void OnLose()
        {
            var player = Player.Player.Instance;
            var buttonsPanel = UIManager.Instance.GetPanel<ButtonsPanel>();


            buttonsPanel.Enabled = true;
            player.Animator.SetTrigger("LoseSword");

            player.SwordHandler.DestroyCurrentSword();
            player.SwordHandler.CreateCurrentSword();

            _swordModel.gameObject.SetActive(true);
            information.gameObject.SetActive(true);

            UIManager.Instance.GetPanel<MiniGamePanel>().Enabled = false;
            CameraController.Instance.SwapToMain();
        }

        private void OnWin()
        {
            var player = Player.Player.Instance;

            player.Animator.SetTrigger("GetSword");
            player.SwordHandler.SetHandState(_currentSword);
            _currentSword = null;

            StartCoroutine(DelayWin(player));
        }

        private void SetSwordAfterWin(Player.Player player)
        {
            var currentSword = Configuration.Instance.SwordData
                .FirstOrDefault(s => s.ID == GBGames.saves.swordSaves.current);

            if (currentSword == null || swordData.EnemyData.Strength > currentSword.EnemyData.Strength)
            {
                GBGames.saves.swordSaves.Unlock(swordData.ID);
                GBGames.saves.swordSaves.SetCurrent(swordData.ID);
                GBGames.instance.Save();
            }
            else
            {
                player.SwordHandler.DestroyCurrentSword();
                player.SwordHandler.CreateCurrentSword();
            }
        }

        private IEnumerator DelayWin(Player.Player player)
        {
            var panel = UIManager.Instance.GetPanel<MiniGamePanel>();
            var buttonsPanel = UIManager.Instance.GetPanel<ButtonsPanel>();

            yield return new WaitForSeconds(2.5f);

            buttonsPanel.Enabled = true;
            panel.Enabled = false;

            _swordModel.gameObject.SetActive(true);
            information.gameObject.SetActive(true);

            CameraController.Instance.SwapToMain();
            EffectPanel.Instance.SpawnEffect(counterEffect).Initialize(CurrencyType.Second, swordData.EnemyData.Cups);
            WalletManager.SecondCurrency += swordData.EnemyData.Cups;

            SetSwordAfterWin(player);
        }
    }
}