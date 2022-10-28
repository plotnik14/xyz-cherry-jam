using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Video;

namespace CherryJam.Components.CutScenes.VideoCutScene
{
    public class VideoCutSceneController : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private UnityEvent _onVideoFinished;

        private void Start()
        {
            _videoPlayer.loopPointReached += OnVideoEndAction;
            _videoPlayer.Play();
        }

        private void OnVideoEndAction(VideoPlayer source)
        {
            _onVideoFinished?.Invoke();
        }
        
        public void OnSkipCutScene(InputAction.CallbackContext context)
        {
            if (context.performed)
                SkipCutScene();
        }

        private void SkipCutScene()
        {
            if (_videoPlayer.isPlaying)
            {
                _videoPlayer.Stop();
                _onVideoFinished?.Invoke();
            }
        }
    }
}