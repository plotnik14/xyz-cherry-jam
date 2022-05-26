using UnityEngine;

namespace PixelCrew.Components
{
    public class HealthChangeComponent : MonoBehaviour
    {
        [SerializeField] private int _healthPoints;

        public void ApplyHealthChange(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();

            if (healthComponent == null) return;

            if (_healthPoints > 0)
            {
                healthComponent.ApplyHealing(_healthPoints);
            }
            else if (_healthPoints < 0)
            {
                var damage = -_healthPoints;
                healthComponent.ApplyDamage(damage);
            }  
            
        }
    }
}