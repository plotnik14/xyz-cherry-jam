using PixelCrew.Model;
using PixelCrew.Model.Definition.Player;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private bool _isHeroAttack;
        [SerializeField] private bool _isRangeAttack;
        
        // ToDo hide field if it is Hero attack
        [SerializeField] private int _hpDelta;

        public void ApplyHealthChange(GameObject target)
        {
            if (_isHeroAttack && _isRangeAttack)
                UpdateHpDelta(StatId.RangeDamage);
            
            var healthComponent = target.GetComponent<HealthComponent>();

            if (healthComponent == null) return;

            if (_hpDelta > 0)
            {
                healthComponent.ApplyHealing(_hpDelta);
            }
            else if (_hpDelta < 0)
            {
                var damage = -_hpDelta;
                healthComponent.ApplyDamage(damage);
            }  
        }

        private void UpdateHpDelta(StatId statId)
        {
            var session = FindObjectOfType<GameSession>();
            var damage = session.StatsModel.GetValue(statId);
            _hpDelta = - (int) damage;
        }
    }
}