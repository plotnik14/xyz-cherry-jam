using PixelCrew.Model;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onHealing;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private HealthChangeEvent _onChange;

        private int _maxHealth;


        public HealthChangeEvent OnChange => _onChange;
        public UnityEvent OnDie => _onDie;
        public int Health => _health;
        public int MaxHealth => _maxHealth;


        public void Start()
        {
            _maxHealth = _health;
        }

        public void ApplyDamage(int damageValue)
        {
            if (_health <= 0) return;
            
            _health -= damageValue;

            _onChange?.Invoke(_health);
            _onDamage?.Invoke();

            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        public void ApplyHealing(int healingValue)
        {
            _health += healingValue;
            _health = Mathf.Min(_health, DefsFacade.I.Player.MaxHealth);

            _onChange?.Invoke(_health);
            _onHealing?.Invoke();
        }

#if UNITY_EDITOR
        [ContextMenu("Update Health")]
        public void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }
#endif

        public void SetHealth(int health)
        {
            _health = health;
        }

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {
        }
    }
}