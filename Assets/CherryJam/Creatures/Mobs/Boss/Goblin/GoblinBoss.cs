
using System.Collections;
using CherryJam.Components.GoBased;
using CherryJam.Components.Health;
using UnityEngine;

namespace CherryJam.Creatures.Mobs.Boss.Goblin
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
        
        [SerializeField] public DirectionalSpawnComponent _rangeProjectileSpawnerCreatureRight;
        
        private Coroutine _current;
        private HealthComponent _health;
        private int _targetPointIndex = 0;
        
        private static readonly int IsOnPointKey = Animator.StringToHash("is-on-point");

        private Hero.Hero _heroStudent;
        
        protected override void Awake()
        {
            base.Awake();

            _health = GetComponent<HealthComponent>();
            _heroStudent = FindObjectOfType<Hero.Hero>();
        }
        

        public void LaunchPerlsDown()
        {
            _perlsDownSpawner.LaunchProjectiles();
        }
        
        public void Run()
        {
            if (_current != null) return;

            _current = StartCoroutine(RunToHero());
        }
        
        private IEnumerator RunToHero()
        {
            var point = _heroStudent.transform;
            
            while (!IsOnPoint(point))
            {
                UpdateRunDirection(point);
                yield return null;
            }

            StopCreature();
            _current = null;
            Animator.SetTrigger(IsOnPointKey);
        }
        
        private void StopCreature()
        {
            SetDirection(Vector2.zero);
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

        public void Spawn()
        {
            throw new System.NotImplementedException();
        }
        
        protected override void OnRangeAttackAnimationTriggered()
        {
            _rangeProjectileSpawnerCreature.Spawn(Vector2.left);
            _rangeProjectileSpawnerCreatureRight.Spawn2(Vector2.right);
        }
    }
}