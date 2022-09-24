using UnityEngine;
using UnityEngine.InputSystem;

namespace CherryJam.Creatures.Hero
{

    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Interact();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Attack();
            }
        }
        
        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Throw(context.duration);
            }
        }

        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.NextItem();
            }
        }
        
        public void OnUseItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.UseItem();
            }
        }

        public void OnShowMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.ShowMainMenuInGame();
            }
        }

        public void OnUseMagic(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.UseMagic();
            }
        }
        
        public void OnLight(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.SwitchLight();
            }
        }
    }
}