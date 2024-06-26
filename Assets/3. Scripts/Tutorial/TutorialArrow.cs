using System;
using _3._Scripts.Interactive;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Tutorial
{
    public class TutorialArrow : MonoBehaviour
    {
        [SerializeField] private StoneWithSword target;


        private void Start()
        {
            target.ONInteract += () => gameObject.SetActive(false);

            Initialize();
        }

        private void Initialize()
        {
            if(!string.IsNullOrEmpty(GBGames.saves.swordSaves.current))
                gameObject.SetActive(false);
        }

        private void Update()
        {
            transform.LookAt(target.transform.position);
        }
    }
}