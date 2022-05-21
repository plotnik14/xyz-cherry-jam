using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimationWithClips : MonoBehaviour
    {
        [SerializeField] private int _frameRate;
        [SerializeField] private Clip[] _clips;
        [SerializeField] private UnityEvent _onComplete;

        private SpriteRenderer _renderer;
        private float _secondsPerFrame;
        private Clip _currentClip;
        private int _currentSpriteIndex;
        private int _currentClipIndex;
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
            _currentClip = _clips.Length == 0 
                ? Clip.GetEmptyInstance() 
                : _clips[_currentClipIndex];
            _sprites = _currentClip.GetSprites();
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time) return;

            if (_currentSpriteIndex >= _sprites.Length)
            {
                if (_currentClip.IsLooped())
                {
                    _currentSpriteIndex = 0;
                }
                else
                {
                    _currentClipIndex++;
                    if (_currentClip.IsAllowNextClip() && _currentClipIndex < _clips.Length)
                    {
                        _currentClip = _clips[_currentClipIndex];
                        _currentSpriteIndex = 0;
                        _sprites = _currentClip.GetSprites();
                        return;
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
            UpdateClipByName(name);

        }

        private void UpdateClipByName(string clipName)
        {
            for (int i = 0; i < _sprites.Length; i++)
            {
                if (_clips[i].GetName() == clipName)
                {
                    _currentClipIndex = i;
                    _currentClip = _clips[_currentClipIndex];
                    _currentSpriteIndex = 0;
                    _sprites = _currentClip.GetSprites();
                    return;
                }
            }
        }
    }
}
