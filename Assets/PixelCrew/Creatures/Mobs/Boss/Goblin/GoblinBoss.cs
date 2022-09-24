
using System.Collections;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.Goblin
{
    public class GoblinBoss : Creature
    {
        [Space][Header("Goblin Params")]
        [SerializeField] private float _jumpDuration;
        [SerializeField] private int _hpRestoredByEating;
        
        [Space][Header("Running Params")]
        [SerializeField] private Transform[] _runPoints;
        [SerializeField] private float _treshold;

        [Space] [Header("Spawners")] 
        [SerializeField] private GroupProjectileSpawner _perlsUpSpawner;
        [SerializeField] private GroupProjectileSpawner _perlsDownSpawner;
        
        private Coroutine _current;
        private HealthComponent _health;
        private int _targetPointIndex = 0;
        
        private static readonly int IsOnPointKey = Animator.StringToHash("is-on-point");

        protected override void Awake()
        {
            base.Awake();

            _health = GetComponent<HealthComponent>();
        }
        
        public void Jump()
        {
            if (_current == null)
                _current = StartCoroutine(Jumping());
        }

        private IEnumerator Jumping()
        {
            SetDirection(Vector2.up);
            yield return new WaitForSeconds(_jumpDuration);
            SetDirection(Vector2.zero);
            _current = null;
        }

        public void FallAction()
        {
            _perlsUpSpawner.LaunchProjectiles();
        }

        public void LaunchPerlsDown()
        {
            _perlsDownSpawner.LaunchProjectiles();
        }

        public void Eat()
        {
            _health.ApplyHealing(_hpRestoredByEating);
        }

        public void Run()
        {
            if (_current != null) return;

            _current = StartCoroutine(RunToPoint());
        }
        
        private IEnumerator RunToPoint()
        {
            var point = _runPoints[_targetPointIndex];
            
            while (!IsOnPoint(point))
            {
                UpdateRunDirection(point);
                yield return null;
            }
            
            // Turn to the next point
            _targetPointIndex = (int)Mathf.Repeat(_targetPointIndex + 1, _runPoints.Length);
            point = _runPoints[_targetPointIndex];
            UpdateRunDirection(point);
            yield return null;

            _current = null;
            Animator.SetTrigger(IsOnPointKey);
        }

        private void UpdateRunDirection(Transform point)
        {
            var direction = point.position - transform.position;
            direction.y = 0;
            SetDirection(direction.normalized);
        }

        private bool IsOnPoint(Transform point)
        {
            return (point.position - transform.position).magnitude < _treshold;
        }
    }
}