using System;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] protected int _health;
        [SerializeField] private UnityEvent _onHealing;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private HealthChangeEvent _onChange;

        protected int _maxHealth;
        protected bool _isInvincible;

        public bool IsInvincible => _isInvincible;

        public HealthChangeEvent OnChange => _onChange;
        public UnityEvent OnDie => _onDie;
        public int Health => _health;
        public int MaxHealth => _maxHealth;


        protected virtual void Start()
        {
            _isInvincible = false;
            _maxHealth = _health;
        }

        public void ApplyDamage(int damageValue)
        {
            if (_isInvincible) return;
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
            _health = Mathf.Min(_health, _maxHealth);

            _onChange?.Invoke(_health);
            _onHealing?.Invoke();
        }

        public void MakeInvincible()
        {
            _isInvincible = true;
        }

        public void MakeVulnerable()
        {
            _isInvincible = false;
        }

#if UNITY_EDITOR
        [ContextMenu("Update Health")]
        public void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }
#endif
        
        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {
        }
    }
}