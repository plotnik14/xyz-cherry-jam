using CherryJam.Components.LevelManagement;
using CherryJam.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace CherryJam.UI.Hud
{
    public class LocationNameController : MonoBehaviour
    {
        [SerializeField] private Text _name;
        [SerializeField] private Animator _animator;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        private static readonly int StartKey = Animator.StringToHash("start");

        private void Start()
        {
            var locationNameComponent = FindObjectOfType<LocationNameComponent>();
            _name.text = locationNameComponent.LocationName;
            _animator.SetTrigger(StartKey);
        }

        private void OnHideFinished()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}