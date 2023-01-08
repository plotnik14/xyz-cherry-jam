using UnityEngine;

namespace CherryJam.Components.Audio
{
    public class PlayMusic : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioSource _source;
        [SerializeField] private bool _playOnStart = true;
        
        private void Start()
        {
            if (_source == null)
                _source = AudioManager.Instance.Music;
            
            if (_playOnStart)
                PlayIfNew();
        }

        public void Play()
        {
            _source.clip = _clip;
            _source.Play();
        }

        private void PlayIfNew()
        {
            if (_source.clip == _clip) return;
            Play();
        }
    }
}
