using System;
using CherryJam.Creatures.Hero;
using CherryJam.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CherryJam.UI.Hud.Letter
{
    public class LetterController : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Text _text;
        
        private Hero _hero;
        private UnityEvent _onComplete;

        public void Show(string text, UnityEvent onComplete)
        {
            _onComplete = onComplete;
            
            TimeUtils.StopTime();
            
            if (_hero == null)
                _hero = FindObjectOfType<Hero>();
            _hero.LockInput();
            
            _text.text = text;
            _container.SetActive(true);
        }

        public void Hide()
        {
            TimeUtils.ResumeTime();
            
            _container.SetActive(false);
            _text.text = String.Empty;
            
            _hero.UnlockInput();
            
            _onComplete?.Invoke();
        }
    }
}