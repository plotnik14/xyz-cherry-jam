using System.Collections;
using System.Collections.Generic;
using CherryJam.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CherryJam.Components.CutScenes.ArtCutScene
{
    public class ArtCutSceneController : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _pictures;
        [SerializeField] private float _delay;
        [SerializeField] private UnityEvent _beforeStart;
        [SerializeField] private UnityEvent _afterFinish;
        [SerializeField] private AudioClip _sound;
        
        private AudioSource _source;

        private ArtCutSceneBox _artCutScene;
        private Image _image;
        private bool _skipCutScene;
        
        private void Start()
        {
            _beforeStart?.Invoke();
            StartCoroutine(ShowCutScene());
        }

        private IEnumerator ShowCutScene()
        {
            yield return Init();
            Play();

            _image.gameObject.SetActive(true);
            
            foreach (var picture in _pictures)
            {
                if (_skipCutScene) break;

                _image.sprite = picture;
                yield return new WaitForSeconds(_delay);
            }
            
            _afterFinish?.Invoke();
        }

        private IEnumerator Init()
        {
            while (_artCutScene == null)
            {
                _artCutScene = FindObjectOfType<ArtCutSceneBox>();
                yield return null;
            }
            
            _image = _artCutScene.Image;
        }
        
        private void Play()
        {
            if (_source == null)
                _source = AudioUtils.FindSfxSource();
            
            if (_sound != null)
                _source.PlayOneShot(_sound);
            
        }
        
        public void OnSkipCutScene(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _skipCutScene = true;
            }
        }
    }
}