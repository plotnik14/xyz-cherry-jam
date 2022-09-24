using CherryJam.Components.Health;
using CherryJam.UI.Widgets;
using UnityEngine;

namespace CherryJam.UI.Hud
{
    public class CreatureHudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;

        private HealthComponent _health;

        public void Start()
        {
            _health = GetComponentInParent<HealthComponent>();
            _health.OnChange.AddListener(OnHealthChanged);
        }

        public void Update()
        {
            // Do not flip the health bar
            transform.localScale = new Vector3(
                transform.parent.transform.localScale.x, 
                transform.localScale.y, 
                transform.localScale.z
            );
        }

        private void OnHealthChanged(int newValue)
        {
            var barValue = (float)newValue / _health.MaxHealth;
            _healthBar.SetProgress(barValue);
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.OnChange.RemoveListener(OnHealthChanged);
            }
        }
    }
}
