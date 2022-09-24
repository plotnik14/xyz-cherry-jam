using System.Collections.Generic;
using CherryJam.Utils.Disposables;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.GameplayTrigger
{
    public class GameplayTriggersGroupComponent : MonoBehaviour
    {
        [SerializeField] private List<GameplayTriggerComponent> _triggers;
        [SerializeField] private UnityEvent _onAllActivated;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        private void Start()
        {
            foreach (var trigger in _triggers)
            {
                _disposable.Retain(trigger.Subscribe(OnTriggerActivated));
            }
        }

        private void OnTriggerActivated()
        {
            foreach (var trigger in _triggers)
            {
                if (!trigger.IsActivated) return;
            }
            
            _onAllActivated?.Invoke();
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}