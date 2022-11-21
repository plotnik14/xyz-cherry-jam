using CherryJam.Components.Health;
using CherryJam.Utils;
using UnityEngine;

namespace CherryJam.Creatures.Mobs
{
    public class EvilFirefly : MonoBehaviour
    {
        [SerializeField] private Cooldown _attackCooldown;

        private ModifyHealthComponent _modifyHealth;
        private Hero.Hero _hero;
        
        private void Awake()
        {
            _hero = FindObjectOfType<Hero.Hero>();
            _modifyHealth = GetComponent<ModifyHealthComponent>();
        }

        private void Update()
        {
            if (_attackCooldown.IsReady)
            {
                _modifyHealth.ApplyHealthChange(_hero.gameObject);
                _attackCooldown.Reset();
            }
        }
    }
}