using PixelCrew.Components.Health;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class BossHpWidget : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private ProgressBarWidget _hpBar;
        [SerializeField] private CanvasGroup _canvas;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private int _maxHealth;

        private void Start()
        {
            _maxHealth = _health.Health;
            _disposable.Retain(_health.OnChange.Subscribe(OnHpChanged));
            _disposable.Retain(_health.OnDie.Subscribe(HideUI));
        }

        [ContextMenu("ShowUI")]
        public void ShowUI()
        {
            this.LerpAnimated(0, 1, 1, SetAlpha);
        }

        [ContextMenu("HideUI")]
        private void HideUI()
        {
            this.LerpAnimated(1, 0, 1, SetAlpha);
        }

        private void SetAlpha(float alpha)
        {
            _canvas.alpha = alpha;
        }
        
        private void OnHpChanged(int hp)
        {
            _hpBar.SetProgress((float) hp / _maxHealth);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}