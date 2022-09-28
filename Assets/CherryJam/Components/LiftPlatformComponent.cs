using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components
{
    public class LiftPlatformComponent : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _treshold;
        [SerializeField] private float _wait;
        [SerializeField] private bool _invertDirection; // ToDo
        [SerializeField] private Transform _up;
        [SerializeField] private Transform _bottom;
        [SerializeField] private UnityEvent _onComplete;

        private Rigidbody2D _rigidbody;
        
        
        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Activate()
        {
            StartCoroutine(StartMovement());
        }
        
        private IEnumerator StartMovement()
        {
            yield return Move(_up, Vector2.up);
            yield return new WaitForSeconds(_wait);
            yield return Move(_bottom, Vector2.down);
 
            _onComplete?.Invoke();
        }

        private IEnumerator Move(Transform point, Vector2 direction)
        {
            while (!IsOnPoint(point))
            {
                _rigidbody.velocity = direction * _speed;
                yield return null;
            }
            
            Stop();
        }

        private bool IsOnPoint(Transform point)
        {
            return (point.position - transform.position).magnitude < _treshold;
        }

        private void Stop()
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
}