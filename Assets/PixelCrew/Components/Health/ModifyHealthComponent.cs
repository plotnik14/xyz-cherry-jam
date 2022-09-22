using PixelCrew.Model;
using PixelCrew.Model.Definition.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PixelCrew.Components.Health
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private bool _isHeroAttack;
        [SerializeField] private bool _isRangeAttack;
        
        // ToDo hide field if it is Hero attack
        [SerializeField] private int _hpDelta;

        private GameSession _session;
        
        private void Start()
        {
            if (_isHeroAttack)
                _session = GameSession.Instance;
        }

        public virtual void ApplyHealthChange(GameObject target)
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
                var damage = -_hpDelta * CalculateCriticalDamageModifier();
                healthComponent.ApplyDamage(damage);
            }  
        }

        private void UpdateHpDelta(StatId statId)
        {
            var damage = _session.StatsModel.GetValue(statId);
            _hpDelta = - (int) damage;
        }

        private int CalculateCriticalDamageModifier()
        {
            if (!_isHeroAttack) return 1;

            return IsCriticalAttack() ? 2 : 1;
        }

        private bool IsCriticalAttack()
        {
            var attackType = _isRangeAttack ? StatId.RangeDamageCritical : StatId.MeleeDamageCritical;
            var criticalAttackChance = _session.StatsModel.GetValue(attackType);
            var maxChance = 100;
            var chanceRoll = Random.Range(1, maxChance + 1);
            var isCriticalAttack = criticalAttackChance >= chanceRoll;

            // if (isCriticalAttack)
            // {
            //     Debug.Log($"Critical Attack! Damage x2. (Chance:{criticalAttackChance} Roll:{chanceRoll})");
            // }
            
            return isCriticalAttack;
        }
    }
}