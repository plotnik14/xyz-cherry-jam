using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _hpDelta;

        public void ApplyHealthChange(GameObject target)
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