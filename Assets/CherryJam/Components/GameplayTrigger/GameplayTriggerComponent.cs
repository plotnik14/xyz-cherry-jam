using System;
using CherryJam.Utils.Disposables;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.GameplayTrigger
{
    public class GameplayTriggerComponent : MonoBehaviour
    {
        [SerializeField] protected bool _activated;
        [SerializeField] protected UnityEvent _onActivate;
        
        protected event Action OnChanged;

        public bool IsActivated => _activated;
        
        public virtual void Activate()
        {
            _activated = true;
            
            _onActivate?.Invoke();
            OnChanged?.Invoke();
        }
        
        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }
    }
}