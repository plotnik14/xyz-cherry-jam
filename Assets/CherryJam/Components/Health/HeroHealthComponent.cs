using CherryJam.Model;
using CherryJam.Model.Definition;
using CherryJam.Model.Definition.Player;

namespace CherryJam.Components.Health
{
    public class HeroHealthComponent : HealthComponent
    {
        protected override void Start()
        {
            _isInvincible = false;
            UpdateMaxHealth();
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
    }
}