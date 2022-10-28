using CherryJam.Model;
using UnityEngine;

namespace CherryJam.Components.Health
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _hpDelta;

        private GameSession _session;
        
        public virtual void ApplyHealthChange(GameObject target)
        {
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
    }
}