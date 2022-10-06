using UnityEngine;
using UnityEngine.InputSystem;

namespace CherryJam.Creatures.Hero
{
    public class InputEnableComponent : MonoBehaviour
    {
        private PlayerInput _input;

        private void Start()
        {
            InitInput();
        }

        public void SetInput(bool isEnabled)
        {
            if (_input == null) 
                InitInput();
            
            _input.enabled = isEnabled;
        }

        private void InitInput()
        {
            _input = FindObjectOfType<PlayerInput>();
        }
    }
}