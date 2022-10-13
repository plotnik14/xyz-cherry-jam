using System;
using CherryJam.Creatures.Hero;
using CherryJam.Utils;
using CherryJam.Utils.Disposables;
using UnityEngine;
using Cinemachine;

namespace CherryJam.Components.LevelManagement
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class FollowCameraController : MonoBehaviour
    {
        [Header("Vision Max Offset")]
        [SerializeField] private float _left;
        [SerializeField] private float _right;
        [SerializeField] private float _up;
        [SerializeField] private float _down;
        
        [Header("Change Offset Params")]
        [SerializeField] private float _timeToChange;
        
        private Hero _hero;
        private CinemachineVirtualCamera _vCamera;
        private CinemachineFramingTransposer _framingTransposer;
        private float _yOriginalOffset;

        private Coroutine _changeX;
        private Coroutine _changeYUp;
        private Coroutine _changeYDown;
        private Coroutine _changeYToOriginal;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private float CurrentXOffset => _framingTransposer.m_TrackedObjectOffset.x;
        private float CurrentYOffset => _framingTransposer.m_TrackedObjectOffset.y;
        private bool HeroDoesntLookUpDown => !_hero.IsLookingUp && !_hero.IsLookingDown;
        private bool OffsetByYIsOriginal => Math.Abs(CurrentYOffset - _yOriginalOffset) < 0.01;
        private bool NeedToResetYOffset => HeroDoesntLookUpDown && !OffsetByYIsOriginal && _changeYToOriginal == null;
        private bool IsWrongYInput => _hero.IsLookingUp && _hero.IsLookingDown;
        private bool NeedToStartLookingUp => _hero.IsLookingUp && _changeYUp == null;
        private bool NeedToStartLookingDown => _hero.IsLookingDown && _changeYDown == null;
        
        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
            _vCamera = GetComponent<CinemachineVirtualCamera>();
            _framingTransposer = _vCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            _vCamera.Follow =_hero.transform;
            _yOriginalOffset = CurrentYOffset;
            
            _disposable.Retain(_hero.SubscribeDirectionChanged(OnDirectionChanged));
        }

        private void OnDirectionChanged()
        {
            DisableCoroutine(ref _changeX);
            var targetOffset = _hero.transform.localScale.x > 0 ? _right : _left;
            _changeX = this.LerpAnimated(CurrentXOffset, targetOffset, _timeToChange, UpdateXOffset);
        }

        private void UpdateXOffset(float value)
        {
            _framingTransposer.m_TrackedObjectOffset.x = value;
        }
        
        private void UpdateYOffset(float value)
        {
            _framingTransposer.m_TrackedObjectOffset.y = value;
        }

        private void LateUpdate()
        {
            if (HeroDoesntLookUpDown && OffsetByYIsOriginal) return;
            if (IsWrongYInput) return;

            if (NeedToResetYOffset)
            {
                DisableCoroutine(ref _changeYUp);
                DisableCoroutine(ref _changeYDown);
                _changeYToOriginal = this.LerpAnimated(CurrentYOffset, _yOriginalOffset, _timeToChange, UpdateYOffset);
                return;
            }

            if (NeedToStartLookingUp)
            {
                DisableCoroutine(ref _changeYToOriginal);
                _changeYUp = this.LerpAnimated(CurrentYOffset, _up, _timeToChange, UpdateYOffset);
                return;
            }
            
            if (NeedToStartLookingDown)
            {
                DisableCoroutine(ref _changeYToOriginal);
                _changeYDown = this.LerpAnimated(CurrentYOffset, _down, _timeToChange, UpdateYOffset);
            }
        }

        private void DisableCoroutine(ref Coroutine coroutine)
        {
            if (coroutine == null) return;
            
            StopCoroutine(coroutine);
            coroutine = null;
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}