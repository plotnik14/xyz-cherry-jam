using PixelCrew.Components;
using PixelCrew.UI.Widgets;
using UnityEngine;

namespace PixelCrew.UI.Hud
{
    public class CreatureHudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _heathBar;

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
            _heathBar.SetProgress(barValue);
        }

        private void OnDestroy()
        {
            _health.OnChange.RemoveListener(OnHealthChanged);
        }
    }
}
