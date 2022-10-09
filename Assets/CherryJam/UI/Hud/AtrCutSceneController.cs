using System.Collections;
using System.Collections.Generic;
using CherryJam.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CherryJam.UI.Hud
{
    public class AtrCutSceneController : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _pictures;
        [SerializeField] private float _delay;
        [SerializeField] private UnityEvent _beforeStart;
        [SerializeField] private UnityEvent _afterFinish;
        [SerializeField] private AudioClip _sound;
        
        private AudioSource _source;

        private AtrCutSceneBox _atrCutScene;
        private Image _image;
        
        private void Start()
        {
            _beforeStart?.Invoke();
            StartCoroutine(ShowCutScene());
        }

        private IEnumerator ShowCutScene()
        {
            yield return Init();
            
            Play();

            foreach (var picture in _pictures)
            {
                if (!_image.gameObject.activeSelf) _image.gameObject.SetActive(true);
                
                _image.sprite = picture;
                yield return new WaitForSeconds(_delay);
            }
            
            // _image.gameObject.SetActive(false);
            _afterFinish?.Invoke();
        }

        private IEnumerator Init()
        {
            while (_atrCutScene == null)
            {
                _atrCutScene = GameObject.FindObjectOfType<AtrCutSceneBox>();
                yield return null;
            }
            
            _image = _atrCutScene.Image;
        }
        
        public void Play()
        {
            if (_source == null)
            {
                _source = AudioUtils.FindSfxSource();
            }

            if (_sound != null)
            {
                _source.PlayOneShot(_sound);
            }
        }
    }
}