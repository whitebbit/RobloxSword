using System;
using UnityEngine;
using UnityEngine.UI;

namespace _3._Scripts.Sounds
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => SoundManager.Instance.PlayOneShot("click"));
        }
    }
}