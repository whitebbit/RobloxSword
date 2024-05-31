using _3._Scripts.Singleton;
using _3._Scripts.Sounds.Scriptable;
using UnityEngine;

namespace _3._Scripts.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager: Singleton<SoundManager>
    {        
        [SerializeField] private SoundDatabase soundDatabase;

        private AudioSource _source;
        protected override void OnAwake()
        {
            base.OnAwake();
            _source = GetComponent<AudioSource>();
        }

        public void PlayOneShot(string id)
        {
            var clip = soundDatabase.GetSound(id);
            _source.PlayOneShot(clip);
        }
    }
}