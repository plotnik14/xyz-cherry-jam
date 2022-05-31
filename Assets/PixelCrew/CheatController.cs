using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace PixelCrew
{
    public class CheatController : MonoBehaviour
    {
        [SerializeField] private float _inputTimeToLive;
        [SerializeField] private CheatItem[] _cheats;

        private string _currentInput;
        private float _inputTime;

        private void Awake()
        {
            Keyboard.current.onTextInput += OnTextInput;
        }

        private void OnDestroy()
        {
            Keyboard.current.onTextInput -= OnTextInput;
        }

        private void OnTextInput(char inputChar)
        {
            _currentInput += inputChar;
            _inputTime = _inputTimeToLive;
            FindAnyCheatsAndApply();
        }

        private void FindAnyCheatsAndApply()
        {
            foreach (var cheatItem in _cheats)
            {
                if (_currentInput.Contains(cheatItem.Name))
                {
                    cheatItem.Action.Invoke();
                    _currentInput = String.Empty;
                    return;
                }
            }
        }

        void Update()
        {
            if (_inputTime < 0)
            {
                _currentInput = string.Empty;
            }
            {
                _inputTime -= Time.deltaTime;
            }
        }
    }

    [Serializable]
    public class CheatItem
    {
        [SerializeField] private string _name;
        [SerializeField] private UnityEvent _action;

        public string Name => _name;
        public UnityEvent Action => _action;
    }
}