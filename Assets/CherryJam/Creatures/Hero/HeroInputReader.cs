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
                _hero.MeleeAttack();
            }
        }
        
        public void OnRangeAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var mousePosition = Mouse.current.position.ReadValue();
                var target = Camera.main.ScreenToWorldPoint(mousePosition);
                _hero.RangeAttack(target);
            }
        }

        public void OnShowMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.ShowMainMenuInGame();
            }
        }
        
        public void OnHeal(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.Heal();
            }
        }

        public void OnBoostAttack(InputAction.CallbackContext context)
        {
            if (context.started)
                _hero.SetBoostedAttack(true);
            else if (context.canceled)
                _hero.SetBoostedAttack(false);
        }

        public void OnLookingUp(InputAction.CallbackContext context)
        {
            if (context.started)
                _hero.IsLookingUp = true;
            else if (context.canceled)
                _hero.IsLookingUp = false;
        }
        
        public void OnLookingDown(InputAction.CallbackContext context)
        {
            if (context.started)
                _hero.IsLookingDown = true;
            else if (context.canceled)
                _hero.IsLookingDown = false;
        }
    }
}