using UnityEngine;
using UnityEngine.InputSystem;

namespace CherryJam.Creatures.Hero
{
    public class InputEnableComponent : MonoBehaviour
    {
        private PlayerInput _input;
        private GameObject _hero;
        
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
            if (_hero != null) return;
            
            _hero = GameObject.FindWithTag("Player");
            _input = _hero.GetComponent<PlayerInput>();
        }
    }
}