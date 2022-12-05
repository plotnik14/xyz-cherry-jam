using System.Collections.Generic;
using CherryJam.Components.Health;
using CherryJam.Utils;
using UnityEngine;

namespace CherryJam.Creatures.Mobs.Boss.EvilFireflies
{
    public class EvilFirefly : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private List<Transform> _wayPoints;
        [SerializeField] private float _speed;
        [SerializeField] private float _onPoitTreshold;

        [Space][Header("Attack")]
        [SerializeField] private Cooldown _attackCooldown;

        private ModifyHealthComponent _modifyHealth;
        private Rigidbody2D _rigidbody;
        
        private Hero.Hero _hero;
        
        private Vector2 _direction;
        private Transform _nextPoint;
        private int _nextPointIndex;

        private void Awake()
        {
            _hero = FindObjectOfType<Hero.Hero>();
            
            _modifyHealth = GetComponent<ModifyHealthComponent>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (_wayPoints == null || _wayPoints.Count == 0) return;
            
            if (IsOnPoint(_nextPoint))
                SetNextWayPoint();

            UpdateDirection(_nextPoint);
            _rigidbody.velocity = _direction * _speed;
        }

        private void SetNextWayPoint()
        {
            _nextPointIndex = (_nextPointIndex + 1) % _wayPoints.Count;
            _nextPoint = _wayPoints[_nextPointIndex];
        }

        public void SetWayPoints(List<Transform> wayPoints)
        {
            _wayPoints = wayPoints;
            
            _nextPointIndex = 0;
            _nextPoint = _wayPoints[_nextPointIndex];
            UpdateDirection(_nextPoint);
        }
        
        public void Attack()
        {
            if (_attackCooldown.IsReady)
            {
                _modifyHealth.ApplyHealthChange(_hero.gameObject);
                _attackCooldown.Reset();
            }
        }
        
        private bool IsOnPoint(Transform point)
        {
            return (point.position - transform.position).magnitude < _onPoitTreshold;
        }
        
        private void UpdateDirection(Transform point)
        {
            var direction = point.position - transform.position;
            _direction = direction.normalized;
        }
    }
}