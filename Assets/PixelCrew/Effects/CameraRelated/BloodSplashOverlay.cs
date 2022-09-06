using PixelCrew.Model;
using PixelCrew.Model.Definition.Player;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Effects.CameraRelated
{
    [RequireComponent(typeof(Animator))]
    public class BloodSplashOverlay : MonoBehaviour
    {
        [SerializeField] private Transform _overlay;
        
        private static readonly int Health = Animator.StringToHash("Health");

        private Animator _animator;
        private Vector3 _overScale;
        private GameSession _session;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _overScale = _overlay.localScale - Vector3.one;

            _session = FindObjectOfType<GameSession>();
            _disposable.Retain(_session.Data.Hp.SubscribeAndInvoke(OnHpChanged));
        }

        private void OnHpChanged(int newValue, int _)
        {
            var maxHp = _session.StatsModel.GetValue(StatId.Hp);
            var hpNormalized = newValue / maxHp;
            _animator.SetFloat(Health, hpNormalized);

            var overlayModifier = Mathf.Max(hpNormalized - 0.3f, 0f);
            _overlay.localScale = Vector3.one + _overScale * overlayModifier;
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}