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
        
        public void OnHeal(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.HealWithFirefly();
            }
        }

        public void OnBoostAttack(InputAction.CallbackContext context)
        {
            if (context.started)
                _hero.SetBoostedAttack(true);
            else if (context.canceled)
                _hero.SetBoostedAttack(false);
        }
    }
}