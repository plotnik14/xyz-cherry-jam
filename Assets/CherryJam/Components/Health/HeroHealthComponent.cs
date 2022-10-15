using System.Collections;
using CherryJam.Model.Definition;
using UnityEngine;

namespace CherryJam.Components.Health
{
    public class HeroHealthComponent : HealthComponent
    {
        [SerializeField] private float _timeInvincibleAfterDamage;

        private Coroutine _invincibleCoroutine;
        
        protected override void Start()
        {
            _isInvincible = false;
            UpdateMaxHealth();
            _onDamage.AddListener(StartInvincibleTime);
        }

        private void StartInvincibleTime()
        {
            if (_invincibleCoroutine == null)
                StartCoroutine(InvincibleCoroutine());
        }

        private IEnumerator InvincibleCoroutine()
        {
            MakeInvincible();
            yield return new WaitForSeconds(_timeInvincibleAfterDamage);
            MakeVulnerable();
            _invincibleCoroutine = null;
        }

        public void SetHealth(int health)
        {
            _health = health;
            UpdateMaxHealth();
        }

        private void UpdateMaxHealth()
        {
            _maxHealth = DefsFacade.I.Player.MaxHP;
        }

        private void OnDestroy()
        {
            _onDamage.RemoveListener(StartInvincibleTime);
        }
    }
}