using System.Collections;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Enemies;
using _3._Scripts.Interactive.Interfaces;
using _3._Scripts.Player;
using _3._Scripts.UI;
using _3._Scripts.UI.Effects;
using _3._Scripts.UI.Panels;
using _3._Scripts.Wallet;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Interactive
{
    public class BattleArena : MonoBehaviour, IInteractive
    {
        [Tab("Components")]
        [SerializeField] private CinemachineVirtualCamera cam;
        [SerializeField] private Enemy enemy;

        [Tab("Positions")]
        [SerializeField] private SerializableTransform enemyTransform;
        [SerializeField] private SerializableTransform playerTransform;

        [Tab("UI")]
        [SerializeField] private CurrencyCounterEffect counterEffect;

        private Player.Player _player;
        private MiniGamePanel _miniGamePanel;
        private ButtonsPanel _buttonsPanel;

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _player = Player.Player.Instance;
            _miniGamePanel = UIManager.Instance.GetPanel<MiniGamePanel>();
            _buttonsPanel = UIManager.Instance.GetPanel<ButtonsPanel>();
        }

        public void Interact()
        {
            TeleportPlayer();
            SetupEnemy();
            InitializeUI();
            CameraController.Instance.SwapTo(cam);
        }

        private void TeleportPlayer()
        {
            _player.Teleport(playerTransform.position);
            _player.transform.DORotate(playerTransform.rotation, 0.1f);
        }

        private void SetupEnemy()
        {
            var eTransform = enemy.transform;
            eTransform.position = enemyTransform.position;
            eTransform.eulerAngles = enemyTransform.rotation;
        }

        private void InitializeUI()
        {
            _buttonsPanel.Enabled = false;
            _miniGamePanel.Enabled = true;

            _miniGamePanel.OnLose += HandleLose;
            _miniGamePanel.OnWin += HandleWin;

            _miniGamePanel.StartGame(_player.Strength(), enemy.Data);
        }

        private void HandleWin()
        {
            EndGame(true);
        }

        private void HandleLose()
        {
            EndGame(false);
        }

        private void EndGame(bool playerWon)
        {
            var winnerAnimator = playerWon ? _player.Animator : enemy.Animator;
            var loserAnimator = playerWon ? enemy.Animator : _player.Animator;

            StartCoroutine(ExecuteBattleAnimations(winnerAnimator, loserAnimator));
            StartCoroutine(DelayedEnd(playerWon));
        }

        private IEnumerator ExecuteBattleAnimations(PlayerAnimator winnerAnimator, PlayerAnimator loserAnimator)
        {
            winnerAnimator.SetTrigger("MegaAttack");
            yield return new WaitForSeconds(0.65f);
            loserAnimator.SetBool("IsDead", true);
            yield return new WaitForSeconds(4);
            loserAnimator.SetBool("IsDead", false);
        }

        private IEnumerator DelayedEnd(bool playerWon)
        {
            yield return new WaitForSeconds(4.65f);

            if (playerWon)
            {
                WalletManager.SecondCurrency += enemy.Data.Cups;
                EffectPanel.Instance.SpawnEffect(counterEffect).Initialize(CurrencyType.Second, enemy.Data.Cups);
            }

            enemy.TeleportToStart();

            _miniGamePanel.Enabled = false;
            _buttonsPanel.Enabled = true;

            CameraController.Instance.SwapToMain();
        }

        public void StopInteract()
        {
            // Implementation for StopInteract if needed
        }
    }
}
