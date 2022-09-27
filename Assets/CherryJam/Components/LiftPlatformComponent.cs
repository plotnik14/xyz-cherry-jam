using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components
{
    public class LiftPlatformComponent : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Transform _up;
        [SerializeField] private Transform _bottom;
        [SerializeField] private UnityEvent _onComplete;

        private Rigidbody2D _rigidbody;
        
        
        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            
            Activate();
        }

        public void Activate()
        {
            
        }


    }
}