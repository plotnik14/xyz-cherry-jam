using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimationWithClips : MonoBehaviour
    {
        [SerializeField] private int _frameRate;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _allowNext;
        [SerializeField] private Clip[] _clips;
        [SerializeField] private UnityEvent _onComplete;

        private SpriteRenderer _renderer;
        private float _secondsPerFrame;
        private int _currentClipIndex;
        private int _currentSpriteIndex;
        private float _nextFrameTime;
        private Sprite[] _sprites;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time + _secondsPerFrame;
            _currentSpriteIndex = 0;
            _currentClipIndex = 0;
            _sprites = _clips.Length == 0 
                ? new Sprite[0] 
                : _clips[_currentClipIndex].GetSprites();
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time) return;

            if (_currentSpriteIndex >= _sprites.Length)
            {
                if (_loop)
                {
                    _currentSpriteIndex = 0;
                }
                else
                {
                    _currentClipIndex++;
                    if (_allowNext && _currentClipIndex < _clips.Length)
                    {                        
                        _sprites = _clips[_currentClipIndex].GetSprites();
                        _currentSpriteIndex = 0;
                    }
                    else
                    {
                        enabled = false;
                        _onComplete?.Invoke();
                        return;
                    }
                }
            }

            _renderer.sprite = _sprites[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }

        public void SetName(string name)
        {
            // ???
        }
    }
}
