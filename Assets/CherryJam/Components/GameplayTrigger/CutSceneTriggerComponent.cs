using CherryJam.Model;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.GameplayTrigger
{
    public class CutSceneTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _triggerId;
        [SerializeField] private UnityEvent _onActivate;

        private void Start()
        {
            var hasBeenActivated = GameSession.Instance.IsTriggerActivated(_triggerId);
            if (hasBeenActivated) return;
            
            GameSession.Instance.ActivateTrigger(_triggerId);
            _onActivate?.Invoke();
        }
    }
}