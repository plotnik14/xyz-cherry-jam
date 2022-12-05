using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.Movement
{
    public class MovementToTargetComponent : MonoBehaviour
    {
        [SerializeField] protected Transform _destination;
        [SerializeField] private float _speed;
        [SerializeField] private float _reachTreshold;
        [SerializeField] private UnityEvent _OnReached;

        private Coroutine _current;
        
        private void OnEnable()
        {
            StartMovement();
        }

        public void StartMovement()
        {
            if (_current != null) return;
            
            _current = StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            var progress = 0f;
            
            while (!IsDestinationReached())
            {
                progress += _speed;
                transform.position = Vector3.Lerp(transform.position, _destination.position, progress);
                yield return null;
            }
            
            _OnReached?.Invoke();
            _current = null;
        }
        
        private bool IsDestinationReached()
        {
            return (_destination.position - transform.position).magnitude < _reachTreshold;
        }
    }
}