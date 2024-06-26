using System.Collections;
using _3._Scripts.Currency.Enums;
using _3._Scripts.Enemies;
using _3._Scripts.Inputs;
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
    public class BattleArena : MonoBehaviour
    {
        [Tab("Components")]
        [SerializeField] private CinemachineVirtualCamera cam;

        [Tab("Positions")] [SerializeField]
        private Transform centerPoint;
        
        [Header("Enemy")]
        [SerializeField] private Transform enemyPoint;

        
        [Header("Player")]
        [SerializeField] private Transform playerPoint;

        [Tab("UI")]
        [SerializeField] private CurrencyCounterEffect counterEffect;

        private Enemy _enemy;
        private Player.Player _player;
        private MiniGamePanel _miniGamePanel;
        private ButtonsPanel _buttonsPanel;

        private void Start()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _player = Player.Player.Instance;
            _miniGamePanel = UIManager.Instance.GetPanel<MiniGamePanel>();
        }

        public void StartBattle(Enemy enemy)
        {
            _enemy = enemy;
            TeleportPlayer();
            SetupEnemy();
            InitializeUI();
            CameraController.Instance.SwapTo(cam);

        }

        private void TeleportPlayer()
        {
            _player.Teleport(playerPoint.position);
            _player.transform.DOLookAt(centerPoint.position, 0.1f, AxisConstraint.Y);
        }

        private void SetupEnemy()
        {
            var eTransform = _enemy.transform;
            eTransform.position = enemyPoint.position;
            eTransform.DOLookAt(centerPoint.position, 0.1f, AxisConstraint.Y);
        }

        private void InitializeUI()
        {
            //_buttonsPanel.Enabled = false;
            _miniGamePanel.Enabled = true;

            _miniGamePanel.OnLose += HandleLose;
            _miniGamePanel.OnWin += HandleWin;

            _miniGamePanel.StartGame(_player.Strength(), _enemy.Data);
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
            var winnerAnimator = playerWon ? _player.Animator : _enemy.Animator;
            var loserAnimator = playerWon ? _enemy.Animator : _player.Animator;

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
                WalletManager.SecondCurrency += _enemy.Data.CurrentCups;
                EffectPanel.Instance.SpawnEffect(counterEffect).Initialize(CurrencyType.Second, _enemy.Data.CurrentCups);
            }

            _enemy.TeleportToStart();
            
            _miniGamePanel.Enabled = false;
           // _buttonsPanel.Enabled = true;

            CameraController.Instance.SwapToMain();
        }
    }
}
