using System.Collections;
using _3._Scripts.Interactive;
using _3._Scripts.Wallet;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    public class TutorialText : MonoBehaviour
    {
        [SerializeField] private StoneWithSword target;
        
        private void Start()
        {
            target.ONInteractEnd += () =>
            {
                if (!(WalletManager.SecondCurrency <= 10)) return;
                gameObject.SetActive(true);
                StartCoroutine(Active());
            };
            gameObject.SetActive(false);
        }

        private IEnumerator Active()
        {
            yield return new WaitForSeconds(5);
            gameObject.SetActive(false);
        }
    }
}