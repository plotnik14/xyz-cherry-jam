using System.Collections;
using CherryJam.Components.GoBased;
using CherryJam.Components.Health;
using UnityEngine;

namespace CherryJam.Creatures.Mobs.Boss.Master
{
    public class BossMaster : Creature
    {
        [SerializeField] public DirectionalSpawnComponent _waveProjectileSpawner;
        
        [Space][Header("Running Params")]
        [SerializeField] private float _treshold;

        private Coroutine _current;
        private HealthComponent _health;
        private Hero.Hero _heroStudent;

        private static readonly int IsOnPointKey = Animator.StringToHash("is-on-point");
        private static readonly int WaveAttackKey = Animator.StringToHash("wave-attack");
        private static readonly int HeroInVisionKey = Animator.StringToHash("hero-in-vision");
        private static readonly int LostHeroKey = Animator.StringToHash("lost-hero");

        protected override void Awake()
        {
            base.Awake();

            _health = GetComponent<HealthComponent>();
            _heroStudent = FindObjectOfType<Hero.Hero>();
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
        
        public void WaveAttack()
        {
            Animator.SetTrigger(WaveAttackKey);
        } 
        
        protected void OnWaveAttackAnimationTriggered()
        {
            var direction = new Vector2( - transform.localScale.x, 0);
            _waveProjectileSpawner.Spawn(direction);
        }

        public void SetVision(bool isInVision)
        {
            Animator.SetBool(HeroInVisionKey, isInVision);

            if (!isInVision)
            {
                StopCurrentCoroutine();
                StopCreature();
                Animator.SetTrigger(LostHeroKey);
            }
        }

        private void StopCurrentCoroutine()
        {
            if (_current == null) return;
            StopCoroutine(_current);
            _current = null;
        }
    }
}